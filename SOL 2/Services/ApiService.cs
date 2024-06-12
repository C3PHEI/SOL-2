using MongoDB.Bson;
using Newtonsoft.Json;
using SOL_2.Models;
using SOL_2_API.Dtos;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SOL_2.Services
{
    public class ApiService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<LoginResponse> LoginAsync(User user)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7094/api/Users/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LoginResponse>(responseBody);
            }
            return null;
        }

        public async Task<List<CashRegisterDto>> GetCashRegisterByUserIdAsync(string userId)
        {
            var response = await client.GetAsync($"https://localhost:7094/api/CashRegister/byuserid/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CashRegisterDto>>(responseBody);
            }
            return null;
        }

        public async Task<bool> UpdateCashRegisterByUserIdAsync(string userId, CashRegister updatedCashRegister)
        {
            var json = JsonConvert.SerializeObject(updatedCashRegister);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"https://localhost:7094/api/CashRegister/byuserid/{userId}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RequestRefillAsync(string userId)
        {
            var restockRequest = new Restock
            {
                UserId = userId,
                Nachfullung = true
            };

            var json = JsonConvert.SerializeObject(restockRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"https://localhost:7094/api/nachfullung/byuserid/{userId}", content);
            return response.IsSuccessStatusCode;
        }
    }

    public class LoginResponse
    {
        public string Message { get; set; }
        public string UserId { get; set; }
    }
}