using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.Order;

namespace Wordsmith.DataAccess.AutoMapper;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<OrderInsertRequest, Order>()
            .ForAllMembers(memberOptions: options =>
            {
                options.Ignore();
            });
    }
}