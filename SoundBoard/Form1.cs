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
            // Get the path of the executable and the config file.
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string configFile = Path.Combine(exePath, "buttonsConfig.json");

            // If the config file exists, read the button configurations.
            if (File.Exists(configFile))
            {
                string json = File.ReadAllText(configFile);
                List<ButtonConfig> buttonConfigs = JsonConvert.DeserializeObject<List<ButtonConfig>>(json);

                // Create and configure buttons for each button configuration.
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

                    // Add click event handler to play sound and execute desired behavior.
                    button.Click += (sender, e) => {
                        // Play sound
                        string soundPath = Path.Combine(exePath, "sounds", buttonConfig.SoundFile);
                        PlaySoundAsync(soundPath).ConfigureAwait(false);

                        // Add the desired behavior for the button click event
                        //MessageBox.Show($"You clicked {button.Text}");
                    };

                    // Add the button to the form's FlowLayoutPanel.
                    flowLayoutPanel1.Controls.Add(button);
                }
            }
            else
            {
                MessageBox.Show("buttonsConfig.json not found.");
            }
        }

        // Asynchronously plays a sound given a file path.
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

