using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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

        private string CurrentStatus;
        private string WichCameraPressed;

        public MainForm()
        {
            InitializeComponent();
            Core.Initialize();

            // Hide when init
            VisibleNotVisibleButtons(false);
            showRecognizedCumBoxes(false);
            saveRecognizedPlateButton.Visible = false;
            cancelRecognizedPlate.Visible = false;
            typeCargoBox.SelectedIndex = -1;
            clientsBox.SelectedIndex = -1;
            currentPlateNumber.Visible = false;
            currentNumbersBox.Visible = false;

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
            _statusTimer.Elapsed += async (sender, e) => await CheckRadarsStatusAsync();
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
            rtspUrl2 = "http://67.53.46.161:65123/mjpg/video.mjpg";
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

            // Start both tasks simultaneously
            var recognizePlateTask = Task.Run(() => _dahuaService.GetPlateNumber(firstCamIp));
            var weightTask = Task.FromResult<double?>(1500);

#if !DEBUG
            weightTask = _barrierService.StartWeightRequests();
#endif

            // Wait for both tasks to complete
            await Task.WhenAll(recognizePlateTask, weightTask);

            // Get the results
            var recognizedPlate = await recognizePlateTask;
            var weight = weightTask.Result ?? 0; // If the weight is null, assign 0

            if (!string.IsNullOrEmpty(recognizedPlate.Error))
            {
                MessageBox.Show("Машина не распознан!");
            }
            else
            {
                MessageBox.Show("Машина распознана, чтобы продолжить сохраните данные!");
                currentWeight.Text = weight.ToString(CultureInfo.InvariantCulture);
                currentPlateNumber.Text = recognizedPlate.PlateNumber;

                WichCameraPressed = Constants.Constants.TruckEmptyStatus;
                CurrentStatus = enterCheckBox.Checked
                    ? Constants.Constants.TruckReceiveFullStatus
                    : Constants.Constants.TruckEmptyStatus;

                showRecognizedCumBoxes(true);
                currentPlateNumber.Visible = true;
            }

            DisableAllButtons(true);
        }

        private async void RecognizeCamSecond_Click(object sender, EventArgs e)
        {
            DisableAllButtons(false);
            var secondCamIp = ConfigurationManager.AppSettings["ExitANPRCam"];

            // Start both tasks concurrently
            var recognizePlateTask = Task.Run(() => _dahuaService.GetPlateNumber(secondCamIp));
            var weightTask = Task.FromResult<double?>(1500);

#if !DEBUG
            weightTask = _barrierService.StartWeightRequests();
#endif

            // Wait for both tasks to complete
            await Task.WhenAll(recognizePlateTask, weightTask);

            // Get the results
            var recognizedPlate = await recognizePlateTask;
            var weight = weightTask.Result ?? 0; // Assign 0 if weight is null

            if (!string.IsNullOrEmpty(recognizedPlate.Error))
            {
                MessageBox.Show("Машина не распознан!");
            }
            else
            {
                MessageBox.Show("Машина распознана, чтобы продолжить сохраните данные!");
                currentWeight.Text = weight.ToString(CultureInfo.InvariantCulture);
                currentPlateNumber.Text = recognizedPlate.PlateNumber;
                
                CurrentStatus = enterCheckBox.Checked
                    ? Constants.Constants.TruckReceiveFullStatus
                    : Constants.Constants.TruckFullStatus;

                var searchStatus = enterCheckBox.Checked
                    ? Constants.Constants.TruckReceiveFullStatus
                    : Constants.Constants.TruckEmptyStatus;

                var trucks = _truckService.GetLatestTruckByPlateAndStatus(recognizedPlate.PlateNumber, searchStatus);
                
                if (!trucks.Any())
                {
                    MessageBox.Show($"Машина {recognizedPlate.PlateNumber} в базе со статусом \"{searchStatus}\" не найден!");
                    DisableAllButtons(true);
                    return;
                }
                
                currentNumbersBox.DataSource = trucks;
                currentNumbersBox.DisplayMember = "plate_number";
                currentNumbersBox.ValueMember = "Id";

                WichCameraPressed = Constants.Constants.TruckFullStatus;
                

                showRecognizedCumBoxes(true);
                currentNumbersBox.Visible = true;
            }

            DisableAllButtons(true);
        }

        private async void SaveAndPrint_Click(object sender, EventArgs e)
        {
            DisableAllButtons(false);
            if (typeCargoBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите тип груза!");
                DisableAllButtons(true);
                return;
            }

            if (clientsBox.SelectedIndex == -1)
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

            if (string.IsNullOrEmpty(quantityBox.Text))
            {
                MessageBox.Show("Пропишите кол-во!");
                DisableAllButtons(true);
                return;
            }

            var selectedGood = (Item)typeCargoBox.SelectedItem;
            var selectedClient = (Item)clientsBox.SelectedItem;
            var fullNameAgent = fullNameContragentBox.Text;
            var cubMetr = cubMetrBox.Text;
            var quantity = quantityBox.Text;

            var selectedPlateNumber = (Truck)currentNumbersBox.SelectedItem;
            var truck = _truckService.GetTruckById(selectedPlateNumber.Id);
            
            // var result1C = await _service1C.GoodsMoving(new SaleRequest
            // {
            //     Date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
            //     Goods = new List<SaleItem>
            //     {
            //         new SaleItem
            //         {
            //             Count = 1,
            //             Guid = selectedGood.Guid,
            //             Weight = double.Parse(cubMetr)
            //         }
            //     },
            //     WarehouseRecipient = selectedClient.Guid,
            //     CarNumber = truck.plate_number
            // });

            truck.c1_status = Constants.Constants.OneCErrorStatus;// result1C ? Constants.Constants.OneCErrorStatus : Constants.Constants.OneCOkStatus;
            truck.type_cargo = selectedGood.Name;
            truck.cub_metr = cubMetr;

            _truckService.UpdateTruck(truck);
            _printService.PrintTruckInfo(truck, selectedClient.Name, fullNameAgent, cubMetr, quantity);

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
            label11.Visible = isVisible;
            quantityBox.Visible = isVisible;

            typeCargoBox.SelectedIndex = -1;
            clientsBox.SelectedIndex = -1;
            fullNameContragentBox.Text = string.Empty;
            cubMetrBox.Text = string.Empty;
            quantityBox.Text = string.Empty;
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
        
        
        private async Task CheckRadarsStatusAsync()
        {
            var ip = ConfigurationManager.AppSettings["BarrierServer"];

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var responseRadar1 = await httpClient.GetStringAsync($"http://{ip}/status_radar_1.cgi");
                    var responseRadar2 = await httpClient.GetStringAsync($"http://{ip}/status_radar_2.cgi");
                    var radar1status = responseRadar1.Trim().Equals("FALSE", StringComparison.OrdinalIgnoreCase);
                    var radar2status = responseRadar2.Trim().Equals("FALSE", StringComparison.OrdinalIgnoreCase);

                    // Update the UI on the main thread
                    Invoke((Action)(() => UpdateRadarsStatus(radar1status, radar2status)));
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
        
        private void UpdateRadarsStatus(bool firstRadar, bool secondRadar)
        {
            SetEmbeddedImageToPictureBox(firstRadarPictureBox, firstRadar ? "switch-icon-on-button.png" : "switch-icon-off-button.png");
            SetEmbeddedImageToPictureBox(secondRadarPictureBox, secondRadar ? "switch-icon-on-button.png" : "switch-icon-off-button.png");
        }

        private async void saveRecognizedPlateButton_Click(object sender, EventArgs e)
        {
            DisableAllButtons(false);

            if (enterCheckBox.Checked && WichCameraPressed == Constants.Constants.TruckEmptyStatus &&
                string.IsNullOrEmpty(enterTextBoxCargo.Text))
            {
                MessageBox.Show("Пожалуйста заполните поле, тип груза!");
                DisableAllButtons(true);
                return;
            }
            if (!enterCheckBox.Checked 
                && WichCameraPressed == Constants.Constants.TruckFullStatus 
                && currentNumbersBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите номер машины из предложенного списка!");
                DisableAllButtons(true);
                return;
            }

            var weight = double.Parse(currentWeight.Text);
            var plateNumber = currentPlateNumber.Text;
            var whichCamera = WichCameraPressed;
            var isSuccess = true;

            switch (CurrentStatus)
            {
                case Constants.Constants.TruckEmptyStatus:
                {
                    var truck = new Truck
                    {
                        plate_number = plateNumber,
                        truck_status = CurrentStatus,
                        weight_empty = weight,
                        Date = DateTime.Now
                    };

                    _truckService.AddTruck(truck);
                    break;
                }
                case Constants.Constants.TruckFullStatus:
                {
                    var goods = await _service1C.GetProductsAsync();
                    var clients = await _service1C.GetClientsWarehouses();

                    typeCargoBox.DataSource = goods;
                    typeCargoBox.DisplayMember = "Name";
                    typeCargoBox.ValueMember = "Guid";

                    clientsBox.DataSource = clients;
                    clientsBox.DisplayMember = "Name";
                    clientsBox.ValueMember = "Guid";
                    
                    var selectedPlateNumber = (Truck)currentNumbersBox.SelectedItem;
                    var truck = _truckService.GetTruckById(selectedPlateNumber.Id);

#if !DEBUG
                    var path = await _dahuaService.SavePicture();
                    truck.image_path = path;
#endif

                    truck.truck_status = CurrentStatus;
                    truck.weight_full = weight;
                    truck.weight_difference = truck.weight_full - truck.weight_empty;
                    truck.Date = DateTime.Now;

                    _truckService.UpdateTruck(truck);
                    VisibleNotVisibleButtons(true);
                    break;
                }
                case Constants.Constants.TruckReceiveFullStatus:
                {
                    if (whichCamera == Constants.Constants.TruckEmptyStatus)
                    {
                        var truck = new Truck
                        {
                            plate_number = plateNumber,
                            truck_status = CurrentStatus,
                            weight_full = weight,
                            type_cargo = enterTextBoxCargo.Text,
                            Date = DateTime.Now
                        };

#if !DEBUG
                        var path = await _dahuaService.SavePicture();
                        truck.image_path = path;
#endif

                        _truckService.AddTruck(truck);
                    }
                    else if (whichCamera == Constants.Constants.TruckFullStatus)
                    {
                        var selectedPlateNumber = (Truck)currentNumbersBox.SelectedItem;
                        var truck = _truckService.GetTruckById(selectedPlateNumber.Id);

                        truck.truck_status = Constants.Constants.TruckReceiveEmptyStatus;
                        truck.weight_empty = weight;
                        truck.weight_difference = truck.weight_full - truck.weight_empty;
                        truck.Date = DateTime.Now;

                        _truckService.UpdateTruck(truck);
                    }

                    break;
                }
            }

            currentWeight.Text = string.Empty;
            currentPlateNumber.Text = string.Empty;
            WichCameraPressed = string.Empty;
            enterTextBoxCargo.Text = string.Empty;
            saveRecognizedPlateButton.Visible = false;
            cancelRecognizedPlate.Visible = false;
            DisableAllButtons(true);
            showRecognizedCumBoxes(false);
            currentPlateNumber.Visible = false;
            currentNumbersBox.Visible = false;

            if (isSuccess)
            {
                MessageBox.Show("Данные сохранены в базе!");
            }
        }

        private void cancelRecognizedPlate_Click(object sender, EventArgs e)
        {
            currentWeight.Text = string.Empty;
            currentPlateNumber.Text = string.Empty;
            currentNumbersBox.DataSource = null;
            saveRecognizedPlateButton.Visible = false;
            cancelRecognizedPlate.Visible = false;
            WichCameraPressed = string.Empty;
            showRecognizedCumBoxes(false);
            currentPlateNumber.Visible = false;
            currentNumbersBox.Visible = false;
            MessageBox.Show("Отменен!");
        }

        private void showRecognizedCumBoxes(bool isShow)
        {
            label1.Visible = isShow;
            currentWeight.Visible = isShow;
            label2.Visible = isShow;
            saveRecognizedPlateButton.Visible = isShow;
            cancelRecognizedPlate.Visible = isShow;

            if (enterCheckBox.Checked && WichCameraPressed == Constants.Constants.TruckEmptyStatus)
            {
                label3.Visible = isShow;
                enterTextBoxCargo.Visible = isShow;
            }
            else
            {
                label3.Visible = false;
                enterTextBoxCargo.Visible = false;
            }
        }

        private void enterCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (enterCheckBox.Checked)
            {
                RecognizeCamFirst.Text = "Распознать загруженную машину";
                RecognizeCamSecond.Text = "Распознать пустую машину";
            }
            else
            {
                RecognizeCamFirst.Text = "Распознать пустую машину";
                RecognizeCamSecond.Text = "Распознать загруженную машину";
            }
        }
        
        private void SetEmbeddedImageToPictureBox(PictureBox pictureBox, string imageName)
        {
            // Ensure the image name includes the correct namespace and filename
            var resourceName = $"CementFactory.{imageName}";

            // Get the executing assembly
            var assembly = Assembly.GetExecutingAssembly();

            // Load the image stream
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    pictureBox.Image = Image.FromStream(stream);
                }
                else
                {
                    MessageBox.Show($"Image '{imageName}' not found as an embedded resource.");
                    Log.Error($"Image '{imageName}' not found as an embedded resource.");
                }
            }
        }
    }
}