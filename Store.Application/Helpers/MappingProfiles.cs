using AutoMapper;
using Store.ModelView;
using Store.Core.Entities;

namespace Store.Application.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand != null ? s.ProductBrand.Name : string.Empty))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType != null ? s.ProductType.Name : string.Empty))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category != null ? s.Category.Name : string.Empty))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
            
            CreateMap<ProductCreateDto, Product>();

            CreateMap<ApplicationUser, UserDto>().ReverseMap();
            CreateMap<ProductBrand, BrandDto>().ReverseMap();
            CreateMap<ProductType, TypeDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<OrderAddress, AddressDto>();

            CreateMap<Cart, CartDto>();
            CreateMap<CartItem, CartItemDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Product.Price))
                .ForMember(d => d.Brand, o => o.MapFrom(s => s.Product.ProductBrand != null ? s.Product.ProductBrand.Name : string.Empty))
                .ForMember(d => d.Type, o => o.MapFrom(s => s.Product.ProductType != null ? s.Product.ProductType.Name : string.Empty))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<CartItemUrlResolver>()); // We need one more resolver or simpler logic

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price))
                .ForMember(d => d.Total, o => o.MapFrom(s => s.GetTotal()));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
        }
    }
}
