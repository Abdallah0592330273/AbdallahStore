using System.Net.Http.Json;
using Store.Dashboard.Models;

namespace Store.Dashboard.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;

        public ApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ProductDto>?> GetProductsAsync() => 
            await _http.GetFromJsonAsync<List<ProductDto>>("products");

        public async Task<List<BrandDto>?> GetBrandsAsync() => 
            await _http.GetFromJsonAsync<List<BrandDto>>("products/brands");

        public async Task<List<CategoryDto>?> GetCategoriesAsync() => 
            await _http.GetFromJsonAsync<List<CategoryDto>>("products/categories");

        public async Task<List<TypeDto>?> GetTypesAsync() => 
            await _http.GetFromJsonAsync<List<TypeDto>>("products/types");

        public async Task<UserDto?> LoginAsync(LoginDto loginDto)
        {
            var response = await _http.PostAsJsonAsync("account/login", loginDto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserDto>();
            }
            return null;
        }
    }
}
