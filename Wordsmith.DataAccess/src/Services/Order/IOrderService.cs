using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.Order;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.Order;

public interface IOrderService : IWriteService<OrderDto, Db.Entities.Order, OrderSearchObject, OrderInsertRequest, OrderUpdateRequest>
{
    
}