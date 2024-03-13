using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.Integration.PayPal;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.RequestObjects.Order;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.Order;

public class OrderService : WriteService<OrderDto, Db.Entities.Order, OrderSearchObject, OrderInsertRequest, OrderUpdateRequest>, IOrderService
{
    private readonly IPayPalService _payPalService;

    public OrderService(DatabaseContext context, IMapper mapper, IPayPalService payPalService) : base(context, mapper)
    {
        _payPalService = payPalService;
    }

    protected override async Task BeforeInsert(Db.Entities.Order entity, OrderInsertRequest insert)
    {
        var user = await ValidateUser(insert);
        var ebook = await ValidateEbook(insert);
        
        var order = await _payPalService.CreateOrder(ebook.Price!.Value, ebook.Title, $"Ebook published by {user.Username}");
        
        entity.OrderCreationDate = DateTime.UtcNow;
        entity.Status = Db.Entities.Order.OrderStatus.Pending;
        entity.ReferenceId = order.PurchaseUnits.First().ReferenceId;
        entity.PayPalOrderId = order.Id;
        entity.PayerId = user.Id;
        entity.PayerUsername = user.Username;
        entity.PayeeId = ebook.AuthorId;
        entity.PayeeUsername = ebook.Author.Username;
        entity.PayeePayPalEmail = ebook.Author.PayPalEmail;
        entity.PaymentAmount = decimal.Parse(order.PurchaseUnits.First().Amount.Value);
        entity.EBookId = ebook.Id;
        entity.EBookTitle = ebook.Title;
    }

    private async Task<Db.Entities.EBook> ValidateEbook(OrderInsertRequest request)
    {
        var ebook = await Context.EBooks.Include(e => e.Author).FirstOrDefaultAsync(e => e.Id == request.EbookId);
        
        if (ebook == null)
        {
            throw new AppException("Ebook does not exist!");
        }

        if (!ebook.Price.HasValue)
        {
            throw new AppException("Ebook is free. Cannot create an order for it");
        }

        return ebook;
    }

    private async Task<Db.Entities.User> ValidateUser(OrderInsertRequest request)
    {
        var user = await Context.Users.FindAsync(request.UserId);
        
        if (user == null)
        {
            throw new AppException("User does not exist!");
        }

        if (await Context.UserLibraries.AnyAsync(e => e.UserId == request.UserId && e.EBookId == request.EbookId))
        {
            throw new AppException("User already owns the requested ebook!");
        }

        return user;
    }
}