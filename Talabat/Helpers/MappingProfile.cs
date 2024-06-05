using AutoMapper;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.DTOs;

namespace Talabat.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDTo>().ForMember(p => p.BrandName, o => o.MapFrom(p => p.ProductBrand.Name))
                 .ForMember(p => p.CategorieName, o => o.MapFrom(p => p.Productcategories.Name))
                 .ForMember(P => P.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());
            
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<CustomerBasket, CustomerBasketDto>();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();
            CreateMap<OrderAddressDto,Core.Entities.OrderAggregate.Address>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>().ForMember(d => d.DeliveryMethod,O=>O.MapFrom(S=>S.DeliveryMethod.ShortName))
                     .ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.Cost));
            CreateMap<OrderItem, OrderItemDto>()
                     .ForMember(d => d.ProductId, O => O.MapFrom(s => s.Product.ProductId))
                     .ForMember(d => d.Productname, O => O.MapFrom(s => s.Product.Productname))
                     .ForMember(d => d.PictureUrl, O => O.MapFrom(s => s.Product.PictureUrl))
                     .ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());

        }
    }
}
