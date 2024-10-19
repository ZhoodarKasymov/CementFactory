using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using CementFactory.Models;
using CementFactory.Services;
using LibVLCSharp.Shared;
using Serilog;

namespace CementFactory
{
    public partial class MainForm : Form
    {
        private readonly LibVLC _libVlc;
        private readonly MediaPlayer _mediaPlayer;
        private readonly MediaPlayer _mediaPlayer2;
        private readonly MediaPlayer _mediaPlayer3;
        private readonly TruckService _truckService;
        private readonly PrintService _printService;
        private readonly Service1C _service1C;
        private readonly DahuaService _dahuaService;
        private readonly BarrierService _barrierService;
        private readonly System.Timers.Timer _statusTimer;

        public MainForm()
        {
            InitializeComponent();
            Core.Initialize();

            _truckService = new TruckService();
            _printService = new PrintService();
            _service1C = new Service1C();
            _dahuaService = new DahuaService();
            _barrierService = new BarrierService();
            
            _libVlc = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVlc);
            _mediaPlayer2 = new MediaPlayer(_libVlc);
            _mediaPlayer3 = new MediaPlayer(_libVlc);

            videoView1.MediaPlayer = _mediaPlayer;
            videoView2.MediaPlayer = _mediaPlayer2;
            videoView3.MediaPlayer = _mediaPlayer3;
            
#if !DEBUG
            // Initialize Timer with 1 second interval
            _statusTimer = new System.Timers.Timer(1000); // 1 seconds
            _statusTimer.Elapsed += async (sender, e) => await CheckBarrierStatusAsync();
            _statusTimer.Elapsed += async (sender, e) => await CheckWeightStatusAsync();
            _statusTimer.AutoReset = true;
            _statusTimer.Enabled = true;
#endif
        }

        private void history_trucks_button(object sender, EventArgs e)
        {
            var truckForm = new Trucks();
            truckForm.Show();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            var goods = await _service1C.GetProductsAsync();
            var clients = await _service1C.GetAgentsAsync();
            var statuses = new List<Item>()
            {
                new Item
                {
                    Name = "--Выберите статус--", 
                    Guid = ""
                },
                new Item
                {
                    Name = Constants.Constants.TruckEmptyStatus, 
                    Guid = Constants.Constants.TruckEmptyStatus
                },
                new Item
                {
                    Name = Constants.Constants.TruckFullStatus, 
                    Guid = Constants.Constants.TruckFullStatus
                },
                new Item
                {
                    Name = Constants.Constants.TruckReceiveStatus, 
                    Guid = Constants.Constants.TruckReceiveStatus
                }
            };
            
            goods.Insert(0, new Item { Name = "--Выберите продукт--", Guid = "" });
            clients.Insert(0, new Item { Name = "--Выберите клиента--", Guid = "" });
            
            typeCargoBox.DataSource = goods;
            typeCargoBox.DisplayMember = "Name";
            typeCargoBox.ValueMember = "Guid";
            
            clientsBox.DataSource = clients;
            clientsBox.DisplayMember = "Name";
            clientsBox.ValueMember = "Guid";
            
            currrentStatusBox.DataSource = statuses;
            currrentStatusBox.DisplayMember = "Name";
            currrentStatusBox.ValueMember = "Guid";
            
            typeCargoBox.SelectedIndex = 0;
            clientsBox.SelectedIndex = 0;
            
            // Hide when init
            VisibleNotVisibleButtons(false);
            showRecognizedCumBoxes(false);
            saveRecognizedPlateButton.Visible = false;
            cancelRecognizedPlate.Visible = false;
            
            var firstCamIp = ConfigurationManager.AppSettings["EnterANPRCam"];
            var secondCamIp = ConfigurationManager.AppSettings["ExitANPRCam"];
            var thirdCamIp = ConfigurationManager.AppSettings["PhotoCam"];
            var loginCam = ConfigurationManager.AppSettings["ANPRLogin"];
            var pswCam = ConfigurationManager.AppSettings["ANPRPwd"];

            //rtsp://admin:admin123@192.168.1.108:554/cam/realmonitor?channel=1&subtype=0
            var rtspUrl1 = $"rtsp://{loginCam}:{pswCam}@{firstCamIp}:554/cam/realmonitor?channel=1&subtype=0";
            var rtspUrl2 = $"rtsp://{loginCam}:{pswCam}@{secondCamIp}:554/cam/realmonitor?channel=1&subtype=0";
            var rtspUrl3 = $"rtsp://{loginCam}:{pswCam}@{thirdCamIp}:554/cam/realmonitor?channel=1&subtype=0";

#if DEBUG
            rtspUrl1 = "http://67.53.46.161:65123/mjpg/video.mjpg";
            rtspUrl2= "http://67.53.46.161:65123/mjpg/video.mjpg";
            rtspUrl3 = "http://67.53.46.161:65123/mjpg/video.mjpg";
#endif
            _mediaPlayer.Play(new Media(_libVlc, rtspUrl1, FromType.FromLocation));
            _mediaPlayer2.Play(new Media(_libVlc, rtspUrl2, FromType.FromLocation));
            _mediaPlayer3.Play(new Media(_libVlc, rtspUrl3, FromType.FromLocation));
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _mediaPlayer.Dispose();
            _mediaPlayer2.Dispose();
            _mediaPlayer3.Dispose();
            _libVlc.Dispose();
            base.OnFormClosing(e);
        }

