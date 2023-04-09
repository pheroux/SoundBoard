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
            int numberOfButtons = 10; // Set the desired number of buttons

            for (int i = 0; i < numberOfButtons; i++)
            {
                Button button = new Button
                {
                    Text = $"Button {i + 1}",
                    AutoSize = true,
                    Margin = new Padding(5)
                };

                button.Click += (sender, e) => {
                    // Add the desired behavior for the button click event
                    MessageBox.Show($"You clicked {button.Text}");
                };

                flowLayoutPanel1.Controls.Add(button);
            }
        }
    }
}