using Newtonsoft.Json;
using System.IO;

namespace SoundBoard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string configFile = Path.Combine(exePath, "buttonsConfig.json");

            if (File.Exists(configFile))
            {
                string json = File.ReadAllText(configFile);
                List<ButtonConfig> buttonConfigs = JsonConvert.DeserializeObject<List<ButtonConfig>>(json);

                foreach (ButtonConfig buttonConfig in buttonConfigs)
                {
                    Button button = new Button
                    {
                        Text = buttonConfig.Text,
                        AutoSize = true,
                        Margin = new Padding(5),
                        Tag = buttonConfig.Id
                    };

                    button.Click += (sender, e) => {
                        // Add the desired behavior for the button click event
                        MessageBox.Show($"You clicked {button.Text}");
                    };

                    flowLayoutPanel1.Controls.Add(button);
                }
            }
            else
            {
                MessageBox.Show("buttonsConfig.json not found.");
            }
        }
    }
}