        private async void OpenGate_Click(object sender, EventArgs e)
        {
            OpenGate.ForeColor = Color.SeaGreen;
            CloseGate_button.ForeColor = Color.Black;
#if !DEBUG
            await _barrierService.CloseOpenBarrier("1");
#endif
        }

        private async void CloseGate_button_Click(object sender, EventArgs e)
        {
            CloseGate_button.ForeColor = Color.IndianRed;
            OpenGate.ForeColor = Color.Black;
#if !DEBUG
            await _barrierService.CloseOpenBarrier("0");
#endif
        }

        private async void RecognizeCamFirst_Click(object sender, EventArgs e)
        {
            DisableAllButtons(false);
            var firstCamIp = ConfigurationManager.AppSettings["EnterANPRCam"];
            var recognizedPlate = _dahuaService.GetPlateNumber(firstCamIp);
            double weight = 1000;
#if !DEBUG
            weight = await _barrierService.StartWeightRequests() ?? 0;
#endif
            
            if (!string.IsNullOrEmpty(recognizedPlate.Error))
            {
                MessageBox.Show("Машина не распознан!");
            }
            else
            {
                MessageBox.Show("Машина распознана, чтобы продолжить выберите статус машины и сохраните!");
                currentWeight.Text = weight.ToString(CultureInfo.InvariantCulture);
                currentPlateNumber.Text = recognizedPlate.PlateNumber;
                saveRecognizedPlateButton.Visible = true;
                cancelRecognizedPlate.Visible = true;
                wichCameraIsPressedBox.Text = Constants.Constants.TruckEmptyStatus;
                showRecognizedCumBoxes(true);
            }
            
            DisableAllButtons(true);
        }

        private async void RecognizeCamSecond_Click(object sender, EventArgs e)
        {
            DisableAllButtons(false);
            var secondCamIp = ConfigurationManager.AppSettings["ExitANPRCam"];
            var recognizedPlate = _dahuaService.GetPlateNumber(secondCamIp);
            double weight = 1500;
#if !DEBUG
            weight = await _barrierService.StartWeightRequests() ?? 0;
#endif
            if (!string.IsNullOrEmpty(recognizedPlate.Error))
            {
                MessageBox.Show("Машина не распознан!");
                DisableAllButtons(true);
                return;
            }
            
            MessageBox.Show("Машина распознана, чтобы продолжить выберите статус машины и сохраните!");
            currentWeight.Text = weight.ToString(CultureInfo.InvariantCulture);
            currentPlateNumber.Text = recognizedPlate.PlateNumber;
            saveRecognizedPlateButton.Visible = true;
            cancelRecognizedPlate.Visible = true;
            wichCameraIsPressedBox.Text = Constants.Constants.TruckFullStatus;
            showRecognizedCumBoxes(true);
            DisableAllButtons(true);
        }

