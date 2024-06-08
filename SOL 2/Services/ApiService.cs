﻿using Newtonsoft.Json;
using SOL_2.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SOL_2.Services
{
    public class ApiService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<TokenResponse> LoginAsync(User user)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://yourapiurl.com/api/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TokenResponse>(responseBody);
            }
            return null;
        }

        public async Task<CashRegister> GetCashRegisterByUserIdAsync(string userId)
        {
            var response = await client.GetAsync($"https://yourapiurl.com/api/cashregister/byuserid/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CashRegister>(responseBody);
            }
            return null;
        }

        public async Task<bool> UpdateCashRegisterByUserIdAsync(string userId, CashRegister updatedCashRegister)
        {
            var json = JsonConvert.SerializeObject(updatedCashRegister);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"https://yourapiurl.com/api/cashregister/byuserid/{userId}", content);
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

            var response = await client.PutAsync($"https://yourapiurl.com/api/nachfullung/byuserid/{userId}", content);
            return response.IsSuccessStatusCode;
        }
    }
}
