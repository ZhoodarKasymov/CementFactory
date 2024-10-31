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

        public async Task<List<Item>> GetProductsAsync()
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync(_baseUrl + "goods");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
                return products.Results.OrderBy(i => i.Name).ToList();
            }
        }
        
        public async Task<List<Item>> GetClientsWarehouses()
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync(_baseUrl + "customerWarehouse");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
                return products.Results.OrderBy(i => i.Name).ToList();
            }
        }
        
        public async Task<bool> GoodsMoving(SaleRequest saleRequest)
        {
            var senderWarehouseId = ConfigurationManager.AppSettings["GlobalCementWarehouseId"];
            saleRequest.WarehouseSender = senderWarehouseId;

            try
            {
                using (var client = CreateHttpClient())
                {
                    var jsonContent = JsonConvert.SerializeObject(saleRequest);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(_baseUrl + "goodsMoving", content);
                    response.EnsureSuccessStatusCode();
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(responseBody);
                    return result.Error;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return false;
            }
        }
    }
}