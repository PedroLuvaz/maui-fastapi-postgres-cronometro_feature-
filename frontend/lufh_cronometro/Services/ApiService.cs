using System.Net.Http.Json;
using System.Text.Json;

namespace LUFH_Cronometro.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        
        private static string GetBaseUrl()
        {
#if ANDROID
            return "http://10.0.2.2:8000";
#elif IOS
            return "http://127.0.0.1:8000";
#else
            return "http://127.0.0.1:8000";
#endif
        }

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(GetBaseUrl()),
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        public async Task<bool> IsApiAvailableAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/health");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> LoginAsync(string email, string senha)
        {
            try
            {
                var loginData = new { email, senha };
                var response = await _httpClient.PostAsJsonAsync("/usuarios/login", loginData);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<T>> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<T>();
            }
            catch
            {
                return new List<T>();
            }
        }

        public async Task<bool> PostAsync<T>(string endpoint, T data)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, data);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> PutAsync<T>(string endpoint, int id, T data)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{endpoint}/{id}", data);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string endpoint, int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{endpoint}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}