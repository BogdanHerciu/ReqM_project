using ReqM_Tool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ReqM_namespace
{
    public partial class ImageForm : Form
    {
        public ImageForm()
        {
            InitializeComponent();
        }

        private Main mainForm = null;

        public ImageForm(Form callingForm)
        {
            mainForm = callingForm as Main;
            InitializeComponent();

            /* Making form not resizable */
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            /* HeaderForm start position */
            this.StartPosition = FormStartPosition.Manual;
            int X = mainForm.Location.X + mainForm.Size.Width / 2 - this.Size.Width / 2;
            int Y = mainForm.Location.Y + mainForm.Size.Height / 2 - this.Size.Height / 2;
            this.Location = new Point(X, Y);
        }

        private void ImageForm_Load(object sender, EventArgs e)
        {
            try
            {
                pictureBox1.Image = null;
                bool pb = false;

                mainForm.imgFolderPath = mainForm.getImagePath();
                //Match match = new Match();
                textBox1.Font = new Font(textBox1.Font, FontStyle.Bold);

                textBox1.ReadOnly = false;
                textBox1.Text = mainForm.DGVText;
                string[] words = textBox1.Text.Split(' ');
                foreach (var word in words)
                {

                    Regex r = new Regex(".jpg|.jpeg|.png");
                    Match match = r.Match(word);

                    if (match.ToString() != "")
                    {
                        pictureBox1.Image = Image.FromFile(mainForm.imgFolderPath + "\\" + word);
                        pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
                        pictureBox1.MaximumSize = pictureBox1.Image.Size;
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pb = true;
                    }
                }
                if (pb == false)
                {
                    pictureBox1.Visible = false;
                    textBox1.Size = new Size(this.Width-40,this.Height-75);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Refresh();
        }

        private void ImageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
