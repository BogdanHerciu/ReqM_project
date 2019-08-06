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
        private List<string> images = new List<string>();
        int currentIndex;

        public ImageForm(Form callingForm)
        {
            mainForm = callingForm as Main;
            InitializeComponent();

            label1.Text = "";
            label2.Text = "";
            button1.Visible = false;
            button2.Visible = false;

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

                string mystring = mainForm.DGVText;
                mystring = mystring.Replace(System.Environment.NewLine, "<br />");

                textBox1.Text = mystring;
                string[] words = textBox1.Text.Split(' ');
                foreach (var word in words)
                {

                    Regex r = new Regex(".jpg|.jpeg|.png");
                    Match match = r.Match(word);

                    if (match.ToString() != "")
                    {
                        images.Add(word);
                        pb = true;
                    }
                }
                if (pb == false)
                {
                    pictureBox1.Visible = false;
                    textBox1.Size = new Size(this.Width - 40, this.Height - 75);
                }
                else
                {
                    button1.Visible = true;
                    button2.Visible = true;
                    currentIndex = 0;
                    DisplayImage(currentIndex);
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

        public void DisplayImage(int index)
        {
            pictureBox1.Image = Image.FromFile(mainForm.imgFolderPath + "\\" + images[currentIndex]);

            if (pictureBox1.Image.Width > pictureBox1.ClientSize.Width || pictureBox1.Image.Height > pictureBox1.ClientSize.Height)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            }

            label1.Text = index + 1 + "/" + images.Count;
            label2.Text = images[index];
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            if (currentIndex < images.Count - 1)
            {
                currentIndex++;
            }
            else
            {
                currentIndex = 0;
            }
            DisplayImage(currentIndex);
        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            mainForm.saved = false;
        }
    }
}
