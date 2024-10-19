using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using CementFactory.Models;
using Newtonsoft.Json;
using Serilog;

namespace CementFactory.Services
{
    public class DahuaService
    {
        public async Task<string> SavePicture()
        {
            var photoCamIp = ConfigurationManager.AppSettings["PhotoCam"];
            var loginCam = ConfigurationManager.AppSettings["ANPRLogin"];
            var pswCam = ConfigurationManager.AppSettings["ANPRPwd"];
            var ip = ConfigurationManager.AppSettings["IpToFolder"];
            var url = $"http://{photoCamIp}/cgi-bin/snapshot.cgi";

            using (var handler = new HttpClientHandler { Credentials = new NetworkCredential(loginCam, pswCam) })
            {
                handler.PreAuthenticate = true;
                handler.UseDefaultCredentials = false;

                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Digest");
                    
                    try
                    {
                        var response = await client.GetAsync(url);
                        if (response.IsSuccessStatusCode)
                        {
                            var snapshot = await response.Content.ReadAsByteArrayAsync();
                            var fileName = await UploadImageToServerAsync(snapshot);
                            var savePath = $"http://{ip}/{fileName}";
                            return savePath;
                        }
                        else
                        {
                            MessageBox.Show($"Ошибка при сохранении изображения!");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка сервера!");
                    }
                }
            }

            return null;
        }

        public PlateRecord GetPlateNumber(string host)
        {
            // Path to Python executable and script
            var pythonExePath = ConfigurationManager.AppSettings["PythonExePath"];

            var isTest = bool.Parse(ConfigurationManager.AppSettings["IsTest"]);

            var scriptPath = Path.Combine(Application.StartupPath, "Scripts",
                isTest ? "test.py" : "anpr_get_plate_numbers.py");

            // Dahua connection details
            var username = ConfigurationManager.AppSettings["ANPRLogin"];
            var password = ConfigurationManager.AppSettings["ANPRPwd"];
            var minutes = int.Parse(ConfigurationManager.AppSettings["Minutes"]);

            var now = DateTime.Now.AddMinutes(-minutes);

            // Specific date and time for the script (input values for year, month, day, hour, minute)
            var year = now.ToString("yyyy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");
            var hour = now.ToString("HH");
            var minute = now.ToString("mm");

            // Prepare the arguments to pass to the Python script
            var arguments = $"{scriptPath} {host} {username} {password} {year} {month} {day} {hour} {minute}";
            
            Log.Debug(arguments);

            // Run the Python script and capture output
            var resultJson = RunPythonScript(pythonExePath, arguments);

            Log.Debug(resultJson);

            var records = JsonConvert.DeserializeObject<List<PlateRecord>>(resultJson);

            var mostRecentRecord = records
                .OrderByDescending(r => r.ParsedTime)
                .FirstOrDefault();

            return mostRecentRecord;
        }

        private async Task<string> UploadImageToServerAsync(byte[] snapshot)
        {
            var ipAdminSite = ConfigurationManager.AppSettings["IpAdminSite"];
            
            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                // Create ByteArrayContent to add to the request
                var fileContent = new ByteArrayContent(snapshot);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

                // Add the file content to the request with a form field name 'file'
                content.Add(fileContent, "file", $"{Guid.NewGuid():N}.jpg");

                // Send the POST request to the server API
                var response = await client.PostAsync($"http://{ipAdminSite}/api/file-upload/upload", content);

                response.EnsureSuccessStatusCode(); // Throws exception if not 2xx status

                // Get the response content as string
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
        }

        private string RunPythonScript(string pythonExePath, string arguments)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = pythonExePath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();

                    // Read the output from the Python script
                    var output = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    return !string.IsNullOrEmpty(error) ? $"Error: {error}" : output;
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }

        private async Task<Image> DownloadSnapshot(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    return Image.FromStream(stream);
                }
            }
        }
    }
}