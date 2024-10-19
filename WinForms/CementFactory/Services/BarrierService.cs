using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;

namespace CementFactory.Services
{
    public class BarrierService
    {
        public async Task<double?> StartWeightRequests()
        {
            var maxRequestDurationMinutes = int.Parse(ConfigurationManager.AppSettings["MaxRequestDurationMinutes"]);
            var requestIntervalSeconds = int.Parse(ConfigurationManager.AppSettings["RequestIntervalSeconds"]);
            int consecutiveMatches = default;
            double previousWeight = default;
            
            var startTime = DateTime.Now;
            var maxDuration = TimeSpan.FromMinutes(maxRequestDurationMinutes);

            while ((DateTime.Now - startTime) < maxDuration)
            {
                var currentWeight = await GetWeightData();

                if (currentWeight == previousWeight)
                {
                    consecutiveMatches++;
                }
                else
                {
                    consecutiveMatches = 0;
                }

                previousWeight = currentWeight;

                // Если вес совпадает 2 раза подряд, возвращаем результат и останавливаем запросы
                if (consecutiveMatches >= 2)
                {
                    MessageBox.Show($"Вес стабилизировался: {currentWeight} кг");
                    break;
                }

                // Ожидание перед следующим запросом (интервал в секундах)
                await Task.Delay(requestIntervalSeconds * 1000);
            }

            if (consecutiveMatches < 2)
            {
                MessageBox.Show($"По истечению минут: {maxRequestDurationMinutes} весы не стабилизировались!");
                return null;
            }

            return previousWeight;
        }

        public async Task CloseOpenBarrier(string value)
        {
            var ip = ConfigurationManager.AppSettings["BarrierServer"];
            var formDataUrl = $"http://{ip}/formdata.cgi";

            // Создание данных для отправки в запросе
            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("usertext1", value)
            });

            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Выполнение POST-запроса для управления шлагбаумом
                    var formDataResponse = await httpClient.PostAsync(formDataUrl, formData);
                    formDataResponse.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка запроса: " + ex.Message);
            }
        }
        
        public async Task<double> GetWeightData()
        {
            var ip = ConfigurationManager.AppSettings["BarrierServer"];
            
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = $"http://{ip}/weight.cgi";
                    var response = await httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var responseData = await response.Content.ReadAsStringAsync();
                    
                    if (double.TryParse(responseData, out double weight))
                    {
                        return weight;
                    }

                    MessageBox.Show("Невозможно преобразовать ответ сервера для получения веса!");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка запроса: " + ex.Message);
                return 0;
            }
        }
    }
}