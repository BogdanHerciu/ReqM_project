using ReqM_Tool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReqM_namespace
{
    public partial class HeaderForm : Form
    {
        private Main mainForm = null;
        public HeaderForm()
        {
            InitializeComponent();
        }

        public HeaderForm(Form callingForm)
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

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void HeaderForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = mainForm.listOfRequirements.list_of_settings.ElementAt(0).header.ToString();
        }
    }
}
