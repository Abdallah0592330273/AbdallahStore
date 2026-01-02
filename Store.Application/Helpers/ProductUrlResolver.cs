using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.ModelView;
using Store.Core.Entities;

namespace Store.Application.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _config;
        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return (_config["ApiUrl"] ?? string.Empty) + source.PictureUrl;
            }

            return string.Empty;
        }
    }
}
