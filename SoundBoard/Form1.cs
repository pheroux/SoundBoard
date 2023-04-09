using NAudio.Wave;
using Newtonsoft.Json;
using System.IO;
using NAudio.Wave;

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
                        Size = new Size(128, 128),
                        Margin = new Padding(5),
                        Tag = buttonConfig.Id,
                        AutoSize = false,
                        TextAlign = ContentAlignment.MiddleCenter,
                        TextImageRelation = TextImageRelation.Overlay
                    };

                    // Set the image as button background
                    string imagePath = Path.Combine(exePath, "images", buttonConfig.ImageFile);
                    if (File.Exists(imagePath))
                    {
                        try
                        {
                            using (FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                            {
                                button.BackgroundImage = Image.FromStream(stream);
                            }
                            button.BackgroundImageLayout = ImageLayout.Stretch;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error loading image: {ex.Message}");
                        }
                    }

                    button.Click += (sender, e) => {
                        // Play sound
                        string soundPath = Path.Combine(exePath, "sounds", buttonConfig.SoundFile);
                        PlaySoundAsync(soundPath).ConfigureAwait(false);

                        // Add the desired behavior for the button click event
                        //MessageBox.Show($"You clicked {button.Text}");
                    };

                    flowLayoutPanel1.Controls.Add(button);
                }
            }
            else
            {
                MessageBox.Show("buttonsConfig.json not found.");
            }
        }

        private async Task PlaySoundAsync(string soundPath)
        {
            if (File.Exists(soundPath))
            {
                using (var audioFile = new AudioFileReader(soundPath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();

                    // Wait for the sound to finish playing before disposing of the outputDevice
                    await Task.Delay((int)audioFile.TotalTime.TotalMilliseconds);
                }
            }
        }
    }
}
