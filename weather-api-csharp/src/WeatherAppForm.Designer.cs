namespace WeatherApp
{
    partial class WeatherAppForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Label cityLabel;
        private TextBox cityTextBox;
        private Button getWeatherButton;
        private Button getForecastButton;
        private Button saveWeatherButton;
        private TextBox weatherInfoTextBox;

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            {
                this.cityLabel = new Label();
                this.cityTextBox = new TextBox();
                this.getWeatherButton = new Button();
                this.getForecastButton = new Button();
                this.saveWeatherButton = new Button();
                this.weatherInfoTextBox = new TextBox();
                this.SuspendLayout();

                // cityLabel
                this.cityLabel.AutoSize = true;
                this.cityLabel.Location = new System.Drawing.Point(12, 15);
                this.cityLabel.Name = "cityLabel";
                this.cityLabel.Size = new System.Drawing.Size(89, 15);
                this.cityLabel.Text = "Enter city name:";

                // cityTextBox
                this.cityTextBox.Location = new System.Drawing.Point(107, 12);
                this.cityTextBox.Name = "cityTextBox";
                this.cityTextBox.Size = new System.Drawing.Size(200, 23);

                // getWeatherButton
                this.getWeatherButton.Location = new System.Drawing.Point(12, 45);
                this.getWeatherButton.Name = "getWeatherButton";
                this.getWeatherButton.Size = new System.Drawing.Size(90, 30);
                this.getWeatherButton.Text = "Get Weather";
                this.getWeatherButton.Click += new System.EventHandler(this.getWeatherButton_Click);

                // getForecastButton
                this.getForecastButton.Location = new System.Drawing.Point(108, 45);
                this.getForecastButton.Name = "getForecastButton";
                this.getForecastButton.Size = new System.Drawing.Size(90, 30);
                this.getForecastButton.Text = "Get Forecast";
                this.getForecastButton.Click += new System.EventHandler(this.getForecastButton_Click);

                // saveWeatherButton
                this.saveWeatherButton.Location = new System.Drawing.Point(204, 45);
                this.saveWeatherButton.Name = "saveWeatherButton";
                this.saveWeatherButton.Size = new System.Drawing.Size(103, 30);
                this.saveWeatherButton.Text = "Save to File";
                this.saveWeatherButton.Click += new System.EventHandler(this.saveWeatherButton_Click);

                // weatherInfoTextBox
                this.weatherInfoTextBox.Location = new System.Drawing.Point(12, 85);
                this.weatherInfoTextBox.Multiline = true;
                this.weatherInfoTextBox.Name = "weatherInfoTextBox";
                this.weatherInfoTextBox.ReadOnly = true;
                this.weatherInfoTextBox.ScrollBars = ScrollBars.Vertical;
                this.weatherInfoTextBox.Size = new System.Drawing.Size(295, 200);

                // WeatherAppForm
                this.ClientSize = new System.Drawing.Size(319, 300);
                this.Controls.Add(this.cityLabel);
                this.Controls.Add(this.cityTextBox);
                this.Controls.Add(this.getWeatherButton);
                this.Controls.Add(this.getForecastButton);
                this.Controls.Add(this.saveWeatherButton);
                this.Controls.Add(this.weatherInfoTextBox);
                this.Name = "WeatherAppForm";
                this.Text = "Weather App";
                this.ResumeLayout(false);
                this.PerformLayout();
            }
        }

        #endregion
    }
}
