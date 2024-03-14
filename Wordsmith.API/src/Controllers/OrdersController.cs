using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services.Order;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.Order;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("orders")]
public class OrdersController : WriteController<OrderDto, Order, OrderSearchObject, OrderInsertRequest, OrderUpdateRequest>
{
    public OrdersController(IOrderService orderService) : base(orderService) { }

    [SwaggerOperation("Create a new order for an ebook")]
    [Authorize("All")]
    [HttpPost]
    public override Task<ActionResult<EntityResult<OrderDto>>> Insert(OrderInsertRequest insert)
    {
        insert.UserId = GetAuthUserId();
        return base.Insert(insert);
    }

    [SwaggerOperation("Capture payment for an order")]
    [Authorize("All")]
    [HttpPost("{id:int}/capture")]
    public async Task<ActionResult<EntityResult<OrderDto>>> CapturePayment(int id)
    {
        return await (WriteService as IOrderService)!.CapturePayment(id);
    }

    [NonAction]
    public override Task<ActionResult<EntityResult<OrderDto>>> Update(int id, OrderUpdateRequest update)
    {
        return base.Update(id, update);
    }
}