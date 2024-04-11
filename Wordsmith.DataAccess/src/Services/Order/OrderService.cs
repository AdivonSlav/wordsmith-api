using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.Integration.Paypal;
using Wordsmith.Integration.Paypal.Enums;
using Wordsmith.Integration.Paypal.Models;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Enums;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.RequestObjects.Order;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Utils;

namespace Wordsmith.DataAccess.Services.Order;

public class OrderService : WriteService<OrderDto, Db.Entities.Order, OrderSearchObject, OrderInsertRequest, OrderUpdateRequest>, IOrderService
{
    private readonly IPaypalService _paypalService;

    public OrderService(DatabaseContext context, IMapper mapper, IPaypalService paypalService) : base(context, mapper)
    {
        _paypalService = paypalService;
    }

    protected override async Task BeforeInsert(Db.Entities.Order entity, OrderInsertRequest insert, int userId)
    {
        await ValidateInsertion(insert);

        var ebook = await Context.EBooks.FirstAsync(e => e.Id == insert.EbookId);
        var user = await Context.Users.FirstAsync(e => e.Id == insert.UserId);
        
        var orderResponse = await _paypalService.CreateOrder(ebook.Price!.Value, ebook.Title, $"Ebook published by {user.Username}");
        MapNewOrderEntity(entity, user, ebook, orderResponse);
    }

    public async Task<EntityResult<OrderDto>> CapturePayment(int id)
    {
        var order = await ValidateOrder(id);
        var captureResponse = await _paypalService.CapturePayment(order.PayPalOrderId);

        ValidateCaptureResponse(captureResponse);
        
        order.PayPalCaptureId = captureResponse.PurchaseUnits.First().Payments.Captures.First().Id;
        await HandlePayout(captureResponse, order);

        order.Status = OrderStatus.Completed;
        order.PaymentDate = DateTime.UtcNow;

        await Context.SaveChangesAsync();

        return new EntityResult<OrderDto>()
        {
            Message = "Successfully completed payment",
            Result = Mapper.Map<OrderDto>(order)
        };
    }
    
    private async Task ValidateInsertion(OrderInsertRequest request)
    {
        var ebook = await Context.EBooks.Include(e => e.Author).FirstOrDefaultAsync(e => e.Id == request.EbookId);
        var user = await Context.Users.FindAsync(request.UserId);
        
        if (ebook == null)
        {
            throw new AppException("Ebook does not exist!");
        }
        
        if (ebook.Author.Status != UserStatus.Active)
        {
            throw new AppException("Cannot purchase this ebook as the author has been banned!");
        }
        
        if (user == null)
        {
            throw new AppException("User does not exist!");
        }
        
        if (await Context.UserLibraries.AnyAsync(e => e.UserId == request.UserId && e.EBookId == request.EbookId))
        {
            throw new AppException("User already owns the requested ebook!");
        }
        
        if (!ebook.Price.HasValue)
        {
            throw new AppException("Ebook is free. Cannot create an order for it");
        }
    }

    private async Task<Db.Entities.Order> ValidateOrder(int id)
    {
        var order = await Context.Orders.FindAsync(id);

        if (order == null)
        {
            throw new AppException("Order does not exist!");
        }

        if (order.Status == OrderStatus.Completed)
        {
            throw new AppException("Order is already completed!");
        }

        if (order.PayeeId == null)
        {
            throw new AppException("The seller's account no longer exists!");
        }

        if (order.PayerId == null)
        {
            throw new AppException("The buyer's account no longer exists!");
        }

        if (order.EBookId == null)
        {
            throw new AppException("The ebook being purchased no longer exists!");
        }

        return order;
    }

    private void ValidateCaptureResponse(PaypalCapturePaymentResponse response)
    {
        if (response.Status != PaypalOrderStatus.Completed)
        {
            var paymentStatusDescription =
                response.PurchaseUnits.FirstOrDefault()?.Payments.Captures.FirstOrDefault()?.Status.GetDescription() ?? "No reason found";
            throw new AppException($"Unable to complete the order at this time. {paymentStatusDescription}");
        }
    }

    private async Task HandlePayout(PaypalCapturePaymentResponse captureResponse, Db.Entities.Order order)
    {
        var capture = captureResponse.PurchaseUnits.First().Payments.Captures.First();
        var amountForPayout = capture.PaypalSellerReceivableBreakdown.NetAmount.Value;

        try
        {
            await _paypalService.CreatePayout(amountForPayout, order.PayeePayPalEmail,
                order.PayerUsername, order.EBookTitle);
        }
        catch (Exception e)
        {
            Logger.LogInfo($"PayPal payout failed, attempting refund for capture ID {capture.Id}");
            var refundResponse = await _paypalService.RefundPayment(capture.Id);

            if (refundResponse.Status == PaypalRefundStatus.Failed)
            {
                throw new Exception($"Failed to issue refund for capture ID {capture.Id}");
            }

            order.PayPalRefundId = refundResponse.Id;
            order.RefundDate = DateTime.UtcNow;
            order.Status = OrderStatus.Refunded;

            await Context.SaveChangesAsync();
            
            throw new Exception(
                $"Could not process order with ID {order.Id}. Refund of payment with capture ID ${capture.Id} completed");
        }
    }
    
    private Db.Entities.Order MapNewOrderEntity(Db.Entities.Order order, Db.Entities.User user, Db.Entities.EBook ebook,
        PaypalOrderResponse orderResponse)
    {
        order.OrderCreationDate = DateTime.UtcNow;
        order.Status = OrderStatus.Pending;
        order.ReferenceId = orderResponse.PurchaseUnits.First().ReferenceId;
        order.PayPalOrderId = orderResponse.Id;
        order.PayerId = user.Id;
        order.PayerUsername = user.Username;
        order.PayeeId = ebook.AuthorId;
        order.PayeeUsername = ebook.Author.Username;
        order.PayeePayPalEmail = ebook.Author.PayPalEmail;
        order.PaymentAmount = decimal.Parse(orderResponse.PurchaseUnits.First().Amount.Value);
        order.EBookId = ebook.Id;
        order.EBookTitle = ebook.Title;
        order.PaymentUrl = orderResponse.Links.First(x => x.Rel == "payer-action").Href;

        return order;
    }
}