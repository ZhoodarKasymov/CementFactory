namespace CementFactory
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ButtonGroup = new System.Windows.Forms.Label();
            this.OpenGate = new System.Windows.Forms.Button();
            this.CloseGate_button = new System.Windows.Forms.Button();
            this.currentWeight = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.currentPlateNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.showHistoryButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.videoView1 = new LibVLCSharp.WinForms.VideoView();
            this.videoView2 = new LibVLCSharp.WinForms.VideoView();
            this.RecognizeCamFirst = new System.Windows.Forms.Button();
            this.RecognizeCamSecond = new System.Windows.Forms.Button();
            this.saveAndPrintButton = new System.Windows.Forms.Button();
            this.typeCargoBox = new System.Windows.Forms.ComboBox();
            this.clientsBox = new System.Windows.Forms.ComboBox();
            this.videoView3 = new LibVLCSharp.WinForms.VideoView();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.fullNameContragentBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.currrentStatusBox = new System.Windows.Forms.ComboBox();
            this.saveRecognizedPlateButton = new System.Windows.Forms.Button();
            this.cancelRecognizedPlate = new System.Windows.Forms.Button();
            this.cubMetrBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.wichCameraIsPressedBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.videoView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoView3)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonGroup
            // 
            this.ButtonGroup.AutoSize = true;
            this.ButtonGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonGroup.Location = new System.Drawing.Point(10, 9);
            this.ButtonGroup.Name = "ButtonGroup";
            this.ButtonGroup.Size = new System.Drawing.Size(184, 16);
            this.ButtonGroup.TabIndex = 0;
            this.ButtonGroup.Text = "Кнопки для управления";
            this.ButtonGroup.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // OpenGate
            // 
            this.OpenGate.BackColor = System.Drawing.SystemColors.Control;
            this.OpenGate.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.OpenGate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.OpenGate.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenGate.ForeColor = System.Drawing.Color.Black;
            this.OpenGate.Location = new System.Drawing.Point(11, 31);
            this.OpenGate.Name = "OpenGate";
            this.OpenGate.Size = new System.Drawing.Size(120, 22);
            this.OpenGate.TabIndex = 1;
            this.OpenGate.Text = "Открыть шлагбаум";
            this.OpenGate.UseVisualStyleBackColor = false;
            this.OpenGate.Click += new System.EventHandler(this.OpenGate_Click);
            // 
            // CloseGate_button
            // 
            this.CloseGate_button.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.CloseGate_button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.CloseGate_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseGate_button.ForeColor = System.Drawing.Color.Black;
            this.CloseGate_button.Location = new System.Drawing.Point(137, 31);
            this.CloseGate_button.Name = "CloseGate_button";
            this.CloseGate_button.Size = new System.Drawing.Size(120, 22);
            this.CloseGate_button.TabIndex = 2;
            this.CloseGate_button.Text = "Закрыть шлагбаум";
            this.CloseGate_button.UseVisualStyleBackColor = true;
            this.CloseGate_button.Click += new System.EventHandler(this.CloseGate_button_Click);
            // 
            // currentWeight
            // 
            this.currentWeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.currentWeight.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.currentWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentWeight.Location = new System.Drawing.Point(12, 414);
            this.currentWeight.Name = "currentWeight";
            this.currentWeight.ReadOnly = true;
            this.currentWeight.Size = new System.Drawing.Size(130, 21);
            this.currentWeight.TabIndex = 3;
            this.currentWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 396);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Стабильный вес:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(284, 396);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Номер машины:";
            // 
            // currentPlateNumber
            // 
            this.currentPlateNumber.Location = new System.Drawing.Point(284, 414);
            this.currentPlateNumber.Name = "currentPlateNumber";
            this.currentPlateNumber.ReadOnly = true;
            this.currentPlateNumber.Size = new System.Drawing.Size(130, 20);
            this.currentPlateNumber.TabIndex = 6;
            this.currentPlateNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(148, 396);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Статус машины:";
            // 
            // showHistoryButton
            // 
            this.showHistoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showHistoryButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.showHistoryButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.showHistoryButton.Location = new System.Drawing.Point(926, 432);
            this.showHistoryButton.Name = "showHistoryButton";
            this.showHistoryButton.Size = new System.Drawing.Size(97, 20);
            this.showHistoryButton.TabIndex = 9;
            this.showHistoryButton.Text = "История машин";
            this.showHistoryButton.UseVisualStyleBackColor = true;
            this.showHistoryButton.Click += new System.EventHandler(this.history_trucks_button);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 474);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = "Тип груза:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(213, 473);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 15);
            this.label5.TabIndex = 13;
            this.label5.Text = "Контрагент:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(455, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(186, 16);
            this.label6.TabIndex = 17;
            this.label6.Text = "Камера №1 возле входа";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(746, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(196, 16);
            this.label7.TabIndex = 18;
            this.label7.Text = "Камера №2 возле выхода";
            // 
            // videoView1
            // 
            this.videoView1.BackColor = System.Drawing.Color.Black;
            this.videoView1.Location = new System.Drawing.Point(455, 80);
            this.videoView1.MediaPlayer = null;
            this.videoView1.Name = "videoView1";
            this.videoView1.Size = new System.Drawing.Size(285, 275);
            this.videoView1.TabIndex = 19;
            this.videoView1.Text = "videoView1";
            // 
            // videoView2
            // 
            this.videoView2.BackColor = System.Drawing.Color.Black;
            this.videoView2.Location = new System.Drawing.Point(746, 80);
            this.videoView2.MediaPlayer = null;
            this.videoView2.Name = "videoView2";
            this.videoView2.Size = new System.Drawing.Size(285, 275);
            this.videoView2.TabIndex = 20;
            this.videoView2.Text = "videoView2";
            // 
            // RecognizeCamFirst
            // 
            this.RecognizeCamFirst.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.RecognizeCamFirst.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.RecognizeCamFirst.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecognizeCamFirst.Location = new System.Drawing.Point(455, 361);
            this.RecognizeCamFirst.Name = "RecognizeCamFirst";
            this.RecognizeCamFirst.Size = new System.Drawing.Size(166, 23);
            this.RecognizeCamFirst.TabIndex = 22;
            this.RecognizeCamFirst.Text = "Распознать пустую машину\r\n\r\n";
            this.RecognizeCamFirst.UseVisualStyleBackColor = true;
            this.RecognizeCamFirst.Click += new System.EventHandler(this.RecognizeCamFirst_Click);
            // 
            // RecognizeCamSecond
            // 
            this.RecognizeCamSecond.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.RecognizeCamSecond.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.RecognizeCamSecond.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecognizeCamSecond.Location = new System.Drawing.Point(748, 361);
            this.RecognizeCamSecond.Name = "RecognizeCamSecond";
            this.RecognizeCamSecond.Size = new System.Drawing.Size(195, 23);
            this.RecognizeCamSecond.TabIndex = 23;
            this.RecognizeCamSecond.Text = "Распознать загруженную машину\r\n";
            this.RecognizeCamSecond.UseVisualStyleBackColor = true;
            this.RecognizeCamSecond.Click += new System.EventHandler(this.RecognizeCamSecond_Click);
            // 
            // saveAndPrintButton
            // 
            this.saveAndPrintButton.AutoSize = true;
            this.saveAndPrintButton.BackColor = System.Drawing.Color.DodgerBlue;
            this.saveAndPrintButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.saveAndPrintButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.saveAndPrintButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveAndPrintButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.saveAndPrintButton.Location = new System.Drawing.Point(12, 560);
            this.saveAndPrintButton.Name = "saveAndPrintButton";
            this.saveAndPrintButton.Size = new System.Drawing.Size(239, 23);
            this.saveAndPrintButton.TabIndex = 24;
            this.saveAndPrintButton.Text = "Сохранить в 1С и распечатать накладную";
            this.saveAndPrintButton.UseVisualStyleBackColor = false;
            this.saveAndPrintButton.Click += new System.EventHandler(this.SaveAndPrint_Click);
            // 
            // typeCargoBox
            // 
            this.typeCargoBox.FormattingEnabled = true;
            this.typeCargoBox.Location = new System.Drawing.Point(11, 492);
            this.typeCargoBox.Name = "typeCargoBox";
            this.typeCargoBox.Size = new System.Drawing.Size(198, 21);
            this.typeCargoBox.TabIndex = 25;
            // 
            // clientsBox
            // 
            this.clientsBox.FormattingEnabled = true;
            this.clientsBox.Location = new System.Drawing.Point(216, 491);
            this.clientsBox.Name = "clientsBox";
            this.clientsBox.Size = new System.Drawing.Size(198, 21);
            this.clientsBox.TabIndex = 26;
            // 
            // videoView3
            // 
            this.videoView3.BackColor = System.Drawing.Color.Black;
            this.videoView3.Location = new System.Drawing.Point(12, 80);
            this.videoView3.MediaPlayer = null;
            this.videoView3.Name = "videoView3";
            this.videoView3.Size = new System.Drawing.Size(437, 304);
            this.videoView3.TabIndex = 27;
            this.videoView3.Text = "videoView3";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(168, 16);
            this.label8.TabIndex = 28;
            this.label8.Text = "Камера №3 Для груза";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(380, 33);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 15);
            this.label10.TabIndex = 42;
            this.label10.Text = "Текущий вес:";
            // 
            // fullNameContragentBox
            // 
            this.fullNameContragentBox.Location = new System.Drawing.Point(12, 534);
            this.fullNameContragentBox.Name = "fullNameContragentBox";
            this.fullNameContragentBox.Size = new System.Drawing.Size(197, 20);
            this.fullNameContragentBox.TabIndex = 55;
            this.fullNameContragentBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(10, 516);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(111, 15);
            this.label13.TabIndex = 54;
            this.label13.Text = "ФИО водителя:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(263, 34);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 15);
            this.label12.TabIndex = 57;
            this.label12.Text = "Статус:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(316, 34);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(0, 15);
            this.label14.TabIndex = 58;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.LimeGreen;
            this.label15.Location = new System.Drawing.Point(469, 33);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(15, 15);
            this.label15.TabIndex = 60;
            this.label15.Text = "0";
            // 
            // currrentStatusBox
            // 
            this.currrentStatusBox.FormattingEnabled = true;
            this.currrentStatusBox.Location = new System.Drawing.Point(148, 414);
            this.currrentStatusBox.Name = "currrentStatusBox";
            this.currrentStatusBox.Size = new System.Drawing.Size(130, 21);
            this.currrentStatusBox.TabIndex = 61;
            // 
            // saveRecognizedPlateButton
            // 
            this.saveRecognizedPlateButton.BackColor = System.Drawing.Color.DodgerBlue;
            this.saveRecognizedPlateButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.saveRecognizedPlateButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.saveRecognizedPlateButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveRecognizedPlateButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.saveRecognizedPlateButton.Location = new System.Drawing.Point(228, 441);
            this.saveRecognizedPlateButton.Name = "saveRecognizedPlateButton";
            this.saveRecognizedPlateButton.Size = new System.Drawing.Size(90, 22);
            this.saveRecognizedPlateButton.TabIndex = 62;
            this.saveRecognizedPlateButton.Text = "Сохранить";
            this.saveRecognizedPlateButton.UseVisualStyleBackColor = false;
            this.saveRecognizedPlateButton.Click += new System.EventHandler(this.saveRecognizedPlateButton_Click);
            // 
            // cancelRecognizedPlate
            // 
            this.cancelRecognizedPlate.BackColor = System.Drawing.SystemColors.Control;
            this.cancelRecognizedPlate.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.cancelRecognizedPlate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cancelRecognizedPlate.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelRecognizedPlate.ForeColor = System.Drawing.Color.Black;
            this.cancelRecognizedPlate.Location = new System.Drawing.Point(324, 441);
            this.cancelRecognizedPlate.Name = "cancelRecognizedPlate";
            this.cancelRecognizedPlate.Size = new System.Drawing.Size(90, 22);
            this.cancelRecognizedPlate.TabIndex = 63;
            this.cancelRecognizedPlate.Text = "Отменить";
            this.cancelRecognizedPlate.UseVisualStyleBackColor = false;
            this.cancelRecognizedPlate.Click += new System.EventHandler(this.cancelRecognizedPlate_Click);
            // 
            // cubMetrBox
            // 
            this.cubMetrBox.Location = new System.Drawing.Point(215, 534);
            this.cubMetrBox.Name = "cubMetrBox";
            this.cubMetrBox.Size = new System.Drawing.Size(197, 20);
            this.cubMetrBox.TabIndex = 65;
            this.cubMetrBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.cubMetrBox.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(213, 516);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 15);
            this.label9.TabIndex = 64;
            this.label9.Text = "Куб.м:";
            // 
            // wichCameraIsPressedBox
            // 
            this.wichCameraIsPressedBox.Enabled = false;
            this.wichCameraIsPressedBox.Location = new System.Drawing.Point(893, 555);
            this.wichCameraIsPressedBox.Name = "wichCameraIsPressedBox";
            this.wichCameraIsPressedBox.ReadOnly = true;
            this.wichCameraIsPressedBox.Size = new System.Drawing.Size(130, 20);
            this.wichCameraIsPressedBox.TabIndex = 66;
            this.wichCameraIsPressedBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.wichCameraIsPressedBox.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1035, 587);
            this.Controls.Add(this.wichCameraIsPressedBox);
            this.Controls.Add(this.cubMetrBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cancelRecognizedPlate);
            this.Controls.Add(this.saveRecognizedPlateButton);
            this.Controls.Add(this.currrentStatusBox);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.fullNameContragentBox);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.videoView3);
            this.Controls.Add(this.clientsBox);
            this.Controls.Add(this.typeCargoBox);
            this.Controls.Add(this.saveAndPrintButton);
            this.Controls.Add(this.RecognizeCamSecond);
            this.Controls.Add(this.RecognizeCamFirst);
            this.Controls.Add(this.videoView2);
            this.Controls.Add(this.videoView1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.showHistoryButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.currentPlateNumber);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.currentWeight);
            this.Controls.Add(this.CloseGate_button);
            this.Controls.Add(this.OpenGate);
            this.Controls.Add(this.ButtonGroup);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Цементный завод";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.videoView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoView3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox wichCameraIsPressedBox;

        private System.Windows.Forms.TextBox cubMetrBox;
        private System.Windows.Forms.Label label9;

        private System.Windows.Forms.ComboBox currrentStatusBox;
        private System.Windows.Forms.Button saveRecognizedPlateButton;
        private System.Windows.Forms.Button cancelRecognizedPlate;

        private System.Windows.Forms.Label label15;

        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;

        private System.Windows.Forms.TextBox fullNameContragentBox;
        private System.Windows.Forms.Label label13;

        private System.Windows.Forms.Label label10;

        private LibVLCSharp.WinForms.VideoView videoView3;
        private System.Windows.Forms.Label label8;

        private System.Windows.Forms.ComboBox typeCargoBox;
        private System.Windows.Forms.ComboBox clientsBox;

        private System.Windows.Forms.Button saveAndPrintButton;

        private System.Windows.Forms.Button RecognizeCamSecond;
        private System.Windows.Forms.Button RecognizeCamFirst;

        private LibVLCSharp.WinForms.VideoView videoView1;
        private LibVLCSharp.WinForms.VideoView videoView2;

        private System.Windows.Forms.Label label7;

        private System.Windows.Forms.Label label6;

        private System.Windows.Forms.Label label5;

        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.Button showHistoryButton;

        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.TextBox currentPlateNumber;

        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.TextBox currentWeight;

        private System.Windows.Forms.Button CloseGate_button;

        private System.Windows.Forms.Button OpenGate;

        private System.Windows.Forms.Label ButtonGroup;

        #endregion
    }
}