using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.ModelView;
using Store.Core.Entities;

namespace Store.Application.Helpers
{
    public class CartItemUrlResolver : IValueResolver<CartItem, CartItemDto, string>
    {
        private readonly IConfiguration _config;
        public CartItemUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(CartItem source, CartItemDto destination, string destMember, ResolutionContext context)
        {
            if (source.Product != null && !string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                return (_config["ApiUrl"] ?? string.Empty) + source.Product.PictureUrl;
            }

            return string.Empty;
        }
    }
}