        private async void SaveAndPrint_Click(object sender, EventArgs e)
        {
            DisableAllButtons(false);
            if (typeCargoBox.SelectedIndex == 0)
            {
                MessageBox.Show("Выберите тип груза!");
                DisableAllButtons(true);
                return;
            }
            if (clientsBox.SelectedIndex == 0)
            {
                MessageBox.Show("Выберите контрагента!");
                DisableAllButtons(true);
                return;
            }
            if (string.IsNullOrEmpty(fullNameContragentBox.Text))
            {
                MessageBox.Show("Пропишите ФИО!");
                DisableAllButtons(true);
                return;
            }
            if (string.IsNullOrEmpty(cubMetrBox.Text))
            {
                MessageBox.Show("Пропишите Куб.м!");
                DisableAllButtons(true);
                return;
            }

            var selectedGood = (Item)typeCargoBox.SelectedItem;
            var selectedClient = (Item)clientsBox.SelectedItem;
            var fullNameAgent = fullNameContragentBox.Text;
            var cubMetr = cubMetrBox.Text;
            
            var lastFoundPlateNumber = currentPlateNumber.Text;
            
            var truck = _truckService.GetLatestTruckByPlateAndStatus(lastFoundPlateNumber, Constants.Constants.TruckFullStatus);
            //TODO:Need to wait changes from Cement factory
            // var result1C = await _service1C.SaveSaleAsync(new SaleRequest
            // {
            //     Date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
            //     Goods = new List<SaleItem>
            //     {
            //         new SaleItem
            //         {
            //             Count = 1,
            //             Guid = selectedGood.Guid,
            //             Weight = truck.weight_full
            //         }
            //     },
            //     ClientGuid = selectedClient.Guid,
            //     CarNumber = truck.plate_number
            // });
            //
            // truck.c1_status = result1C ? Constants.Constants.OneCOkStatus : Constants.Constants.OneCErrorStatus;
            truck.c1_status = Constants.Constants.OneCErrorStatus;
            truck.type_cargo = selectedGood.Name;
            truck.cub_metr = cubMetr;
            
            _truckService.UpdateTruck(truck);
            _printService.PrintTruckInfo(truck, selectedClient.Name, fullNameAgent, cubMetr);

            typeCargoBox.SelectedIndex = 0;
            clientsBox.SelectedIndex = 0;
            currentWeight.Text = string.Empty;
            currentPlateNumber.Text = string.Empty;

            VisibleNotVisibleButtons(false);
            DisableAllButtons(true);
        }

        private void DisableAllButtons(bool isDisable)
        {
            OpenGate.Enabled = isDisable;
            CloseGate_button.Enabled = isDisable;
            RecognizeCamFirst.Enabled = isDisable;
            RecognizeCamSecond.Enabled = isDisable;
            showHistoryButton.Enabled = isDisable;
            saveAndPrintButton.Enabled = isDisable;
            this.Enabled = isDisable;
        }

        private void VisibleNotVisibleButtons(bool isVisible)
        {
            label4.Visible = isVisible;
            typeCargoBox.Visible = isVisible;
            label5.Visible = isVisible;
            clientsBox.Visible = isVisible;
            saveAndPrintButton.Visible = isVisible;
            fullNameContragentBox.Visible = isVisible;
            label13.Visible = isVisible;
            cubMetrBox.Visible = isVisible;
            label9.Visible = isVisible;
            
            typeCargoBox.SelectedIndex = 0;
            clientsBox.SelectedIndex = 0;
            fullNameContragentBox.Text = string.Empty;
            cubMetrBox.Text = string.Empty;
        }
        
