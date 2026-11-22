using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using UserAddress = Talabat.Core.Entities.Identity.Address;
using OrderAddress = Talabat.Core.Entities.Orders.Address;
using Talabat.Core.Entities.Orders;

namespace Talabat.APIs.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductToReturnDto>()
            .ForMember(D => D.Brand, O => O.MapFrom(S => S.Brand.Name))
            .ForMember(D => D.Category, O => O.MapFrom(S => S.Category.Name))
            .ForMember(D => D.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());
       
        CreateMap<RegisterDto, AppUser>()
           .ForMember(
               destination => destination.UserName,
               options => options.MapFrom(source => source.Email.Split(
                new[] { '@' },
                StringSplitOptions.None)[0]));
        CreateMap<BasketItemDto, BasketItem>()
         .ReverseMap();

        CreateMap< CustomerBasket, CustmorBasketDto>()
            .ReverseMap();

        CreateMap<RegisterDto, AppUser>().ReverseMap();
        CreateMap<UserAddress, AddressDto>().ReverseMap();
        CreateMap<OrderAddress, AddressDto>().ReverseMap();

        CreateMap<Order, OrderToReturnDto>()
            .ForMember(o => o.DeliveryMethods, o => o.MapFrom(o => o.DeliveryMethods.ShortName))
            .ForMember(o => o.DeliveryMethodsCost, o => o.MapFrom(o => o.DeliveryMethods.Cost));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(o => o.ProductId, o => o.MapFrom(s => s.Product.ProductId))
            .ForMember(o => o.ProductName, o => o.MapFrom(s => s.Product.ProductName))
            .ForMember(o => o.PictureUrl, o => o.MapFrom(s=>s.Product.PictureUrl))
            .ForMember(D => D.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());

    }
}
