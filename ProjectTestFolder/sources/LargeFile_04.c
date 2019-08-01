/******************************************************************************
*
*   COMPANY:         NTT DATA Romania (19-21 Constanta Street, Cluj Napoca)
*
*    PROJECT:        2019 Summer Internship
*
*    FILE:           LargeFile_04.c
*
*    AUTHOR:         John Doe
*
*    DESCRIPTION:    This is a demo file used by the C# Application
*
*    HISTORY:        01_Aug-2019 Initial Version
*
**************************************************************************** */
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using function_namespace;
using ReqM_namespace;
using System.Runtime.InteropServices;
using Microsoft.Office.Core;

namespace ReqM_Tool
{


    public partial class Main : Form
    {
        /* create variable to the root of the xml file, for reading the requirements */
        public root_file listOfRequirements { get; set; } = new root_file();


        /* define lists of possible values */
        public List<string> status { get; }
        public List<string> safetyRelevant { get; }
        public List<string> domain { get; }
        public List<string> tested { get; }
        public List<string> type { get; }

        /* Data Table */
        DataTable dt = new DataTable();

        /* define columns from DataGridView */
        /*public int Column_ID = 0;
        public int Column_Description = 1;
        public int Column_Status = 2;
        public int Column_CreatedBy = 3;
        public int Column_needscoverage = 4;
        public int Column_providescoverage = 5;
        public int Column_version = 6;
        public int Column_SafetyRelevant = 7;
        public int Column_ChangeRequest = 8;
        public int Column_ReviewID = 9;
        public int Column_RequirementType = 10;
        public int Column_Chapter = 11;
        public int Column_HWPlatform = 12;
        public int Column_Domain = 13;
        public int Column_TestedAt = 14;
        public int Column_ReqBaseline = 15;
        public int Column_HWPlatform_COPY = 16;
        public int MAX_Columns = 17;*/

        /* "No File has been open" Text Box. */
        public string NoFileOpen { get; } = "No file has been opened!";

        /* FilterForm checkBoxes */
        public bool cbox1;

        public bool OpenFileFinished = false;

        /* All the files from a folder. */
        List<MyFile> listOfFiles;
        OpenFileDialog FileDialog = new OpenFileDialog();
        /* The path of the xml file. */
        public string XmlFilePath { get; set; }
        /* Path in where we search for the Requirements that needs to cover the Implementation. */
        string implementationFilePath;
        /* Path in where we search for the Requirements that needs to cover the Tests. */
        string testFilePath;
        // FileDialog.Filter = "XML|*.xml";

        public Main()
        {
            InitializeComponent();

            status = new List<string> { "Accepted by project", "Ready for review", "Discarded by project", "In work", "Draft" };
            safetyRelevant = new List<string> { "N/A", "QM", "ASIL A", "ASIL B", "ASIL C", "ASIL D" };
            domain = new List<string> { "N/A", "SW", "MD", "HW" };
            tested = new List<string> { "N/A", "SYS.5", "SYS.4", "SWE.6", "SWE.5", "SWE.4", "DEV Test/Review" };
            type = new List<string> { "Description", "Technical Requirement", "Project Requirement", "Functional Requirement", "Non-Functional Requirement", "Template" };
            dataGridView1.AutoGenerateColumns = false;
        }

        public DataGridView dgv
        {
            get { return dataGridView1; }
        }

        /* Method called when a cell is modified */
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            /* LINKSTO: Req076 */
            /* Color each column that is changed. */
            if (OpenFileFinished == true)
            {
                foreach (DataGridViewColumn column in this.dataGridView1.Columns)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[column.Index].Style.BackColor = Color.Yellow;
                }
                /* Increment the Requirement Baseline with 0.1.  */
                double DocumentBaseline = Convert.ToDouble(listOfRequirements.list_of_settings[0].Baseline);
                listOfRequirements.Requirements_Dynamic_List[e.RowIndex].ReqBaseline = Convert.ToString(DocumentBaseline + 0.1);

            }

            /* Color change for needscoverage column. */
            /* If the Value is neither tst nor src, color the box in red. */