        // Asynchronous method to call the status API and update the UI
        private async Task CheckBarrierStatusAsync()
        {
            var ip = ConfigurationManager.AppSettings["BarrierServer"];
            
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync($"http://{ip}/status.cgi");
                    var status = response.Trim().Equals("TRUE", StringComparison.OrdinalIgnoreCase);

                    // Update the UI on the main thread
                    Invoke((Action)(() => UpdateBarrierStatus(status)));
                }
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
            }
        }

        private async Task CheckWeightStatusAsync()
        {
            var ip = ConfigurationManager.AppSettings["BarrierServer"];
            
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync($"http://{ip}/weight.cgi");
                    Invoke((Action)(() => UpdateWeightStatus(response)));
                }
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
            }
        }
        
        private void UpdateWeightStatus(string wight)
        {
            label15.Text = wight;
        }

        // Method to update the status and color in the UI
        private void UpdateBarrierStatus(bool isOpen)
        {
            if (isOpen)
            {
                label14.Text = "ОТКРЫТ";
                label14.ForeColor = Color.SeaGreen;
                OpenGate.ForeColor = Color.SeaGreen;
                CloseGate_button.ForeColor = Color.Black;
            }
            else
            {
                label14.Text = "ЗАКРЫТ";
                label14.ForeColor = Color.IndianRed;
                CloseGate_button.ForeColor = Color.IndianRed;
                OpenGate.ForeColor = Color.Black;
            }
        }

        private async void saveRecognizedPlateButton_Click(object sender, EventArgs e)
        {
            DisableAllButtons(false);
            if (currrentStatusBox.SelectedIndex == 0)
            {
                MessageBox.Show("Выберите статус машины!");
                DisableAllButtons(true);
                return;
            }
            
            var selectedStatus = (Item)currrentStatusBox.SelectedItem;
            var weight = double.Parse(currentWeight.Text);
            var plateNumber = currentPlateNumber.Text;
            var whichCamera = wichCameraIsPressedBox.Text;
            var isSuccess = true;

            switch (selectedStatus.Name)
            {
                case Constants.Constants.TruckEmptyStatus:
                {
                    var truck = new Truck
                    {
                        plate_number = plateNumber,
                        truck_status = selectedStatus.Name,
                        weight_empty = weight,
                        Date = DateTime.Now
                    };
                    
                    _truckService.AddTruck(truck);
                    break;
                }
                case Constants.Constants.TruckFullStatus:
                {
                    var truck = _truckService.GetLatestTruckByPlateAndStatus(plateNumber, Constants.Constants.TruckEmptyStatus);

                    if (truck is null)
                    {
                        MessageBox.Show($"Машина {plateNumber} в базе со статусом \"Пустой\" не найден!");
                        isSuccess = false;
                        break;
                    }

#if !DEBUG
                    var path = await _dahuaService.SavePicture();
                    truck.image_path = path;
#endif
                
                    truck.truck_status = selectedStatus.Name;
                    truck.weight_full = weight;
                    truck.weight_difference = truck.weight_full - truck.weight_empty;
                    truck.Date = DateTime.Now;
                
                    _truckService.UpdateTruck(truck);
                    VisibleNotVisibleButtons(true);
                    break;
                }
                case Constants.Constants.TruckReceiveStatus:
                {
                    if (whichCamera == Constants.Constants.TruckEmptyStatus)
                    {
                        var truck = new Truck
                        {
                            plate_number = plateNumber,
                            truck_status = selectedStatus.Name,
                            weight_full = weight,
                            Date = DateTime.Now
                        };
                    
                        _truckService.AddTruck(truck);
                    }
                    else if (whichCamera == Constants.Constants.TruckFullStatus)
                    {
                        var truck = _truckService.GetLatestTruckByPlateAndStatus(plateNumber, Constants.Constants.TruckReceiveStatus);

                        if (truck is null)
                        {
                            MessageBox.Show($"Машина {plateNumber} в базе со статусом \"Приход\" не найден!");
                            isSuccess = false;
                            break;
                        }
                
                        truck.truck_status = selectedStatus.Name;
                        truck.weight_empty = weight;
                        truck.weight_difference = truck.weight_full - truck.weight_empty;
                        truck.Date = DateTime.Now;
                
                        _truckService.UpdateTruck(truck);
                    }
                    
                    break;
                }
            }
            
            currrentStatusBox.SelectedIndex = 0;
            currentWeight.Text = string.Empty;
            currentPlateNumber.Text = string.Empty;
            wichCameraIsPressedBox.Text = string.Empty;
            saveRecognizedPlateButton.Visible = false;
            cancelRecognizedPlate.Visible = false;
            DisableAllButtons(true);
            showRecognizedCumBoxes(false);
            
            if (isSuccess)
            {
                MessageBox.Show("Данные сохранены в базе!");
            }
        }

        private void cancelRecognizedPlate_Click(object sender, EventArgs e)
        {
            currrentStatusBox.SelectedIndex = 0;
            currentWeight.Text = string.Empty;
            currentPlateNumber.Text = string.Empty;
            saveRecognizedPlateButton.Visible = false;
            cancelRecognizedPlate.Visible = false;
            wichCameraIsPressedBox.Text = string.Empty;
            showRecognizedCumBoxes(false);
            MessageBox.Show("Отменен!");
        }

        private void showRecognizedCumBoxes(bool isShow)
        {
            label1.Visible = isShow;
            currentWeight.Visible = isShow;
            currrentStatusBox.Visible = isShow;
            label3.Visible = isShow;
            currentPlateNumber.Visible = isShow;
            label2.Visible = isShow;
        }
    }
}