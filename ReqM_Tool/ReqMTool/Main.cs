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
using System.Collections.Generic;
using function_namespace;
using ReqM_namespace;

namespace ReqM_Tool
{


    public partial class Main : Form
    { 
        /* create variable to the root of the xml file, for reading the requirements */
        root_file listOfRequirements = new root_file();

        /* create variable to the root of the xml file, for reading the settings */
        bool statistics = false;

        /* define lists of possible values */
        public List<string> status { get; } 
        public List<string> safetyRelevant { get; }
        public List<string> domain { get; }
        public List<string> tested { get; }
        public List<string> type { get; }

        /* define columns from DataGridView */
        public int Column_ID = 0;
        public int Column_Description = 1;
        public int Column_Status = 2;
        public int Column_CreatedBy = 3;
        public int Column_needscoverage = 5;
        public int Column_providescoverage = 6;
        public int Column_version=7;
        public int Column_SafetyRelevant=8;
        public int Column_ChangeRequest = 9;
        public int Column_ReviewID = 10;
        public int Column_RequirementType = 11;
        public int Column_Chapter = 12;
        public int Column_HWPlatform = 13;
        public int Column_Domain = 14;
        public int Column_TestedAt = 15;
        public int Column_ReqBaseline = 16;
        public int MAX_Columns = 17;

        /* "No File has been open" Text Box. */
        public string NoFileOpen = "No file has been opened!";

        /* FilterForm checkBoxes */
        public bool cbox1;

        /* all the files from a folder */
        List<MyFile> listOfFiles;
        OpenFileDialog FileDialog = new OpenFileDialog();
        /* the path of the xml file */
        string XmlFilePath;
        /* path in where we search for the Requirements that needs to cover the Implementation */
        string implementationFilePath;
        /* path in where we search for the Requirements that needs to cover the Tests */
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
            foreach (DataGridViewColumn col in this.dataGridView1.Columns)
            {
                dataGridView1.Rows[e.RowIndex].Cells[col.Index].Style.BackColor = Color.Yellow;
            }
            /* Increment the Requirement Baseline with 0.1.  */
            double DocumentBaseline = Convert.ToDouble(listOfRequirements.list_of_settings[0].Baseline);
            listOfRequirements.Requirements_Dynamic_List[e.RowIndex].ReqBaseline = Convert.ToString(DocumentBaseline + 0.1);



            /* Color change for needscoverage column.
             * If the Value is neighter tst nor src, color the box in red. */
            if (e.ColumnIndex == Column_needscoverage)
            { 
                if (
               (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "tst") ||
               (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "src")
               )
                {
                    dataGridView1.Rows[e.RowIndex].Cells[Column_needscoverage].Style.BackColor = Color.White; 
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].Cells[Column_needscoverage].Style.BackColor = Color.Red;
                }
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

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                /* build the XML file path */
                XmlFilePath = FileDialog.FileName;

                string extension = Path.GetExtension(XmlFilePath);

                /* verify file format */
                if (extension != ".xml")
                {
                    MessageBox.Show("Unsupported file format. Please open supported Requirements file.");
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

                    /* add the data into the table */
                    dataGridView1.DataSource = listOfRequirements.Requirements_Dynamic_List;

                    //listOfRequirements.list_of_settings.ElementAt(0).columns = new List<string> { "a", "b" };

                    /* default columns to display */
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        dataGridView1.Columns[i].Visible = false;
                    }

                    dataGridView1.Columns["id"].Visible = true;
                    dataGridView1.Columns["description"].Visible = true;
                    dataGridView1.Columns["status"].Visible = true;
                    dataGridView1.Columns["CreatedBy"].Visible = true;
                    dataGridView1.Columns["needscoverage"].Visible = true;
                    dataGridView1.Columns["providescoverage"].Visible = true;
                    dataGridView1.Columns["version"].Visible = true; 

                    /* add event for Cell value changed */
                    dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
                    dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;

                    /* color with red needscoverage column if there's an error! */
                    for (int index = 0; index < (listOfRequirements.Requirements_Dynamic_List.Count()); index++)
                    {
                        if (
                            (listOfRequirements.Requirements_Dynamic_List[index].needscoverage.ToString() != "tst") &&
                            (listOfRequirements.Requirements_Dynamic_List[index].needscoverage.ToString() != "src")
                            )
                        {
                            dataGridView1.Rows[index].Cells[Column_needscoverage].Style.BackColor = Color.Red;
                        }
                    }

                   
                }                
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                /* Center the text for all columns except "Description" column. */
                foreach (DataGridViewRow Row in this.dataGridView1.Rows)
                {
                    foreach (DataGridViewColumn Column in this.dataGridView1.Columns)
                    {
                        if (Column.Index != Column_Description)
                        {
                            dataGridView1.Columns[Column.Index].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                    }
                }
            }

            /* LINKSTO: Req076 */
            /* Color the requirement with Yellow if it has different baseline than the document Baseline. */
            /* Color each column that is changed. */
            foreach (DataGridViewRow Row in this.dataGridView1.Rows)
            {
                if (listOfRequirements.Requirements_Dynamic_List[Row.Index].ReqBaseline != listOfRequirements.list_of_settings[0].Baseline)
                {
                    /* Color each column that is changed. */
                    foreach (DataGridViewColumn Column in this.dataGridView1.Columns)
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
                /* the name of the requirement which is searched in all files */
                pattern = listOfRequirements.Requirements_Dynamic_List[index].id;
                MyFile mf = new MyFile();
                MatchCollection matches;
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

                /* search in each file from the folder "selectable_path" */
                foreach (string file in Directory.GetFiles(selectable_path, "*.*"))
                {
                    content = File.ReadAllText(file);
                    Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                    matches = r.Matches(content);

                    /* if Requirement is found. Add into the table. */
                    if (matches.Count > 0)
                    {
                        int rowId = dataGridView2.Rows.Add();
                        /* grab the new row */
                        DataGridViewRow row = dataGridView2.Rows[rowId];

                        /* add the data */
                        /* Set Req_ID and Covered cell only first time */
                        if (requirementFound == false)
                        {
                            row.Cells[0].Value = pattern;
                            row.Cells[1].Value = "Covered";
                            /* TODO: merge cells!!! */

                        }
                        row.Cells[2].Value = Path.GetFileName(file);
                        /* set the color */
                        row.DefaultCellStyle.BackColor = Color.Green;

                        /* mark as a found requirement */
                        requirementFound = true;
                    }
                } /* end foreach */

                /* if the requirement has not been found, add requirement_id and "notCovered" text */
                if (requirementFound == false)
                {
                    myFunctions.ReqNotFound_AddRow(dataGridView2, pattern);
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

                //Create workbook object
                Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(Type.Missing);

                //Create worksheet object
                Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.ActiveSheet;

                for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                {
                    worksheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
                }
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel|*.xlsx";

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
            float coverage = 0; ;

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
            FilterForm form = new FilterForm(this);
            form.Show();   
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
                    listOfRequirements.list_of_settings[0].Baseline = Convert.ToString(DocumentBaseline);
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
                /* TODO */
                /* Your CODE. */
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
    }


}
