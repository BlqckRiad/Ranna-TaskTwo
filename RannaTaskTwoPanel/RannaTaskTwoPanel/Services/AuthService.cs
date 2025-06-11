using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RannaTaskTwoPanel.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7014/api";

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            try
            {
                var loginRequest = new { username, password };
                var response = await _httpClient.PostAsJsonAsync("Auth/login", loginRequest);
                response.EnsureSuccessStatusCode();
                
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result.AccessToken;
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                throw new Exception($"Login failed: {ex.Message}");
            }
        }

        private class LoginResponse
        {
            public string AccessToken { get; set; }
            public string Expiration { get; set; }
        }
    }
} 