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
    public partial class AddForm : Form
    {
        private Main mainForm = null;
        public AddForm()
        {
            InitializeComponent();
        }

        public AddForm(Form callingForm)
        {
            InitializeComponent();
            mainForm = callingForm as Main;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            column.Name = textBox1.Text.ToString();
            column.HeaderText = textBox1.Text.ToString();
            //column.DataPropertyName = ;
            mainForm.dgv.Columns.Add(column);

            foreach (var req in mainForm.listOfRequirements.Requirements_Dynamic_List)
            {
                req.newColumns.Add(new NewColumn { name = textBox1.Text.ToString(), value = ""});
            }
            this.Close();
        }

    }
}
