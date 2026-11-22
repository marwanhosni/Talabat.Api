using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Orders;

namespace Talabat.APIs.Helpers;

public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
{
    private readonly IConfiguration _cong;

    public OrderItemPictureUrlResolver(IConfiguration cong)
    {
        _cong = cong;
    }
    public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.Product.PictureUrl))
        {
            return $"{_cong["BaseUrl"]}/{source.Product.PictureUrl}";
        }
        else
        {
            return string.Empty;
        }
    }
}


