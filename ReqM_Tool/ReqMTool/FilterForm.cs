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
    public partial class FilterForm : Form
    {
        private Main mainForm = null;

        public FilterForm()
        {
            InitializeComponent();
        }

        public FilterForm(Form callingForm)
        {
            mainForm = callingForm as Main;
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
//           int i = 0;
//           foreach(Control con in this.Controls)
//           {
//               i++;
//               if (con is CheckBox && con != null)
//               {
//                   this.mainForm.dgv.Columns.Remove(this.mainForm.dgv.Columns[i]);
//               }
//           }
//           Console.WriteLine(this.mainForm.dgv.RowCount);

            if (!checkBox1.Checked && this.mainForm.dgv.Columns.Contains("id"))
            {
                //cbox1 = true;
                this.mainForm.dgv.Columns.Remove(this.mainForm.dgv.Columns["id"]);
            }
            if (!checkBox2.Checked && this.mainForm.dgv.Columns.Contains("description"))
            {
                this.mainForm.dgv.Columns.Remove(this.mainForm.dgv.Columns["description"]);
            }
            if (!checkBox3.Checked && this.mainForm.dgv.Columns.Contains("status"))
            {
                this.mainForm.dgv.Columns.Remove(this.mainForm.dgv.Columns["status"]);
            }
            if (!checkBox3.Checked && this.mainForm.dgv.Columns.Contains("CreatedBy"))
            {
                this.mainForm.dgv.Columns.Remove(this.mainForm.dgv.Columns["CreatedBy"]);
            }
            if (!checkBox3.Checked && this.mainForm.dgv.Columns.Contains("needscoverage"))
            {
                this.mainForm.dgv.Columns.Remove(this.mainForm.dgv.Columns["needscoverage"]);
            }

        }

        private void FilterForm_Load(object sender, EventArgs e)
        {
            //checkBox1.Checked = Main.cbox1;
        }
    }
}
