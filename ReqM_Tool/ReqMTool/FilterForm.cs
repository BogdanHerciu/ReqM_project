using ReqM_Tool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;


namespace ReqM_namespace
{
    public partial class FilterForm : Form
    {
        private Main mainForm = null;
        //public root_file listOfRequirementsCopy  = new root_file();

        public FilterForm()
        {
            InitializeComponent();
        }

        public FilterForm(Form callingForm)
        {
            mainForm = callingForm as Main;
            InitializeComponent();

            /* Making the form not resizable */
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            /* Filter Form start position */
            this.StartPosition = FormStartPosition.Manual;
            int X = mainForm.Location.X + mainForm.Size.Width / 2 - this.Size.Width / 2;
            int Y = mainForm.Location.Y + mainForm.Size.Height / 2 - this.Size.Height / 2;
            this.Location = new Point(X, Y);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            /* LINKSTO: Req048 */
            if (!checkBox1.Checked && this.mainForm.dgv.Columns.Contains("id"))
            {
                this.mainForm.dgv.Columns["id"].Visible = false;
                //this.mainForm.dgv.Columns.Remove(this.mainForm.dgv.Columns["id"]);
            }
            else
                this.mainForm.dgv.Columns["id"].Visible = true;
            if (!checkBox2.Checked && this.mainForm.dgv.Columns.Contains("description"))
            {
                this.mainForm.dgv.Columns["description"].Visible = false;
            }
            else
                this.mainForm.dgv.Columns["description"].Visible = true;
            if (!checkBox3.Checked && this.mainForm.dgv.Columns.Contains("status"))
            {
                this.mainForm.dgv.Columns["status"].Visible = false;
            }
            else
                this.mainForm.dgv.Columns["status"].Visible = true;
            if (!checkBox4.Checked && this.mainForm.dgv.Columns.Contains("CreatedBy"))
            {
                this.mainForm.dgv.Columns["CreatedBy"].Visible = false;
            }
            else
                this.mainForm.dgv.Columns["CreatedBy"].Visible = true;
            if (!checkBox5.Checked && this.mainForm.dgv.Columns.Contains("needscoverage"))
            {
                this.mainForm.dgv.Columns["needscoverage"].Visible = false;
            }
            else
                this.mainForm.dgv.Columns["needscoverage"].Visible = true;
            if (!checkBox6.Checked && this.mainForm.dgv.Columns.Contains("providescoverage"))
            {
                this.mainForm.dgv.Columns["providescoverage"].Visible = false;
            }
            else
                this.mainForm.dgv.Columns["providescoverage"].Visible = true;
            if (!checkBox7.Checked && this.mainForm.dgv.Columns.Contains("version"))
            {
                this.mainForm.dgv.Columns["version"].Visible = false;
            }
            else
                this.mainForm.dgv.Columns["version"].Visible = true;
            if (!checkBox8.Checked && this.mainForm.dgv.Columns.Contains("SafetyRelevant"))
            {
                this.mainForm.dgv.Columns["SafetyRelevant"].Visible = false;
            }
            else
                this.mainForm.dgv.Columns["SafetyRelevant"].Visible = true;
            if (!checkBox9.Checked && this.mainForm.dgv.Columns.Contains("ChangeRequest"))
            {
                this.mainForm.dgv.Columns["ChangeRequest"].Visible = false;
            }
            else
                this.mainForm.dgv.Columns["ChangeRequest"].Visible = true;
            if (!checkBox10.Checked && this.mainForm.dgv.Columns.Contains("ReviewID"))
            {
                this.mainForm.dgv.Columns["ReviewID"].Visible = false;
            }
            else
                this.mainForm.dgv.Columns["ReviewID"].Visible = true;
            if (!checkBox11.Checked && this.mainForm.dgv.Columns.Contains("RequirementType"))
            {
                this.mainForm.dgv.Columns["RequirementType"].Visible = false;
            }
            else
                this.mainForm.dgv.Columns["RequirementType"].Visible = true;
            if (!checkBox12.Checked && this.mainForm.dgv.Columns.Contains("Chapter"))
            {
                this.mainForm.dgv.Columns["Chapter"].Visible = false;
            }
            else
                this.mainForm.dgv.Columns["Chapter"].Visible = true;
            if (!checkBox13.Checked && this.mainForm.dgv.Columns.Contains("HWPlatform"))
            {
                this.mainForm.dgv.Columns["HWPlatform"].Visible = false;
            }
            else
                this.mainForm.dgv.Columns["HWPlatform"].Visible = true;
            if (!checkBox14.Checked && this.mainForm.dgv.Columns.Contains("Domain"))
            {
                this.mainForm.dgv.Columns["Domain"].Visible = false;
            }
            else
                this.mainForm.dgv.Columns["Domain"].Visible = true;
            if (!checkBox15.Checked && this.mainForm.dgv.Columns.Contains("TestedAt"))
            {
                this.mainForm.dgv.Columns["TestedAt"].Visible = false;
            }
            else
                this.mainForm.dgv.Columns["TestedAt"].Visible = true;
            if (!checkBox16.Checked && this.mainForm.dgv.Columns.Contains("ReqBaseline"))
            {
                this.mainForm.dgv.Columns["ReqBaseline"].Visible = false;
            }
            else
                this.mainForm.dgv.Columns["ReqBaseline"].Visible = true;

        }

