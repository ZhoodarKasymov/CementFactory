using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CementFactory.Models;
using Newtonsoft.Json;

namespace CementFactory.Services
{
    public class Service1C
    {
        private readonly string _baseUrl = "http://10.10.0.11:80/solt/hs/solt/";
        private readonly string _username = "Admin";
        private readonly string _password = "123";

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes($"{_username}:{_password}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            return client;
        }

        public async Task<List<Item>> GetAgentsAsync()
        {
            var isTest = bool.Parse(ConfigurationManager.AppSettings["IsTest"]);
            
            if (isTest)
            {
                return new List<Item>
                {
                    new Item
                    {
                        Guid = "asdasdasd",
                        Name = "Агент 1"
                    },
                    new Item
                    {
                        Guid = "asdasdasd",
                        Name = "Агент 2"
                    },
                    new Item
                    {
                        Guid = "asdasdasd",
                        Name = "Агент 3"
                    },
                    new Item
                    {
                        Guid = "asdasdasd",
                        Name = "Агент 4"
                    },
                };
            }
            
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync(_baseUrl + "clients");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var agents = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
                return agents.Results;
            }
        }

        public async Task<List<Item>> GetProductsAsync()
        {
            var isTest = bool.Parse(ConfigurationManager.AppSettings["IsTest"]);

            if (isTest)
            {
                return new List<Item>
                {
                    new Item
                    {
                        Guid = "asdasdasd",
                        Name = "Цемент"
                    },
                    new Item
                    {
                        Guid = "asdasdasd",
                        Name = "Цемент 2"
                    },
                    new Item
                    {
                        Guid = "asdasdasd",
                        Name = "Цемент 3"
                    },
                    new Item
                    {
                        Guid = "asdasdasd",
                        Name = "Цемент 4"
                    },
                };
            }
            
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync(_baseUrl + "goods");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
                return products.Results;
            }
        }

        public async Task<bool> SaveSaleAsync(SaleRequest saleRequest)
        {
            var isTest = bool.Parse(ConfigurationManager.AppSettings["IsTest"]);

            if (isTest) return true;
            
            using (var client = CreateHttpClient())
            {
                var jsonContent = JsonConvert.SerializeObject(saleRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(_baseUrl + "sales", content);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(responseBody);
                return !result.Error;
            }
        }
    }
}