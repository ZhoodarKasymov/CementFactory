using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CementFactory.Models;
using Newtonsoft.Json;
using Serilog;

namespace CementFactory.Services
{
    public class Service1C
    {
        private readonly string _username = "Admin";
        private readonly string _password = "123";

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes($"{_username}:{_password}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            return client;
        }

        public async Task<List<Item>> GetProductsAsync()
        {
#if DEBUG
            return new List<Item>
            {
                new Item
                {
                    Guid = "test product",
                    Name = "Test product",
                },
                new Item
                {
                    Guid = "test product 2",
                    Name = "Test product 2",
                },
            };
#endif
            var ip = ConfigurationManager.AppSettings["1CServer"];
            var baseUrl = $"http://{ip}/solt/hs/solt/";
            
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync(baseUrl + "goods");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
                return products.Results.OrderBy(i => i.Name).ToList();
            }
        }
        
        public async Task<List<Item>> GetClientsWarehouses()
        {
#if DEBUG
            return new List<Item>
            {
                new Item
                {
                    Guid = "test client",
                    Name = "Test Client",
                },
                new Item
                {
                    Guid = "test client 2",
                    Name = "Test Client 2",
                },
            };
#endif
            var ip = ConfigurationManager.AppSettings["1CServer"];
            var baseUrl = $"http://{ip}/solt/hs/solt/";
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync(baseUrl + "customerWarehouse");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
                return products.Results.OrderBy(i => i.Name).ToList();
            }
        }
        
        public async Task<bool> GoodsMoving(SaleRequest saleRequest)
        {
#if DEBUG
            return true;
#endif
            
            var senderWarehouseId = ConfigurationManager.AppSettings["GlobalCementWarehouseId"];
            saleRequest.WarehouseSender = senderWarehouseId;

            try
            {
                var ip = ConfigurationManager.AppSettings["1CServer"];
                var baseUrl = $"http://{ip}/solt/hs/solt/";
                
                using (var client = CreateHttpClient())
                {
                    var jsonContent = JsonConvert.SerializeObject(saleRequest);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(baseUrl + "goodsMoving", content);
                    response.EnsureSuccessStatusCode();
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(responseBody);
                    return result.Error;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + " 1C service goodsMoving", ex);
                return false;
            }
        }
    }
}