        private void FilterForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < mainForm.listOfRequirements.list_of_settings.ElementAt(0).columns.Count; i++)
            {
                if (this.mainForm.dgv.Columns[0].Visible)
                    checkBox1.Checked = true;
                else
                    checkBox1.Checked = false;
                if (this.mainForm.dgv.Columns[1].Visible)
                    checkBox2.Checked = true;
                else
                    checkBox2.Checked = false;
                if (this.mainForm.dgv.Columns[2].Visible)
                    checkBox3.Checked = true;
                else
                    checkBox3.Checked = false;
                if (this.mainForm.dgv.Columns[3].Visible)
                    checkBox4.Checked = true;
                else
                    checkBox4.Checked = false;
                if (this.mainForm.dgv.Columns[4].Visible)
                    checkBox5.Checked = true;
                else
                    checkBox5.Checked = false;
                if (this.mainForm.dgv.Columns[5].Visible)
                    checkBox6.Checked = true;
                else
                    checkBox6.Checked = false;
                if (this.mainForm.dgv.Columns[6].Visible)
                    checkBox7.Checked = true;
                else
                    checkBox7.Checked = false;
                if (this.mainForm.dgv.Columns[7].Visible)
                    checkBox8.Checked = true;
                else
                    checkBox8.Checked = false;
                if (this.mainForm.dgv.Columns[8].Visible)
                    checkBox9.Checked = true;
                else
                    checkBox9.Checked = false;
                if (this.mainForm.dgv.Columns[9].Visible)
                    checkBox10.Checked = true;
                else
                    checkBox10.Checked = false;
                if (this.mainForm.dgv.Columns[10].Visible)
                    checkBox11.Checked = true;
                else
                    checkBox11.Checked = false;
                if (this.mainForm.dgv.Columns[11].Visible)
                    checkBox12.Checked = true;
                else
                    checkBox12.Checked = false;
                if (this.mainForm.dgv.Columns[12].Visible)
                    checkBox13.Checked = true;
                else
                    checkBox13.Checked = false;
                if (this.mainForm.dgv.Columns[13].Visible)
                    checkBox14.Checked = true;
                else
                    checkBox14.Checked = false;
                if (this.mainForm.dgv.Columns[14].Visible)
                    checkBox15.Checked = true;
                else
                    checkBox15.Checked = false;
                if (this.mainForm.dgv.Columns[15].Visible)
                    checkBox16.Checked = true;
                else
                    checkBox16.Checked = false;
            }

           // Console.WriteLine(this.mainForm.dgv.Columns[0].HeaderText.ToString());
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            /* LINKSTO: Req049 */
            //NO
            root_file listOfRequirementsCopy = (root_file)mainForm.listOfRequirements.ShallowCopy();

            for (int i = 0; i < mainForm.listOfRequirements.list_of_settings.ElementAt(0).columns.Count; ++i)
                Console.WriteLine("ORIGINAL:"+mainForm.listOfRequirements.list_of_settings.ElementAt(0).columns.ElementAt(i));
            for (int i = 0; i < listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Count; ++i)
                Console.WriteLine("COPY:"+listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.ElementAt(i));

            listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Clear();

            for (int i = 0; i < mainForm.listOfRequirements.list_of_settings.ElementAt(0).columns.Count; ++i)
                Console.WriteLine("ORIGINAL:" + mainForm.listOfRequirements.list_of_settings.ElementAt(0).columns.ElementAt(i));
            for (int i = 0; i < listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Count; ++i)
                Console.WriteLine("COPY:" + listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.ElementAt(i));

            if (checkBox1.Checked)
                listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Add("id");
            if (checkBox2.Checked)
                listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Add("description");
           if (checkBox3.Checked)
               listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Add("status");
           if (checkBox4.Checked)
               listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Add("CreatedBy");
           if (checkBox5.Checked)
               listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Add("needscoverage");
           if (checkBox6.Checked)
               listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Add("providescoverage");
           if (checkBox7.Checked)
               listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Add("version");
           if (checkBox8.Checked)
               listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Add("SafetyRelevant");
           if (checkBox9.Checked)
               listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Add("ChangeRequest");
           if (checkBox10.Checked)
               listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Add("ReviewID");
           if (checkBox11.Checked)
               listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Add("RequirementType");
           if (checkBox12.Checked)
               listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Add("Chapter");
           if (checkBox13.Checked)
               listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Add("HWPlatform");
           if (checkBox14.Checked)
               listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Add("Domain");
           if (checkBox15.Checked)
               listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Add("TestedAt");
           if (checkBox16.Checked)
               listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Add("ReqBaseline");

           for (int i = 0; i < mainForm.listOfRequirements.list_of_settings.ElementAt(0).columns.Count; ++i)
               Console.WriteLine("ORIGINAL:"+mainForm.listOfRequirements.list_of_settings.ElementAt(0).columns.ElementAt(i));
           for (int i = 0; i < listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.Count; ++i)
               Console.WriteLine("COPY:"+listOfRequirementsCopy.list_of_settings.ElementAt(0).columns.ElementAt(i));

            if (listOfRequirementsCopy.Equals(mainForm.listOfRequirements))
            {
                //NO
                MessageBox.Show("No filters applied!");
                return;
            }
            if (mainForm.XmlFilePath == null)
            {
                MessageBox.Show(mainForm.NoFileOpen);
                return;
            }

            try
            {
                /* Save the file to the XmlFilePath (the path from where was opened) */
                listOfRequirementsCopy.Save(mainForm.XmlFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}

