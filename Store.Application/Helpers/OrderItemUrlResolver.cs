using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.ModelView;
using Store.Core.Entities;

namespace Store.Application.Helpers
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _config;
        public OrderItemUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (source.ItemOrdered != null && !string.IsNullOrEmpty(source.ItemOrdered.PictureUrl))
            {
                return (_config["ApiUrl"] ?? string.Empty) + source.ItemOrdered.PictureUrl;
            }

            return string.Empty;
        }
    }
}
