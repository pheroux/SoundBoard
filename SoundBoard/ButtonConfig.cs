using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundBoard
{
    public class ButtonConfig 
    {
        // Property to store the text displayed on the button.
        public string Text { get; set; }

        // Property to store the unique identifier for the button.
        public int Id { get; set; }

        // Property to store the file path of the image used as a button background.
        public string ImageFile { get; set; }

        // Property to store the file path of the sound to be played when the button is clicked.
        public string SoundFile { get; set; }
    }
}