            if (dataGridView1.Columns[e.ColumnIndex].Name == "needscoverage")
            {
                if (
                (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "tst") ||
                (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "src")
                )
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                }
            }
        }

        public void UpdateDT()
        {
            /* Clears DataTable */
            dt.Columns.Clear();
            dt.Rows.Clear();

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                dt.Columns.Add(col.Name);
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataRow dRow = dt.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                dt.Rows.Add(dRow);
            }
        }

        private void OpenBtn_Click(object sender, EventArgs e)
        {

        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {

        }

        private void AddBtn_Click(object sender, EventArgs e)
        {

        }
        private void DeleteBtn_Click(object sender, EventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            /* for providescoverage column */
            if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "providescoverage")
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null || !dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.Equals("N/A"))
                {
                    /* value of providescoverage column */
                    string value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    /* path of xml files */
                    string path = XmlFilePath.Substring(0, XmlFilePath.LastIndexOf("\\"));                

                    /* search in each file from the folder "path" */
                    foreach (string file in Directory.GetFiles(path, "*.xml"))
                    {
                        string content = File.ReadAllText(file);
                        Regex r = new Regex(value);
                        Match match = r.Match(content);

                        if (match.ToString() != "")
                        {
                            /* current xml file */
                            if(file.Equals(XmlFilePath))
                            {
                                int index = SearchReq(value);
                                if (index > -1)
                                    dataGridView1.FirstDisplayedScrollingRowIndex = index;
                            }
                            /* another xml file */
                            else
                            {
                                try
                                {
                                    /* create a serializer for the requirements */
                                    XmlSerializer serializer = new XmlSerializer(typeof(root_file));
                                    /* read the data from the xml file */
                                    StreamReader reader = new StreamReader(file);
                                    /* dezerialize the data */
                                    listOfRequirements = (root_file)serializer.Deserialize(reader);

                                    /* find index of requirement */
                                    int index = SearchReq(value);
                                    /* if found */
                                    if (index > -1)
                                    {
                                        /* clear table */
                                        //dataGridView1.Data;

                                        /* add the data into the table */
                                        //CreateDataGridView();
                                        //AddCustomColumns();
                                        dataGridView1.DataSource = listOfRequirements.Requirements_Dynamic_List;
                                        dataGridView1.Refresh();
                                        //DisplayDefaultColumns();

                                        dataGridView1.FirstDisplayedScrollingRowIndex = index;
                                        UpdateDT();
                                        CheckBaseline();
                                        CheckNeedscoverage();

                                        XmlFilePath = file;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                }

                            }

                            return;
                        }
                    }
                }
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void ImplementationPathButton_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        public void CreateDataGridView()
        {
            DataGridViewTextBoxColumn textBoxColumn = new DataGridViewTextBoxColumn();
            textBoxColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textBoxColumn.DataPropertyName = "id";
            textBoxColumn.Name = "id";
            textBoxColumn.HeaderText = "id";
            dataGridView1.Columns.Add(textBoxColumn);

            DataGridViewTextBoxColumn textBoxColumn2 = new DataGridViewTextBoxColumn();
            textBoxColumn2.Name = "description";
            textBoxColumn2.HeaderText = "description";
            textBoxColumn2.DataPropertyName = "description";
            dataGridView1.Columns.Add(textBoxColumn2);

            DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn();
            comboBoxColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            comboBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            comboBoxColumn.Name = "status";
            comboBoxColumn.HeaderText = "status";
            comboBoxColumn.DataPropertyName = "status";
            comboBoxColumn.DataSource = status;
            dataGridView1.Columns.Add(comboBoxColumn);

            DataGridViewTextBoxColumn textBoxColumn3 = new DataGridViewTextBoxColumn();
            textBoxColumn3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textBoxColumn3.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textBoxColumn3.Name = "CreatedBy";
            textBoxColumn3.HeaderText = "CreatedBy";
            textBoxColumn3.DataPropertyName = "CreatedBy";
            dataGridView1.Columns.Add(textBoxColumn3);

            DataGridViewTextBoxColumn textBoxColumn4 = new DataGridViewTextBoxColumn();
            textBoxColumn4.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textBoxColumn4.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textBoxColumn4.Name = "needscoverage";
            textBoxColumn4.HeaderText = "needscoverage";
            textBoxColumn4.DataPropertyName = "needscoverage";
            dataGridView1.Columns.Add(textBoxColumn4);

            DataGridViewTextBoxColumn textBoxColumn5 = new DataGridViewTextBoxColumn();
            textBoxColumn5.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textBoxColumn5.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textBoxColumn5.Name = "providescoverage";
            textBoxColumn5.HeaderText = "providescoverage";
            textBoxColumn5.DataPropertyName = "providescoverage";
            dataGridView1.Columns.Add(textBoxColumn5);

            DataGridViewTextBoxColumn textBoxColumn6 = new DataGridViewTextBoxColumn();
            textBoxColumn6.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textBoxColumn6.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textBoxColumn6.Name = "version";
            textBoxColumn6.HeaderText = "version";
            textBoxColumn6.DataPropertyName = "version";
            dataGridView1.Columns.Add(textBoxColumn6);

            DataGridViewComboBoxColumn comboBoxColumn2 = new DataGridViewComboBoxColumn();
            comboBoxColumn2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            comboBoxColumn2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            comboBoxColumn2.Name = "SafetyRelevant";
            comboBoxColumn2.HeaderText = "SafetyRelevant";
            comboBoxColumn2.DataPropertyName = "SafetyRelevant";
            comboBoxColumn2.DataSource = safetyRelevant;
            dataGridView1.Columns.Add(comboBoxColumn2);

            DataGridViewTextBoxColumn textBoxColumn7 = new DataGridViewTextBoxColumn();
            textBoxColumn7.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textBoxColumn7.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textBoxColumn7.Name = "ChangeRequest";
            textBoxColumn7.HeaderText = "ChangeRequest";
            textBoxColumn7.DataPropertyName = "ChangeRequest";
            dataGridView1.Columns.Add(textBoxColumn7);

            DataGridViewTextBoxColumn textBoxColumn8 = new DataGridViewTextBoxColumn();
            textBoxColumn8.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textBoxColumn8.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textBoxColumn8.Name = "ReviewID";
            textBoxColumn8.HeaderText = "ReviewID";
            textBoxColumn8.DataPropertyName = "ReviewID";
            dataGridView1.Columns.Add(textBoxColumn8);

            DataGridViewComboBoxColumn comboBoxColumn3 = new DataGridViewComboBoxColumn();
            comboBoxColumn3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            comboBoxColumn3.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            comboBoxColumn3.Name = "RequirementType";
            comboBoxColumn3.HeaderText = "RequirementType";
            comboBoxColumn3.DataPropertyName = "RequirementType";
            comboBoxColumn3.DataSource = type;
            dataGridView1.Columns.Add(comboBoxColumn3);

            DataGridViewComboBoxColumn comboBoxColumn4 = new DataGridViewComboBoxColumn();
            comboBoxColumn4.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            comboBoxColumn4.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            comboBoxColumn4.Name = "Chapter";
            comboBoxColumn4.HeaderText = "Chapter";
            comboBoxColumn4.DataPropertyName = "Chapter";
            comboBoxColumn4.DataSource = listOfRequirements.customValues.chapters;
            dataGridView1.Columns.Add(comboBoxColumn4);

            DataGridViewComboBoxColumn comboBoxColumn5 = new DataGridViewComboBoxColumn();
            comboBoxColumn5.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            comboBoxColumn5.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            comboBoxColumn5.Name = "HWPlatform";
            comboBoxColumn5.HeaderText = "HWPlatform";
            comboBoxColumn5.DataPropertyName = "HWPlatform";
            comboBoxColumn5.DataSource = listOfRequirements.customValues.hwPlatforms;
            dataGridView1.Columns.Add(comboBoxColumn5);

            DataGridViewComboBoxColumn comboBoxColumn7 = new DataGridViewComboBoxColumn();
            comboBoxColumn7.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            comboBoxColumn7.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            comboBoxColumn7.Name = "Domain";
            comboBoxColumn7.HeaderText = "Domain";
            comboBoxColumn7.DataPropertyName = "Domain";
            comboBoxColumn7.DataSource = domain;
            dataGridView1.Columns.Add(comboBoxColumn7);

            DataGridViewComboBoxColumn comboBoxColumn6 = new DataGridViewComboBoxColumn();
            comboBoxColumn6.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            comboBoxColumn6.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            comboBoxColumn6.Name = "TestedAt";
            comboBoxColumn6.HeaderText = "TestedAt";
            comboBoxColumn6.DataPropertyName = "TestedAt";
            comboBoxColumn6.DataSource = tested;
            dataGridView1.Columns.Add(comboBoxColumn6);

            DataGridViewTextBoxColumn textBoxColumn9 = new DataGridViewTextBoxColumn();
            textBoxColumn9.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textBoxColumn9.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textBoxColumn9.Name = "ReqBaseline";
            textBoxColumn9.HeaderText = "ReqBaseline";
            textBoxColumn9.DataPropertyName = "ReqBaseline";
            dataGridView1.Columns.Add(textBoxColumn9);

            dataGridView1.DataError += new DataGridViewDataErrorEventHandler(dgvCombo_DataError);
        }


        void dgvCombo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // (No need to write anything in here)
        }

        public void AddCustomColumns()
        {
            foreach (var column in listOfRequirements.Requirements_Dynamic_List.ElementAt(0).newColumns)
            {
                DataGridViewTextBoxColumn newColumn = new DataGridViewTextBoxColumn();
                newColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                newColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                newColumn.Name = column.name;
                newColumn.HeaderText = column.name;
                newColumn.DataPropertyName = "value";
                dataGridView1.Columns.Add(newColumn);
            }
        }

        public void DisplayDefaultColumns()
        {
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Visible = false;
            }

            if (listOfRequirements.list_of_settings.ElementAt(0).columns.Count > 0)
            {
                for (int i = 0; i < listOfRequirements.list_of_settings.ElementAt(0).columns.Count; i++)
                {
                    string column = listOfRequirements.list_of_settings.ElementAt(0).columns.ElementAt(i);
                    dataGridView1.Columns[column].Visible = true;
                }
            }
            else
            {
                dataGridView1.Columns["id"].Visible = true;
                dataGridView1.Columns["description"].Visible = true;
                dataGridView1.Columns["status"].Visible = true;
                dataGridView1.Columns["CreatedBy"].Visible = true;
                dataGridView1.Columns["needscoverage"].Visible = true;
                dataGridView1.Columns["providescoverage"].Visible = true;
                dataGridView1.Columns["version"].Visible = true;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                /* Clear DataGridView if other xml is opened */
                dataGridView1.DataSource = null;
                XmlFilePath = null;
                dataGridView1.Columns.Clear();
                dataGridView1.Refresh();

                /* build the XML file path */
                XmlFilePath = FileDialog.FileName;

                string extension = Path.GetExtension(XmlFilePath);

                /* verify file format */
                if (extension != ".xml")
                {
                    MessageBox.Show("Unsupported file format. Please open supported Requirements file.");
                    XmlFilePath = null;
                    return;
                }
                try
                {
                    /* create a serializer for the requirements */
                    XmlSerializer serializer = new XmlSerializer(typeof(root_file));
                    /* read the data from the xml file */
                    StreamReader reader = new StreamReader(XmlFilePath);
                    /* dezerialize the data */
                    listOfRequirements = (root_file)serializer.Deserialize(reader);
                    reader.Close();

                    /* add the data into the table */
                    CreateDataGridView();
                    AddCustomColumns();
                    dataGridView1.DataSource = listOfRequirements.Requirements_Dynamic_List;

                    /* Clears DataTable */
                    dt.Columns.Clear();
                    dt.Rows.Clear();

                    /* default columns to display */
                    DisplayDefaultColumns();

                    /* add event for Cell value changed */
                    dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
                    dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;

                    /* color with red needscoverage column if there's an error! */
                    CheckNeedscoverage();  
                    CheckBaseline();
                    UpdateDT();
                    
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                /* Center the text for all columns except "Description" column. */
                /*foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    if (c.Name != "description")
                        dataGridView1.Columns[c.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }*/

                
            }
            OpenFileFinished = true;
        }

        public void CheckNeedscoverage()
        {
            for (int index = 0; index < (listOfRequirements.Requirements_Dynamic_List.Count()); index++)
            {
                if (
                    (listOfRequirements.Requirements_Dynamic_List[index].needscoverage.ToString() != "tst") &&
                    (listOfRequirements.Requirements_Dynamic_List[index].needscoverage.ToString() != "src")
                    )
                {
                    int ind = dataGridView1.Columns["needscoverage"].Index;
                    dataGridView1.Rows[index].Cells[ind].Style.BackColor = Color.Red;
                }
            }
        }

        public void CheckBaseline()
        {
            /* LINKSTO: Req076 */
            /* Color the requirement with Yellow if it has different baseline than the document Baseline. */
            /* Color each column that is changed. */
            Console.WriteLine(dataGridView1.Rows.Count);
            foreach (DataGridViewRow Row in dataGridView1.Rows)
            {
                if (listOfRequirements.Requirements_Dynamic_List[Row.Index].ReqBaseline != listOfRequirements.list_of_settings[0].Baseline)
                {
                    /* Color each column that is changed. */
                    foreach (DataGridViewColumn Column in dataGridView1.Columns)
                    {
                        dataGridView1.Rows[Row.Index].Cells[Column.Index].Style.BackColor = Color.Yellow;
                    }
                }
            }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (XmlFilePath == null)
            {
                MessageBox.Show(NoFileOpen);
                return;
            }

            try
            {
                /* Save the file to the XmlFilePath (the path from where was opened) */
                listOfRequirements.list_of_settings.ElementAt(0).columns.Clear();

                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    listOfRequirements.list_of_settings.ElementAt(0).columns.Add(c.Name);
                }
                listOfRequirements.Save(XmlFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (XmlFilePath == null)
            {
                MessageBox.Show(NoFileOpen);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XML|*.xml";
            /* open File dialog where to save the file */
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    listOfRequirements.list_of_settings.ElementAt(0).columns.Clear();

                    foreach (DataGridViewColumn c in dataGridView1.Columns)
                    {
                        listOfRequirements.list_of_settings.ElementAt(0).columns.Add(c.Name);
                    }
                    listOfRequirements.SaveAs(sfd.FileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private void addRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (XmlFilePath == null)
            {
                MessageBox.Show(NoFileOpen);
                return;
            }
            else
            {
                /* make sure that AllowUserToAddRows is set to 'true' */
                dataGridView1.AllowUserToAddRows = true;
                if (this.dataGridView1.SelectedRows.Count > 0)
                {
                    /* get the selected row */
                    int selected_row = (this.dataGridView1.SelectedRows[0].Index);


                    /* INSERT AFTER: add a new element to Requirements_Dynamic_List "database" */
                    listOfRequirements.Requirements_Dynamic_List.Insert(selected_row + 1, new RequirementItem()
                    {
                        id = "id",                                /* default text for the id */
                        description = "Please enter the description of the requirement",    /* default text for the description */
                        status = "Draft",                         /* default text for the Status */
                        CreatedBy = "Author of the requirement",  /* default text for the CreatedBy */
                        needscoverage = "To be linked",           /* default text for the needscoverage */
                        providescoverage = "To be linked",        /* default text for the providescoverage */
                        version = "0.1",                          /* default text for the version */
                        SafetyRelevant = "N/A",                   /* default text for the SafetyRelevant */
                        ChangeRequest = "Change Request ID",
                        ReviewID = "Review Ticket ID",
                        RequirementType = "Template",
                        Chapter = listOfRequirements.customValues.chapters.ElementAt(0), //"first element from the list",
                        HWPlatform = listOfRequirements.customValues.hwPlatforms.ElementAt(0),
                        Domain = "N/A",
                        TestedAt = "N/A",
                        ReqBaseline = "1.0",

                    });

                    /* refresh the dataGridView */
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = listOfRequirements.Requirements_Dynamic_List;
                }
                else
                {
                    MessageBox.Show("No row is selected for adding a new item");
                }
            }
            CheckNeedscoverage();
            CheckBaseline();
            UpdateDT();
        }

        private void deleteRowToolStripMenuItem_Click(object sender, EventArgs e)
        {


            /* check if a XML file is open */
            if (XmlFilePath == null)
            {
                MessageBox.Show(NoFileOpen);
                return;
            }
            else
            {
                if (this.dataGridView1.SelectedRows.Count > 0)
                {
                    /* get the selected row */
                    int selected_row = (this.dataGridView1.SelectedRows[0].Index);

                    /* remove the row from the List */
                    listOfRequirements.Requirements_Dynamic_List.RemoveAt(selected_row);

                    /* refresh the dataGridView */
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = listOfRequirements.Requirements_Dynamic_List;
                }
                else
                {
                    MessageBox.Show("No row is selected for removal.");
                }
            }
            CheckNeedscoverage();
            CheckBaseline();
            UpdateDT();
        }

        private void selectSourcePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* get the file path for the "source" folder(folder in where are the .c and .h files) */
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                /* save "source" folder path */
                implementationFilePath = fbd.SelectedPath;
            }
        }

        private void selectTestPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* get the file path for the "test" folder(folder in where are the .c and .h files) */
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                /* save "test" folder path */
                testFilePath = fbd.SelectedPath;
            }
        }

        private void makeCoverageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* check the paths for null */
            if (false == myFunctions.Paths(implementationFilePath, testFilePath, XmlFilePath))
            {
                return;
            }

            /* create variable to the root of the xml file */
            root_file listOfRequirements = null;
            /* create a serializer */
            XmlSerializer serializer = new XmlSerializer(typeof(root_file));
            /* read the data from the xml file */
            StreamReader reader = new StreamReader(XmlFilePath);
            /* dezerialize the data */
            listOfRequirements = (root_file)serializer.Deserialize(reader);

            /* SEARCH EACH REQUIREMENT_ID IN FILES. */
            listOfFiles = new List<MyFile>();
            string content = string.Empty;
            /* Clear the Grid View */
            dataGridView2.Rows.Clear();
            /* search in folders string */
            string pattern;
            /* check if the requirement has been found or not in all folders FLAG */
            bool requirementFound = false;

            for (int index = 0; index < (listOfRequirements.Requirements_Dynamic_List.Count()); index++)
            {
                /* requirement version */
                string version = listOfRequirements.Requirements_Dynamic_List[index].version.ToString();

                /* the name and version of the requirement which is searched in all files */
                pattern = listOfRequirements.Requirements_Dynamic_List[index].id + ", " + "\\d\\.\\d";
                MyFile mf = new MyFile();
                Match matches;
                string selectable_path = null;
                /* flag to check if the requirement is not found in any file */
                requirementFound = false;

                /* is a test requirement or a source requirement? */
                selectable_path = myFunctions.getPath(implementationFilePath, testFilePath, listOfRequirements.Requirements_Dynamic_List[index].needscoverage, index);
                if (selectable_path == null)
                {
                    /* if no path is selected, exit the function */
                    return;
                }

                foreach (string word in selectable_path.Split('*'))
                {
                    /* search in each file from the folder "selectable_path" */
                    foreach (string file in Directory.GetFiles(word, "*.*"))
                    {
                        content = File.ReadAllText(file);
                        Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                        matches = r.Match(content);

                        /* if Requirement is found. Add into the table. */
                        if (matches.ToString() != "")
                        {
                            int rowId = dataGridView2.Rows.Add();
                            /* grab the new row */
                            DataGridViewRow row = dataGridView2.Rows[rowId];

                            /* add the data */
                            /* Set Req_ID and Covered cell only first time */
                            if (requirementFound == false)
                            {
                                row.Cells[0].Value = pattern.Substring(0, pattern.IndexOf(","));
                                row.Cells[1].Value = "Covered";
                                /* TODO: merge cells!!! */
                            }
                            //row.Cells[2].Value = Path.GetFileName(file);
                            row.Cells[2].Value = word;
                            /* Version Mismatch */
                            if (version == matches.Value.Substring(matches.Value.LastIndexOf(", ") + 2, 3))
                            {
                                row.Cells[3].Value = "Version OK";
                            }
                            else
                            {
                                row.Cells[3].Value = "Version mismatch";
                                row.Cells[3].Style.BackColor = Color.Red;
                            }
                            /* set the color */
                            row.DefaultCellStyle.BackColor = Color.Green;

                            /* mark as a found requirement */
                            requirementFound = true;
                        }
                    } /* end foreach */

                    /* if the requirement has not been found, add requirement_id and "notCovered" text */
                    if (requirementFound == false)
                    {
                        myFunctions.ReqNotFound_AddRow(dataGridView2, pattern.Substring(0, pattern.IndexOf(",")));
                    }
                }

                
            }
            /* close the reader */
            reader.Close();
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure?", "Close Document", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                dataGridView1.DataSource = null;
                XmlFilePath = null;
                dataGridView1.Columns.Clear();
                dataGridView1.Refresh();
            }

        }

        private void PublishToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void SaveAsExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* Check if a XML file is open. */
            if (XmlFilePath == null)
            {
                MessageBox.Show(NoFileOpen);
                return;
            }
            else
            {
                //DataSet ds = new DataSet();

                //Convert the XML into Dataset
                //ds.ReadXml(XmlFilePath);

                //Retrieve the table fron Dataset
                // DataTable dt = ds.Tables[1];

                // Create an Excel object
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                // Create workbook object
                Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(Type.Missing);
                // Create worksheet object
                Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.ActiveSheet;

                // index for nr. of columns
                int index = 0;
                for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                {
                    // int n = dataGridView1.Columns[i - 1].HeaderText.Length;
                    index++;
                    worksheet.Cells.ColumnWidth = 20;
                    worksheet.Cells[5, 2].ColumnWidth = 40;
                    worksheet.Cells[5, i] = dataGridView1.Columns[i - 1].HeaderText;
                }
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 6, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }

                /* NTT logo */
                var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string filePath = Path.Combine(projectPath, "Resources");
                worksheet.Shapes.AddPicture(filePath + "\\NTT.png", MsoTriState.msoFalse, MsoTriState.msoCTrue, 0, 3, 300, 57);

                /* Date&Project */
                worksheet.Cells[2, 3] = "R.A.D.U. - Requirements And Design Utility";
                worksheet.Cells[2, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                worksheet.Cells[3, 3] = DateTime.Now.ToString("dddd, dd MMMM yyyy");
                worksheet.Cells[3, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                /* Headings color */
                var columnHeadingsRange = excel.Range[excel.Cells[5, 1], excel.Cells[5, index]];
                columnHeadingsRange.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbSkyBlue;

                /* Cells align - centered */
                worksheet.Cells.Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                worksheet.Cells.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                /* Description cells align - top left */
                var descriptionRange = excel.Range[excel.Cells[5, 2], excel.Cells[dataGridView1.Rows.Count + 5, 2]];
                descriptionRange.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                descriptionRange.Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;

                /* Title (name of the XML file) */
                string XmlFileName = XmlFilePath.Substring(XmlFilePath.LastIndexOf("\\") + 1);
                XmlFileName = XmlFileName.Substring(0, XmlFileName.LastIndexOf("."));

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel|*.xlsx";
                sfd.FileName = XmlFileName;

                /* open File dialog where to save the file */
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        //Save the workbook
                        workbook.SaveAs(sfd.FileName);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }

                //Close the Workbook
                workbook.Close();

                // Finally Quit the Application
                ((Microsoft.Office.Interop.Excel._Application)excel).Quit();
            }
        }

        private void Label2_Click_1(object sender, EventArgs e)
        {

        }
        private void Button1_Click_2(object sender, EventArgs e)
        {
            /* Total number of requirements. */
            float countReq = 0;
            /* Number of found requirements. */
            float countReqFound = 0;
            float coverage = 0;

            string content = string.Empty;
            MatchCollection matches;

            /* Check the paths for null. */
            if (false == myFunctions.Paths(implementationFilePath, testFilePath, XmlFilePath))
            {
                return;
            }

            /* Path of xml files. */
            string path = XmlFilePath;
            int indx = path.LastIndexOf("\\");
            if (indx > 0)
                path = path.Substring(0, indx + 1);

            foreach (string file in Directory.GetFiles(path, "*Software*"))
            {
                /* Create variable to the root of the xml file. */
                root_file listOfRequirements = null;
                /* Create a serializer. */
                XmlSerializer serializer = new XmlSerializer(typeof(root_file));
                /* Read the data from the xml file. */
                StreamReader reader = new StreamReader(file);
                /* Dezerialize the data. */
                listOfRequirements = (root_file)serializer.Deserialize(reader);

                /* Count RequirementItems. */
                countReq += listOfRequirements.Requirements_Dynamic_List.Count();

                /* Search in folders string. */
                string pattern;
                for (int index = 0; index < (listOfRequirements.Requirements_Dynamic_List.Count()); index++)
                {
                    /* ID - the name of the Requirement which is searched in all files. */
                    pattern = listOfRequirements.Requirements_Dynamic_List[index].id;

                    string selectable_path = implementationFilePath;

                    /* Search in each file from the folder "selectable_path". */
                    foreach (string file2 in Directory.GetFiles(selectable_path, "*.*"))    //.c.h
                    {
                        content = File.ReadAllText(file2);
                        Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                        matches = r.Matches(content);

                        /* If Requirement is found, cout it. */
                        if (matches.Count > 0)
                        {
                            countReqFound++;
                            break;
                        }
                    }

                    selectable_path = testFilePath;

                    /* Search in each file from the folder "selectable_path". */
                    foreach (string file2 in Directory.GetFiles(selectable_path, "*.*"))    //.c.h
                    {
                        content = File.ReadAllText(file2);
                        Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                        matches = r.Matches(content);

                        /* If Requirement is found, count it. */
                        if (matches.Count > 0)
                        {
                            countReqFound++;
                            break;
                        }
                    }
                }
            }
            coverage = (countReqFound / countReq) * 100;

            textBox1.Text = countReq.ToString();
            textBox2.Text = coverage.ToString() + "%";

            chart1.Series["Series1"].Points.AddXY("Requirements missing", countReq - countReqFound);
            chart1.Series["Series1"].Points.AddXY("Found Requirements", countReqFound);
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            if (XmlFilePath != null)
            {
                if ((Application.OpenForms["FilterForm"] as FilterForm) == null)
                {
                    FilterForm myform = new FilterForm(this);
                    myform.Show();
                }
            }
            else
                MessageBox.Show(NoFileOpen);
        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void BaselineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* Check if a XML file is open. */
            if (XmlFilePath == null)
            {
                MessageBox.Show(NoFileOpen);
                return;
            }
            else
            {
                if (MessageBox.Show("Are you sure that you want to Baseline the Document?", "Baseline", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    /* LINKSTO: Req083 */
                    /* Increment the Requirement Baseline with 1.0.  */
                    double DocumentBaseline = Convert.ToDouble(listOfRequirements.list_of_settings[0].Baseline);
                    DocumentBaseline += 1.0;
                    listOfRequirements.list_of_settings[0].Baseline = DocumentBaseline.ToString();//Convert.ToString(DocumentBaseline);
                    /* Set the Requirement Baseline to the Document Baseline. */
                    foreach (DataGridViewRow Row in this.dataGridView1.Rows)
                    {
                        listOfRequirements.Requirements_Dynamic_List[Row.Index].ReqBaseline = listOfRequirements.list_of_settings[0].Baseline;
                    }
                    /* Gray the rows that are colored with yellow. */
                    foreach (DataGridViewRow Row in this.dataGridView1.Rows)
                    {
                        if (listOfRequirements.Requirements_Dynamic_List[Row.Index].ReqBaseline == listOfRequirements.list_of_settings[0].Baseline)
                        {
                            /* Color each column back to "gray". */
                            foreach (DataGridViewColumn Column in this.dataGridView1.Columns)
                            {
                                dataGridView1.Rows[Row.Index].Cells[Column.Index].Style.BackColor = Color.White;
                            }
                        }
                    }
                }
            }

        }

        private void AddColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* Check if a XML file is open. */
            if (XmlFilePath == null)
            {
                MessageBox.Show(NoFileOpen);
                return;
            }
            else
            {
                AddForm form = new AddForm(this);
                form.ShowDialog();
            }
        }

        private void SearchBox_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            /* LINKSTO: Req050 */
           
            if (XmlFilePath != null)
            {
                if (searchBox.Text != " Search")
                {
                    DataView dv = new DataView(dt);
                    string columns = "";

                    foreach (DataGridViewColumn c in dataGridView1.Columns)
                    {
                        if (c.Visible)
                            columns += c.HeaderText + " LIKE '%{0}%' OR ";
                    }

                    columns = columns.Substring(0, columns.Length - 3);
                    dv.RowFilter = string.Format(columns, searchBox.Text);
                    dataGridView1.DataSource = dv.ToTable();             
                    CheckBaseline();
                    CheckNeedscoverage();
                }
            }
        }

        private void SearchBox_Click(object sender, EventArgs e)
        {
            if (searchBox.Text == " Search")
                searchBox.Text = "";
        }

        private void SearchBox_Leave(object sender, EventArgs e)
        {
            if (searchBox.Text == "")
                searchBox.Text = " Search";
        }

        private void Label5_Click(object sender, EventArgs e)
        {

        }

        private void SrcButton_Click(object sender, EventArgs e)
        {
            /* get the file path for the "source" folder(folder in where are the .c and .h files) */
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                /* save "source" folder path */
                implementationFilePath = fbd.SelectedPath;
            }
            srcTextBox.Text = implementationFilePath;
        }

        private void TstButton_Click(object sender, EventArgs e)
        {
            /* get the file path for the "test" folder(folder in where are the .c and .h files) */
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                /* save "test" folder path */
                testFilePath = fbd.SelectedPath;
            }
            tstTextBox.Text = testFilePath;
        }

        private void PublishToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            /* Check if a XML file is open. */
            if (XmlFilePath == null)
            {
                MessageBox.Show(NoFileOpen);
                return;
            }
            else
            {
                // Create an Excel object
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                //Create workbook object
                Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(Type.Missing);
                //Create worksheet object
                Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.ActiveSheet;

                int index = 0;
                for (int i = 1; i < dataGridView2.Columns.Count + 1; i++)
                {
                    index++;
                    worksheet.Cells.ColumnWidth = 20;
                    worksheet.Cells[5, i] = dataGridView2.Columns[i - 1].HeaderText;
                }
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView2.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 6, j + 1] = dataGridView2.Rows[i].Cells[j].Value.ToString();
                    }
                }

                /* NTT logo */
                var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string filePath = Path.Combine(projectPath, "Resources");
                worksheet.Shapes.AddPicture(filePath + "\\NTT.png", MsoTriState.msoFalse, MsoTriState.msoCTrue, 0, 3, 300, 57);

                /* Date&Project */
                worksheet.Cells[2, 4] = "R.A.D.U. - Requirements And Design Utility";
                worksheet.Cells[2, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                worksheet.Cells[3, 4] = DateTime.Now.ToString("dddd, dd MMMM yyyy");
                worksheet.Cells[3, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                /* Headings color */
                var columnHeadingsRange = excel.Range[excel.Cells[5, 1], excel.Cells[5, index]];
                columnHeadingsRange.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbSkyBlue;

                /* Cells align - centered */
                worksheet.Cells.Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                worksheet.Cells.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                /* Title (name of the XML file) */
                string XmlFileName = XmlFilePath.Substring(XmlFilePath.LastIndexOf("\\") + 1);
                XmlFileName = XmlFileName.Substring(0, XmlFileName.LastIndexOf("."));
                XmlFileName += "_Report";

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel|*.xlsx";
                sfd.FileName = XmlFileName;

                /* open File dialog where to save the file */
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        //Save the workbook
                        workbook.SaveAs(sfd.FileName);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }

                //Close the Workbook
                workbook.Close();

                // Finally Quit the Application
                ((Microsoft.Office.Interop.Excel._Application)excel).Quit();
            }
        }

        public int SearchReq(string id)
        {
            for (int i = 0; i < listOfRequirements.Requirements_Dynamic_List.Count; i++)
            {
                if (listOfRequirements.Requirements_Dynamic_List[i].id.Equals(id))
                    return i;
            }
            return -1;
        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
            /*if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "providescoverage")
            {
                DataGridViewColumn column = dataGridView1.Columns["needscoverage"];
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    string value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    int id = SearchReq(value);

                    if (id != -1)
                    {
                        dataGridView1.Rows[id].Cells[column.Index].Value = listOfRequirements.Requirements_Dynamic_List[e.RowIndex].id;
                    }
                    /*else
                    {
                        dataGridView1.Rows[id].Cells[column.Index].Value = "";
                    }*/
                //}
                /*else
                {
                    if (listOfRequirements.Requirements_Dynamic_List[e.RowIndex].providescoverage != "")
                        dataGridView1.Rows[id].Cells[column.Index].Value = "";
                }*/
            //}

            /*if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "needscoverage")
            {
                DataGridViewColumn column = dataGridView1.Columns["providescoverage"];
                string value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                int id = SearchReq(value);
                
                if (id != -1)
                {    
                    dataGridView1.Rows[id].Cells[column.Index].Value = listOfRequirements.Requirements_Dynamic_List[e.RowIndex].id;
                }
                /*else
                {
                    dataGridView1.Rows[id].Cells[column.Index].Value = "";
                }*/
            //}
          

            UpdateDT();
        }
		private void DataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dataGridView1.HitTest(e.X, e.Y);
                dataGridView1.ClearSelection();
                dataGridView1.Rows[hti.RowIndex].Selected = true;
            }
            UpdateDT();
        }

        private void DuplicateRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (XmlFilePath == null)
            {
                MessageBox.Show(NoFileOpen);
                return;
            }
            else
            {
                /* make sure that AllowUserToAddRows is set to 'true' */
                dataGridView1.AllowUserToAddRows = true;

                /* get the selected row */
                int selected_row = (this.dataGridView1.SelectedRows[0].Index);

                /* INSERT AFTER: add a new element to Requirements_Dynamic_List "database" */
                RequirementItem reqitem = listOfRequirements.Requirements_Dynamic_List[selected_row];
                listOfRequirements.Requirements_Dynamic_List.Insert(selected_row + 1, reqitem);

                /* refresh the dataGridView */
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = listOfRequirements.Requirements_Dynamic_List;
            }
            UpdateDT();
        }

        private void DataGridView1_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dataGridView1.HitTest(e.X, e.Y);
                dataGridView1.ClearSelection();
                dataGridView1.Rows[hti.RowIndex].Selected = true;
            }
            UpdateDT();
        }
    }

    /* *******************/
    /* CLASS DEFINITIONS */
    /* *******************/
    class MyFile
    {
        /* path for the file in where is searched the "pattern" (requirement_id) */
        public string FileName { get; set; }
    }

    [Serializable()]
    /* class used to extract the setting */
    public class doc_settings
    {
        [System.Xml.Serialization.XmlElement("Baseline")]
        public string Baseline { get; set; }

        [XmlArray("Columns")]
        [XmlArrayItem(ElementName = "column")]
        public List<string> columns { get; set; }
    }

    public class CustomValues
    {
        [XmlArray("Chapters")]
        [XmlArrayItem(ElementName = "chapter")]
        public List<string> chapters { get; set; }

        [XmlArray("HWPlatform")]
        [XmlArrayItem(ElementName = "value")]
        public List<string> hwPlatforms { get; set; }

        [System.Xml.Serialization.XmlElement("chapter")]
        public string chapter { get; set; }

        public CustomValues()
        {
            chapters = new List<string>();
            hwPlatforms = new List<string>();
        }
    }
    public class NewColumn
    {
        [System.Xml.Serialization.XmlElement("name")]
        public string name { get; set; }
        [System.Xml.Serialization.XmlElement("value")]
        public string value { get; set; }

        /*public override string ToString()
        {
            return value;
        }*/

    }
    

    [Serializable()]
    [System.Xml.Serialization.XmlRoot("Requirements")]
    public class RequirementItem
    {
        [System.Xml.Serialization.XmlElement("id")]
        public string id { get; set; }

        [System.Xml.Serialization.XmlElement("description")]
        public string description { get; set; }

        [System.Xml.Serialization.XmlElement("status")]
        public string status { get; set; }

        [System.Xml.Serialization.XmlElement("CreatedBy")]
        public string CreatedBy { get; set; }

        [System.Xml.Serialization.XmlElement("needscoverage")]
        public string needscoverage { get; set; }

        [System.Xml.Serialization.XmlElement("providescoverage")]
        public string providescoverage { get; set; }

        [System.Xml.Serialization.XmlElement("version")]
        public string version { get; set; }

        [System.Xml.Serialization.XmlElement("SafetyRelevant")]
        public string SafetyRelevant { get; set; }

        [System.Xml.Serialization.XmlElement("ChangeRequest")]
        public string ChangeRequest { get; set; }

        [System.Xml.Serialization.XmlElement("ReviewID")]
        public string ReviewID { get; set; }

        [System.Xml.Serialization.XmlElement("RequirementType")]
        public string RequirementType { get; set; }

        [System.Xml.Serialization.XmlElement("Chapter")]
        public string Chapter { get; set; }

        [System.Xml.Serialization.XmlElement("HWPlatform")]
        public string HWPlatform { get; set; }

        [System.Xml.Serialization.XmlElement("Domain")]
        public string Domain { get; set; }

        [System.Xml.Serialization.XmlElement("TestedAt")]
        public string TestedAt { get; set; }


        [System.Xml.Serialization.XmlElement("ReqBaseline")]
        public string ReqBaseline { get; set; }

        [XmlArray("NewColumns")]
        [XmlArrayItem(ElementName = "column")]
        public List<NewColumn> newColumns { get; set; }
    }


    public class root_file
    {

        [XmlArray("document_settings")]
        public List<doc_settings> list_of_settings { get; set; }

        [System.Xml.Serialization.XmlElement("custom_values")]
        public CustomValues customValues { get; set; }
        [XmlArray("Requirements")]
        public List<RequirementItem> Requirements_Dynamic_List { get; set; }


        public root_file()
        {
            list_of_settings = new List<doc_settings>();
            Requirements_Dynamic_List = new List<RequirementItem>();
            customValues = new CustomValues();
        }
        public void SaveAs(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                var XML = new XmlSerializer(typeof(root_file));
                XML.Serialize(stream, this);
            }
        }
        public void Save(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                var XML = new XmlSerializer(typeof(root_file));
                XML.Serialize(stream, this);

            }
        }
        public object ShallowCopy()
        {
            return (root_file)this.MemberwiseClone();
        }
    }

}