using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using RannaTaskTwoPanel.Models;

namespace RannaTaskTwoPanel.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7014/api";

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<List<Product>> GetAllProductsAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetFromJsonAsync<List<Product>>("products");
        }

        public async Task<Product> GetProductByIdAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetFromJsonAsync<Product>($"products/{id}");
        }

        public async Task<Product> CreateProductAsync(Product product, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsJsonAsync("products", product);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Product>();
        }

        public async Task<Product> UpdateProductAsync(Product product, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsJsonAsync($"products/{product.Id}", product);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Product>();
        }

        public async Task DeleteProductAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"products/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
} 