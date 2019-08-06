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
        /* If file is saved => true */
        public bool saved = true;

        /* "No File has been open" Text Box. */
        public string NoFileOpen { get; } = "No file has been opened!";
        public string invalidColumns { get; } = "There are invalid columns";

        /* FilterForm checkBoxes */
        public bool cbox1;
        /* lists for Chapter, HWPlatform*/
        List<string> list1 = new List<string>();
        List<string> list2 = new List<string>();

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

            this.Size = new Size(1400, 800);

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

        private void AddColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
                    if (listOfRequirements.Requirements_Dynamic_List[e.RowIndex].ReqBaseline != listOfRequirements.list_of_settings[0].Baseline)
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Yellow;
                    else
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                }
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "Chapter")
            {
                /* Get id of current RequirementItem */
                string id = dataGridView1.Rows[e.RowIndex].Cells[dataGridView1.Columns["id"].Index].Value.ToString();
                /* Get current RequirementItem from list */
                RequirementItem item = FindReq(id, listOfRequirements);
                
                if (item != null)
                {
                    /* Get the node from treeView */
                    TreeNode node = FindNode(item.id);

                    /* Remove the node if it exists */
                    if (node != null)
                    {
                        treeView1.Nodes[node.Parent.Text].Nodes[node.Text].Remove();
                    }

                    /* Only add the node if the chapter is set */
                    if (item.Chapter != "N/A")
                    {
                        AddTreeNode(item.id, item.Chapter);
                    }
                   
                }

            }
            UpdateDT();
            saved = false;
        }

        public void UpdateDT()
        {
            /* Clears DataTable */
            /*dt.Columns.Clear();
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
            }*/

            if ( searchBox.Text == srchBoxText)
            {
                dt = new DataTable();
                //Adding the Columns.
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    dt.Columns.Add(column.HeaderText);
                }

                //Adding the Rows.
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    dt.Rows.Add();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        dt.Rows[dt.Rows.Count - 1][cell.ColumnIndex] = cell.Value.ToString();
                    }
                }
            } 
        }

        private void Main_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Chapters");
            comboBox1.Items.Add("HWPlatform");
            comboBox1.SelectedIndex = 0;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            /* for providescoverage column */
            if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "providescoverage")
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "N/A")
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
                            if (file.Equals(XmlFilePath))
                            {
                                int index = FindReqIndex(value, listOfRequirements);
                                if (index > -1)
                                {
                                    dataGridView1.FirstDisplayedScrollingRowIndex = index;
                                    return;
                                }
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
                                    root_file listOfRequirementsNew = (root_file)serializer.Deserialize(reader);
                                    reader.Close();

                                    /* find index of requirement */
                                    int index = FindReqIndex(value, listOfRequirementsNew);
                                    /* if found */
                                    if (index > -1)
                                    {
                                        if (!saved)
                                        {
                                            DialogResult result = MessageBox.Show("Opening another file. Do you want to save changes?", "Close file", MessageBoxButtons.YesNoCancel);
                                            if (result == DialogResult.Yes)
                                            {
                                                if (CheckTBD())
                                                {
                                                    Save();

                                                    listOfRequirements = listOfRequirementsNew;
                                                    dataGridView1.DataSource = listOfRequirements.Requirements_Dynamic_List;

                                                    XmlFilePath = file;
                                                    saved = true;
                                                    DisplayDefaultColumns();
                                                    dataGridView1.Refresh();

                                                    /* Update Treeview */
                                                    ClearTreeView();
                                                    GenerateTreeView();

                                                    dataGridView1.FirstDisplayedScrollingRowIndex = index;
                                                    UpdateDT();
                                                    CheckBaseline();
                                                    CheckNeedscoverage();
                                                }
                                                else
                                                {
                                                    MessageBox.Show(invalidColumns);
                                                }
      
                                            }
                                            else if (result == DialogResult.No)
                                            {
                                                dataGridView1.DataSource = listOfRequirementsNew.Requirements_Dynamic_List;
                                                listOfRequirements = listOfRequirementsNew;
                                                XmlFilePath = file;
                                                saved = true;
                                                DisplayDefaultColumns();
                                                dataGridView1.Refresh();

                                                /* Update Treeview */
                                                ClearTreeView();
                                                GenerateTreeView();

                                                dataGridView1.FirstDisplayedScrollingRowIndex = index;
                                                UpdateDT();
                                                CheckBaseline();
                                                CheckNeedscoverage();
                                            }
                                            return;
                                        }
                                        else
                                        {
                                            DialogResult result = MessageBox.Show("Close file?", "Close file", MessageBoxButtons.YesNo);
                                            if (result == DialogResult.Yes)
                                            {
                                                dataGridView1.DataSource = listOfRequirementsNew.Requirements_Dynamic_List;
                                                listOfRequirements = listOfRequirementsNew;
                                                XmlFilePath = file;
                                                saved = true;
                                                DisplayDefaultColumns();
                                                dataGridView1.Refresh();

                                                dataGridView1.FirstDisplayedScrollingRowIndex = index;
                                                UpdateDT();
                                                CheckBaseline();
                                                CheckNeedscoverage();
                                            }
                                            return;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                }
                            }
                        }
                    }
                    MessageBox.Show("Requirement could not be found");
                      
                }
            }
        }

        public void CreateDataGridView()
        {
            DataGridViewTextBoxColumn textBoxColumn = new DataGridViewTextBoxColumn();
            textBoxColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textBoxColumn.DataPropertyName = "id";
            textBoxColumn.Name = "id";
            textBoxColumn.HeaderText = "id";
            textBoxColumn.ReadOnly = true;
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

        /*
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
        }*/

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

        public void CloseFile()
        {
            dataGridView1.DataSource = null;
            XmlFilePath = null;
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
            ClearTreeView();
            saved = true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                /* Clear DataGridView if other xml is opened */
                if (!saved)
                {
                    DialogResult result = MessageBox.Show("Do you want to save your changes?", "Open file", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        if (CheckTBD())
                        {
                            Save();
                            CloseFile();
                        }
                        else
                        {
                            MessageBox.Show(invalidColumns);
                            return;
                        }
                    }
                    else if (result == DialogResult.No)
                    {
                        CloseFile();
                    }
                    else
                    {
                        return;
                    }
                }

                else
                {
                    CloseFile();
                }
                

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
                    dataGridView1.DataSource = listOfRequirements.Requirements_Dynamic_List;

                    /* Clears DataTable */
                    dt.Columns.Clear();
                    dt.Rows.Clear();

                    /* Clears TreeView */
                    ClearTreeView();

                    /* default columns to display */
                    DisplayDefaultColumns();

                    /* generate TreeView */
                    GenerateTreeView();

                    /* add event for Cell value changed */
                    dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
                    dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;

                    /* color with red needscoverage column if there's an error! */
                    CheckNeedscoverage();  
                    CheckBaseline();
                    UpdateDT();
                    
                    
                    PopulateList();
                    listBox1.DataSource = list1;

                    /* Chapter chart */
                    foreach (var series in chart2.Series)
                        series.Points.Clear();
                    int index = 0;
                    int[] chapters = new int[listOfRequirements.customValues.chapters.Count];

                    foreach (var c in listOfRequirements.customValues.chapters)
                    {
                        foreach (var v in listOfRequirements.Requirements_Dynamic_List)
                        {
                            if (v.Chapter.ToString() == c.ToString())
                                chapters[index]++;
                        }
                        chart2.Series["Chapters"].Points.AddXY(c.ToString(), chapters[index]);
                        index++;
                    }
                    /* Status chart */
                    foreach (var series in chart3.Series)
                        series.Points.Clear();
                    int[] stsTable = new int[status.Count];
                    for (int i = 0; i < status.Count; i++)
                    {
                        foreach (var v in listOfRequirements.Requirements_Dynamic_List)
                        {
                            if (v.status.ToString() == status[i])
                            {
                                stsTable[i]++;
                            }
                        }
                        chart3.Series["Status"].Points.AddXY(status[i].ToString(), stsTable[i]);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

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

        /* Check if cells have valid values
         * Returns false - if any cell has invalid value */
        public bool CheckTBD()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    if (dataGridView1.Rows[row.Index].Cells[column.Index].Style.BackColor == Color.Red)
                        return false;
                }
            }
            return true;
        }

        public void Save()
        {
            try
            {
                /* Save the file to the XmlFilePath (the path from where was opened) */
                listOfRequirements.list_of_settings.ElementAt(0).columns.Clear();

                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    if (c.Visible)
                    {
                        listOfRequirements.list_of_settings.ElementAt(0).columns.Add(c.Name);
                    }
                }
                listOfRequirements.Save(XmlFilePath);
                saved = true;   
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (XmlFilePath == null)
            {
                MessageBox.Show(NoFileOpen);
                return;
            }
            if (CheckTBD())
            {
                Save();
            }
            else
            {
                MessageBox.Show(invalidColumns);
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
                if (CheckTBD())
                {
                    Save();
                }
                else
                {
                    MessageBox.Show(invalidColumns);
                }
            }
        }
		        private void InsertReq(int index)
        {
            string newID = GenerateID();
            if (newID != null)
            {
                listOfRequirements.Requirements_Dynamic_List.Insert(index, new RequirementItem()
                {
                    id = newID,                                /* default text for the id */
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
                    ReqBaseline = listOfRequirements.list_of_settings[0].Baseline,

                });
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

                if (listOfRequirements.Requirements_Dynamic_List.Count > 0)
                {
                    if (this.dataGridView1.SelectedRows.Count > 0)
                    {
                        /* get the selected row */
                        int selected_row = (this.dataGridView1.SelectedRows[0].Index);


                        /* INSERT AFTER: add a new element to Requirements_Dynamic_List "database" */
                        InsertReq(selected_row + 1);

                        /* refresh the dataGridView */
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = listOfRequirements.Requirements_Dynamic_List;

                        /* add req to TreeView */
                        RequirementItem item = listOfRequirements.Requirements_Dynamic_List[selected_row + 1];
                        AddTreeNode(item.id, item.Chapter);

                        saved = false;
                        CheckNeedscoverage();
                        CheckBaseline();
                        UpdateDT();
                    }
                    else
                    {
                        MessageBox.Show("No row is selected for adding a new item");
                    }
                }
                else
                {
                    InsertReq(0);

                    if (listOfRequirements.Requirements_Dynamic_List.Count > 0)
                    {
                        /* refresh the dataGridView */
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = listOfRequirements.Requirements_Dynamic_List;

                        /* add req to TreeView */
                        RequirementItem item = listOfRequirements.Requirements_Dynamic_List[0];
                        AddTreeNode(item.id, item.Chapter);

                        saved = false;
                        CheckNeedscoverage();
                        CheckBaseline();
                        UpdateDT();
                    }        
                }           
            }
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

                    string id = dataGridView1.Rows[selected_row].Cells[dataGridView1.Columns["id"].Index].Value.ToString();

                    RequirementItem item = FindReq(id, listOfRequirements);

                    /* Delete req from TreeView */
                    if(FindNode(id) != null)
                        DeleteTreeNode(item.id, item.Chapter);

                    /* If id number is lastID, set lastID - 1*/
                    if (item.id.Substring(item.id.LastIndexOf('_')+1).Equals(listOfRequirements.list_of_settings[0].lastID))
                        listOfRequirements.list_of_settings[0].lastID = (Convert.ToInt32(listOfRequirements.list_of_settings[0].lastID) - 1).ToString();

                    /* remove the row from the List */
                    listOfRequirements.Requirements_Dynamic_List.Remove(item);

                    /* refresh the dataGridView */
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = listOfRequirements.Requirements_Dynamic_List;

                    saved = false;
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
                pattern = listOfRequirements.Requirements_Dynamic_List[index].id + "," + "\\d\\.\\d";
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
                            if (version == matches.Value.Substring(matches.Value.LastIndexOf(",") + 1, 3))
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

            if (XmlFilePath != null)
            {
                if (!saved)
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to save changes?", "Close Document", MessageBoxButtons.YesNoCancel);

                    if (dialogResult == DialogResult.Yes)
                    {
                        if (CheckTBD())
                        {
                            Save();
                            CloseFile();
                            /* Reset combox value */
                            list1.Clear(); list2.Clear();
                            listBox1.DataSource = null;
                            listBox1.Items.Add("XML file not open!");
                        }

                        else
                        {
                            MessageBox.Show(invalidColumns);
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        CloseFile();
                        /* Reset combox value */
                        list1.Clear(); list2.Clear();
                        listBox1.DataSource = null;
                        listBox1.Items.Add("XML file not open!");
                    }


                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure?", "Close Document", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        CloseFile();
                        /* Reset combox value */
                        list1.Clear(); list2.Clear();
                        listBox1.DataSource = null;
                        listBox1.Items.Add("XML file not open!");
                    }
                }
            }

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
                    saved = false;
                }
            }

        }

        /*private void AddColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if a XML file is open. 
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

        }*/
		public string srchBoxText = " Search";

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            /* LINKSTO: Req050 */
           
            if (XmlFilePath != null)
            {
                if (searchBox.Text != srchBoxText)
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
        if (searchBox.Text == srchBoxText)
            searchBox.Text = "";
    }

        private void SearchBox_Leave(object sender, EventArgs e)
        {
            if (searchBox.Text == "")
            {
                searchBox.Text = srchBoxText;
                dataGridView1.DataSource = listOfRequirements.Requirements_Dynamic_List;
            }
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

        public int FindReqIndex(string id, root_file list)
        {
            for (int i = 0; i < list.Requirements_Dynamic_List.Count; i++)
            {
                if (list.Requirements_Dynamic_List[i].id.Equals(id))
                    return i;
            }
            return -1;
        }

        public RequirementItem FindReq(string id, root_file list)
        {
            foreach (RequirementItem item in list.Requirements_Dynamic_List)
            {
                if (item.id.Equals(id))
                    return item;
            }
            return null;
        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
           
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

                if (dataGridView1.SelectedRows.Count > 0)
                {
                    /* get the selected row */
                    int selected_row = (this.dataGridView1.SelectedRows[0].Index);

                    /* INSERT AFTER: add a new element to Requirements_Dynamic_List "database" */

                    RequirementItem item = listOfRequirements.Requirements_Dynamic_List[selected_row];
                    RequirementItem newItem = new RequirementItem()
                    {
                        id = GenerateID(),
                        description = item.description,
                        status = item.status,
                        CreatedBy = item.CreatedBy,
                        needscoverage = item.needscoverage,
                        providescoverage = item.providescoverage,
                        version = item.version,
                        SafetyRelevant = item.SafetyRelevant,
                        ChangeRequest = item.ChangeRequest,
                        ReviewID = item.ReviewID,
                        RequirementType = item.RequirementType,
                        Chapter = item.Chapter,
                        HWPlatform = item.HWPlatform,
                        Domain = item.Domain,
                        TestedAt = item.TestedAt,
                        ReqBaseline = item.ReqBaseline

                    };
                    listOfRequirements.Requirements_Dynamic_List.Insert(selected_row + 1, newItem);

                    /* refresh the dataGridView */
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = listOfRequirements.Requirements_Dynamic_List;

                    /* add req to TreeView */
                    AddTreeNode(newItem.id, newItem.Chapter);
                }
                else
                {
                    MessageBox.Show("No row selected");
                }
                
            }
            CheckNeedscoverage();
            CheckBaseline();
            UpdateDT();
        }

        private void DataGridView1_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && dataGridView1.SelectedRows != null)
            {
                var hti = dataGridView1.HitTest(e.X, e.Y);
                dataGridView1.ClearSelection();
                if (hti.RowIndex > -1)
                    dataGridView1.Rows[hti.RowIndex].Selected = true;
                UpdateDT();
            }
           
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if file is open
            if (XmlFilePath != null && saved == false)
            {
                // Display a MsgBox asking the user to save changes or abort
                DialogResult result = MessageBox.Show("Do you want to save changes to your file?", "Close", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    // Call method to save file...
                    if (CheckTBD())
                    {
                        Save();
                    }
                    else
                    {
                        MessageBox.Show(invalidColumns);
                        e.Cancel = true;
                    }
                        
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
		
		private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (XmlFilePath == null)
                listBox1.Items.Add("XML file not open!");
            else
            {
                if (comboBox1.SelectedItem.ToString() == "Chapters")
                {
                    listBox1.DataSource = list1;
                }
                if (comboBox1.SelectedItem.ToString() == "HWPlatform")
                {
                    listBox1.DataSource = list2;
                }
            }            
        }
		
		public void PopulateList()
        {
            for (int i = 0; i < listOfRequirements.customValues.chapters.Count; i++)
            {
                list1.Add(listOfRequirements.customValues.chapters.ElementAt(i).ToString());
            }
            for (int i = 0; i < listOfRequirements.customValues.hwPlatforms.Count; i++)
            {
                list2.Add(listOfRequirements.customValues.hwPlatforms.ElementAt(i).ToString());
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            /* DELETE button */
            if (listBox1.SelectedIndex >= 0)
            {
                if (listBox1.Items.Count > 0)
                {
                    if (comboBox1.SelectedIndex == 0)
                    {
                        list1.RemoveAt(listBox1.SelectedIndex);
                        listBox1.DataSource = null;
                        listBox1.DataSource = list1;
                    }
                    if (comboBox1.SelectedIndex == 1)
                    {
                        list2.RemoveAt(listBox1.SelectedIndex);
                        listBox1.DataSource = null;
                        listBox1.DataSource = list2;
                    }
                }
            }
        }

        private void Button3_Click_2(object sender, EventArgs e)
        {
            /* ADD button */
            bool exists = false;
            foreach (var v in listBox1.Items)
            {
                if (textBox3.Text == v.ToString())
                    exists = true;
            }
            
            if (listBox1.SelectedIndex >= 0)
            {
                if (textBox3.Text != String.Empty && exists == false)
                {
                    if (comboBox1.SelectedIndex == 0)
                    {
                        list1.Insert(listBox1.SelectedIndex + 1, textBox3.Text);
                        listBox1.DataSource = null;
                        listBox1.DataSource = list1;;
                    }
                    if (comboBox1.SelectedIndex == 1 && exists == false)
                    {
                        list2.Insert(listBox1.SelectedIndex + 1, textBox3.Text);
                        listBox1.DataSource = null;
                        listBox1.DataSource = list2;
                    }
                }
                else if (exists == true)
                    MessageBox.Show("Element already exists");
                textBox3.Text = null;
            }
            else
            {               
                if (textBox3.Text != String.Empty)
                {
                    if (listBox1.Items.Count != 0)
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    if (comboBox1.SelectedIndex == 0 && exists == false)
                    {
                        list1.Insert(listBox1.SelectedIndex + 1, textBox3.Text);
                        listBox1.DataSource = null;
                        listBox1.DataSource = list1;
                    }
                    if (comboBox1.SelectedIndex == 1 && exists == false)
                    {
                        list2.Insert(listBox1.SelectedIndex + 1, textBox3.Text);
                        listBox1.DataSource = null;
                        listBox1.DataSource = list2;
                    }
                }
                else if (exists == true)
                    MessageBox.Show("Element already exists");
                textBox3.Text = null;
            }
        }

        public void GenerateTreeView()
        {
            foreach (string chapter in listOfRequirements.customValues.chapters)
            {
                if (chapter != "N/A")
                    treeView1.Nodes.Add(chapter, chapter);
            }

            foreach (RequirementItem item in listOfRequirements.Requirements_Dynamic_List)
            {
                if (item.Chapter != "N/A")
                {
                    treeView1.Nodes[item.Chapter].Nodes.Add(item.id, item.id);
                }
            }
        }

        private void TreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Parent != null)
            {
                dataGridView1.DataSource = listOfRequirements.Requirements_Dynamic_List;
                CheckBaseline();
                CheckNeedscoverage();

                int index = FindReqIndex(e.Node.Text, listOfRequirements);
                if (index > -1)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = index;
                }
            }
        }

        public void DeleteTreeNode(string id, string chapter)
        {
            treeView1.Nodes[chapter].Nodes[id].Remove();
        }

        public void AddTreeNode(string id, string chapter)
        {
            treeView1.Nodes[chapter].Nodes.Add(id, id);
        }

        public void ClearTreeView()
        {
            treeView1.Nodes.Clear();
            treeView1.Nodes.Clear();
        }

        public TreeNode FindNode(string id)
        {
            foreach(TreeNode node in treeView1.Nodes)
            {
                if (node.Nodes.ContainsKey(id))
                    return node.Nodes[id];
            }
            return null;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            /* SAVE button */
            XmlDocument doc = new XmlDocument();
            doc.Load(XmlFilePath);

            /* Clear all Columns child nodes */
            XmlNode chapterNode = doc.SelectSingleNode("root_file/custom_values/Chapters");
            chapterNode.RemoveAll();

            XmlNode hwpNode = doc.SelectSingleNode("root_file/custom_values/HWPlatform");
            hwpNode.RemoveAll();

            XmlElement currentNode1 = (XmlElement)doc.SelectSingleNode("root_file/custom_values/Chapters");
            XmlElement currentNode2 = (XmlElement)doc.SelectSingleNode("root_file/custom_values/HWPlatform");

            listOfRequirements.customValues.chapters.Clear();
            listOfRequirements.customValues.hwPlatforms.Clear();

            foreach (string elem in list1)
            {
                XmlElement elm = doc.CreateElement("chapter");
                elm.InnerText = elem;
                currentNode1.AppendChild(elm);
                listOfRequirements.customValues.chapters.Add(elem);
            }
            foreach (string elem in list2)
            {
                XmlElement elm = doc.CreateElement("value");
                elm.InnerText = elem;
                currentNode2.AppendChild(elm);
                listOfRequirements.customValues.hwPlatforms.Add(elem);
            }
            doc.Save(XmlFilePath);
            dataGridView1.Refresh(); // todo
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            if (XmlFilePath != null)
            {
                if ((Application.OpenForms["HeaderForm"] as Form) == null)
                {
                    HeaderForm HeaderForm = new HeaderForm(this);
                    HeaderForm.Show();
                }
            }
            else
                MessageBox.Show(NoFileOpen);
        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        public string imgUrl;
        public string imgFolderPath;
        private void InsertImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imgFolderPath = getImagePath();

            int index = dataGridView1.CurrentCell.ColumnIndex;
            if (dataGridView1.Columns[index].HeaderText == "description")
            {
                OpenFileDialog open = new OpenFileDialog();
                open.InitialDirectory = imgFolderPath;
                open.Filter = "Image Files(*.jpg; .jpeg; .gif; .bmp, .png)|*.jpg; .jpeg; .gif; .bmp; .png";

                if (open.ShowDialog() == DialogResult.OK)
                {
                    // display image in picture box  
                    //Image img = new Bitmap(open.FileName);
                    ///dataGridView1.
                    imgUrl = open.FileName;
                    string text = dataGridView1.CurrentCell.Value.ToString();
                    string output = imgUrl.Substring(imgUrl.LastIndexOf('\\') + 1);
                    text += " " + output;
                    dataGridView1.CurrentCell.Value = text;
                }
            }
            else
            {
                MessageBox.Show("Select Description Cell");
            }
        }
        public string getImagePath()
        {
            string imgFolderPath = XmlFilePath;
            for (int i = 0; i < 2; ++i)
            {
                imgFolderPath = imgFolderPath.Substring(0, imgFolderPath.LastIndexOf("\\"));
            }
            imgFolderPath += "\\img";
            return imgFolderPath;
        }

        public string DGVText { get; set; }
        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "description")
            {
                string imageurl = dataGridView1.CurrentRow.Cells[dataGridView1.Columns["description"].Index].Value.ToString();
                DGVText = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                ImageForm myForm = new ImageForm(this);
                myForm.ShowDialog();
            }
        }

        private void InsertImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imgFolderPath = getImagePath();

            int index = dataGridView1.CurrentCell.ColumnIndex;
            if (dataGridView1.Columns[index].HeaderText == "description")
            {
                OpenFileDialog open = new OpenFileDialog();
                open.InitialDirectory = imgFolderPath;
                open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp, *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";

                if (open.ShowDialog() == DialogResult.OK)
                {
                    // display image in picture box  
                    //Image img = new Bitmap(open.FileName);
                    ///dataGridView1.
                    imgUrl = open.FileName;
                    string text = dataGridView1.CurrentCell.Value.ToString();
                    string output = imgUrl.Substring(imgUrl.LastIndexOf('\\') + 1);
                    text += " " + output;
                    dataGridView1.CurrentCell.Value = text;
                }
            }
            else
            {
                MessageBox.Show("Select Description Cell");
            }
        }
		
		public string GenerateID()
        {
            /* Generate the first id when there are no requirements */
            if (listOfRequirements.Requirements_Dynamic_List.Count == 0)
            {
                //generate first id??
                return GenerateFirstID();
            }
            else
            {
                string id = listOfRequirements.Requirements_Dynamic_List[0].id;
                id = id.Substring(0, id.LastIndexOf('_') + 1);
                int lastID = Convert.ToInt32(listOfRequirements.list_of_settings[0].lastID) + 1;
                listOfRequirements.list_of_settings[0].lastID = lastID.ToString();
                id += lastID.ToString();
                return id;
            }
        }
		
		public string GenerateFirstID()
        {
            if (listOfRequirements.list_of_settings[0].reqType != null)
            {
                string id = "";
                string type = listOfRequirements.list_of_settings[0].reqType;
                if (type.Equals("SW"))
                {
                    id = "SWRS_0";
                    listOfRequirements.list_of_settings[0].lastID = "0";
                }
                else if (type.Equals("SYS"))
                {
                    id = "SYSRS_0";
                    listOfRequirements.list_of_settings[0].lastID = "0";
                }
                return id;
            }
            else
            {
                MessageBox.Show("Please set ReqType in XML");
                return null;
            }
            
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
        [System.Xml.Serialization.XmlElement("DocumentIntro")]
        public string header { get; set; }
        [System.Xml.Serialization.XmlElement("ReqType")]
        public string reqType { get; set; }
        [System.Xml.Serialization.XmlElement("LastID")]
        public string lastID { get; set; }
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

        /*[XmlArray("NewColumns")]
        [XmlArrayItem(ElementName = "column")]
        public List<NewColumn> newColumns { get; set; }*/
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

        public void Save(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                var XML = new XmlSerializer(typeof(root_file));
                XML.Serialize(stream, this);

            }
        }

    }

}