using System.Configuration;
using System.Windows.Forms;
using CementFactory.Models;

namespace CementFactory.Services
{
    public class PrintService
    {
        private readonly WebBrowser _webBrowser;

        public PrintService()
        {
            // Initialize the WebBrowser control without adding it to a form
            _webBrowser = new WebBrowser();
            _webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
        }

        public void PrintTruckInfo(Truck truck, string clientName, string fullNameAgent, string cubMetr, string quantity)
        {
            var htmlContent = GenerateHtmlTemplate(truck, clientName, fullNameAgent, cubMetr, quantity);
            _webBrowser.DocumentText = htmlContent;
        }

        #region Private Methods

        private string GenerateHtmlTemplate(Truck truck, string clientName, string driverFullName, string cubMetr, string quantity)
        {
            var htmlTemplate = @"
            <html>
                <head>
                    <style>
                        @page {
                            size: A4; /* Ensure it fits A4 */
                            margin: 10mm;
                        }
                        body {
                            font-family: Arial, sans-serif;
                        }
                        .invoice-title h1 {
                            text-align: center;
                            font-size: 28px;
                            margin-top: 10px;
                            font-family: Arial, sans-serif;
                        }
                        .d-flex {
                            display: flex;
                            align-items: center;
                        }
                        .col-6, .col-12 {
                            position: relative;
                            padding-right: 15px;
                            padding-left: 15px;
                        }
                        .col-6 {
                            flex: 0 0 50%;
                            max-width: 50%;
                        }
                        .col-12 {
                            flex: 0 0 100%;
                            max-width: 100%;
                        }
                        .table {
                            width: 100%;
                            margin-bottom: 1rem;
                            color: #212529;
                            border: 1px solid black;
                            border-collapse: collapse;
                        }
                        .table-bordered {
                            border: 1px solid black;
                        }
                        .table th, .table td {
                            padding: 8px;
                            text-align: left;
                            border: 1px solid black;
                            font-family: Arial, sans-serif;
                        }
                        .table-no-border {
                            border-collapse: collapse;
                            width: 100%;
                        }
                        .table-no-border td {
                            border: none;
                            padding: 5px;
                            font-family: Arial, sans-serif;
                            padding-left: 0;
                        }
                        .section-title {
                            background-color: #f2f2f2;
                            padding: 8px;
                            border: 1px solid black;
                            margin-bottom: 10px;
                            font-family: Arial, sans-serif;
                        }
                        .mt-2 {
                            margin-top: 10px;
                        }
                        .mt-4 {
                            margin-top: 20px;
                        }
                    </style>
                </head>
                <body>

                    <div class=""container"">
                        <!-- Title Section -->
                        <div class=""row"">
                            <div class=""col-12 invoice-title"">
                                <h1>Накладная №{{ID}}</h1>
                            </div>
                        </div>
                        
                        <!-- Date and Time Section -->
                        <div class=""row mt-4"">
                            <div class=""col-6 d-flex"">
                                <p style=""margin-right: 10px;""><strong>Дата:</strong> {{Date}}</p>
                                <p><strong>Время:</strong> {{Time}}</p>
                            </div>
                        </div>
                        
                        <!-- Sender and Receiver Section -->
                        <div class=""row"">
                            <div class=""col-12"">
                                <table class=""table-no-border"">
                                    <tr>
                                        <td><strong>Отправитель:</strong> {{CompanyName}} ИНН: {{INN}}</td>
                                    </tr>
                                    <tr>
                                        <td>{{Address}}</td>
                                    </tr>
                                </table>
                            </div>
                            <div class=""col-12"">
                                <table class=""table-no-border mt-2"">
                                    <tr>
                                        <td><strong>Получатель:</strong> {{Client}}</td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                        <!-- Table Section -->
                        <div class=""row mt-4"">
                            <div class=""col-12"">
                                <table class=""table table-bordered"" style=""width: 97%;"">
                                    <thead>
                                        <tr>
                                            <th>№</th>
                                            <th>Наименование</th>
                                            <th>Единица измерения</th>
                                            <th>Количество</th>
                                            <th>Выход</th>
                                            <th>Вес(кг)</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>1</td>
                                            <td>{{TypeCargo}}</td>
                                            <td>куб.м</td>
                                            <td>{{CubMetr}}</td>
                                            <td>{{Quantity}}</td>
                                            <td>{{WeightFull}}</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>

                        <!-- Signatures Section -->
                        <div class=""row mt-4"">
                            <div class=""col-12"">
                                <table class=""table-no-border"">
                                    <tr>
                                        <td><strong>Отпустил:</strong> {{CompanyName}}</td>
                                        <td><strong>Принял:</strong> {{Client}}</td>
                                    </tr>
                                    <tr>
                                        <td><strong>ФИО:</strong> {{CompanyFullName}}</td>
                                        <td><strong>ФИО:</strong> ____________________________</td>
                                    </tr>
                                    <tr>
                                        <td><strong>Подпись:</strong> _________________________</td>
                                        <td><strong>Подпись:</strong> _________________________</td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                        <!-- Driver Info Section -->
                        <div class=""row mt-4"">
                            <div class=""col-12"">
                                <table class=""table-no-border"">
                                    <tr>
                                        <td><strong>Гос номер авто:</strong> {{PlateNumber}}</td>
                                    </tr>
                                    <tr>
                                        <td><strong>Доставку принял водитель:</strong> {{DriverFullName}}</td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <br>
                    <hr>
                    <div class=""container"">
                        <!-- Title Section -->
                        <div class=""row"">
                            <div class=""col-12 invoice-title"">
                                <h1>Накладная №{{ID}}</h1>
                            </div>
                        </div>
                        
                        <!-- Date and Time Section -->
                        <div class=""row mt-4"">
                            <div class=""col-6 d-flex"">
                                <p style=""margin-right: 10px;""><strong>Дата:</strong> {{Date}}</p>
                                <p><strong>Время:</strong> {{Time}}</p>
                            </div>
                        </div>
                        
                        <!-- Sender and Receiver Section -->
                        <div class=""row"">
                            <div class=""col-12"">
                                <table class=""table-no-border"">
                                    <tr>
                                        <td><strong>Отправитель:</strong> {{CompanyName}} ИНН: {{INN}}</td>
                                    </tr>
                                    <tr>
                                        <td>{{Address}}</td>
                                    </tr>
                                </table>
                            </div>
                            <div class=""col-12"">
                                <table class=""table-no-border mt-2"">
                                    <tr>
                                        <td><strong>Получатель:</strong> {{Client}}</td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                        <!-- Table Section -->
                        <div class=""row mt-4"">
                            <div class=""col-12"">
                                <table class=""table table-bordered"" style=""width: 97%;"">
                                    <thead>
                                        <tr>
                                            <th>№</th>
                                            <th>Наименование</th>
                                            <th>Единица измерения</th>
                                            <th>Количество</th>
                                            <th>Выход</th>
                                            <th>Вес(кг)</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>1</td>
                                            <td>{{TypeCargo}}</td>
                                            <td>куб.м</td>
                                            <td>{{CubMetr}}</td>
                                            <td>{{Quantity}}</td>
                                            <td>{{WeightFull}}</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>

                        <!-- Signatures Section -->
                        <div class=""row mt-4"">
                            <div class=""col-12"">
                                <table class=""table-no-border"">
                                    <tr>
                                        <td><strong>Отпустил:</strong> {{CompanyName}}</td>
                                        <td><strong>Принял:</strong> {{Client}}</td>
                                    </tr>
                                    <tr>
                                        <td><strong>ФИО:</strong> {{CompanyFullName}}</td>
                                        <td><strong>ФИО:</strong> ____________________________</td>
                                    </tr>
                                    <tr>
                                        <td><strong>Подпись:</strong> _________________________</td>
                                        <td><strong>Подпись:</strong> _________________________</td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                        <!-- Driver Info Section -->
                        <div class=""row mt-4"">
                            <div class=""col-12"">
                                <table class=""table-no-border"">
                                    <tr>
                                        <td><strong>Гос номер авто:</strong> {{PlateNumber}}</td>
                                    </tr>
                                    <tr>
                                        <td><strong>Доставку принял водитель:</strong> {{DriverFullName}}</td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </body>
            </html>";

            var companyName = ConfigurationManager.AppSettings["CompanyName"];
            var fullNameCompanyOwner = ConfigurationManager.AppSettings["FullName"];
            var inn = ConfigurationManager.AppSettings["INN"];
            var address = ConfigurationManager.AppSettings["Address"];

            // Replace placeholders with actual truck data
            htmlTemplate = htmlTemplate
                .Replace("{{PlateNumber}}", truck.plate_number)
                .Replace("{{TypeCargo}}", truck.type_cargo)
                .Replace("{{Date}}", truck.Date.ToString("dd/MM/yyyy"))
                .Replace("{{Time}}", truck.Date.ToString("HH:mm"))
                .Replace("{{CompanyName}}", companyName)
                .Replace("{{CompanyFullName}}", fullNameCompanyOwner)
                .Replace("{{INN}}", inn)
                .Replace("{{Address}}", address)
                .Replace("{{Client}}", clientName)
                .Replace("{{DriverFullName}}", driverFullName)
                .Replace("{{CubMetr}}", cubMetr)
                .Replace("{{Quantity}}", quantity)
                .Replace("{{ID}}", truck.Id.ToString())
                .Replace("{{WeightFull}}", truck.weight_full.ToString("F2"));

            return htmlTemplate;
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            _webBrowser.Print();
        }

        #endregion
    }
}