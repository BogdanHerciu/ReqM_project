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


namespace ReqM_Tool
{


    public partial class Main : Form
    { 
        /* create variable to the root of the xml file, for reading the requirements */
        root_file listOfRequirements = new root_file();
        /* create variable to the root of the xml file, for reading the settings */
        //root_settings listOfSettings = null;

        /* define columns from DataGridView */
        public int Column_ID = 0;
        public int Column_Description = 1;
        public int Column_Status = 2;
        public int Column_CreatedBy = 3;
        public int Column_Priority = 4;
        public int Column_needscoverage = 5;
        public int Column_providescoverage = 6;

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
        }

        /* Method called when a cell is modified */
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            /* collor change for needscoverage column */
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

        private void ImplementationPathButton_Click(object sender, EventArgs e)
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

                    /**** load the settings ****/
                    /* create a serializer for the settings list */
                    //XmlSerializer settings_serializer = new XmlSerializer(typeof(root_settings));
                    /* read the data from the xml file */
                    //StreamReader reader_settings = new StreamReader(XmlFilePath);
                    /* dezerialize the data */
                    //listOfSettings = (root_settings)settings_serializer.Deserialize(reader_settings);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (XmlFilePath == null)
            {
                MessageBox.Show("No file has been opened!");
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
                MessageBox.Show("No file has been opened!");
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
                MessageBox.Show("No file has been opened!");
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
                        Chapter = "first element from the list",
                        HWPlatform = "first elem from list",
                        Domain = "N/A",
                        TestedAt = "N/A",
                        
                        
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
                MessageBox.Show("No file has been opened!");
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
            DataSet ds = new DataSet();

            //Convert the XML into Dataset
            ds.ReadXml(XmlFilePath);

            //Retrieve the table fron Dataset
            DataTable dt = ds.Tables[1];

            // Create an Excel object
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            //Create workbook object


            Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Open(Filename : XmlFilePath);

            //Create worksheet object
            Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.ActiveSheet;

            // Column Headings
            int iColumn = 0;

            foreach (DataColumn c in dt.Columns)
            {
                iColumn++;
                excel.Cells[1, iColumn] = c.ColumnName;
            }

            // Row Data
            int iRow = worksheet.UsedRange.Rows.Count - 1;

            foreach (DataRow dr in dt.Rows)
            {
                iRow++;

                // Row's Cell Data
                iColumn = 0;
                foreach (DataColumn c in dt.Columns)
                {
                    iColumn++;
                    excel.Cells[iRow + 1, iColumn] = dr[c.ColumnName];
                }
            }

                    ((Microsoft.Office.Interop.Excel._Worksheet)worksheet).Activate();

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
        
    }

    /*public class Chapter
    {
        [System.Xml.Serialization.XmlElement("chapter")]
        public string chapter { get; set; }
    }*/

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

        
    }

    /*[Serializable()]
    [System.Xml.Serialization.XmlRoot("root_file")]
    public class root_settings
    {

        [XmlArray("document_settings")]
        public List<doc_settings> list_of_settings = new List<doc_settings>();

        public void Save(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                var XML = new XmlSerializer(typeof(root_settings));
                XML.Serialize(stream, this);

            }
        }
        //public List<document_settings> Settings_List = new List<document_settings>();
    }

    /*[Serializable()]
    [System.Xml.Serialization.XmlRoot("root_file")]
    public class root_file
    {
        [XmlArray("Requirements")]
       
        public List<RequirementItem> Requirements_Dynamic_List = new List<RequirementItem>();
        public void SaveAs(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create ))
            {
                var XML = new XmlSerializer(typeof(root_file));
                XML.Serialize(stream, this);
            }
        }
        public void Save(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Append))
            {
                var XML = new XmlSerializer(typeof(root_file));
                XML.Serialize(stream, this);

            }
        }
    }*/

    public class root_file
    {
        
        [XmlArray("document_settings")]
        public List<doc_settings> list_of_settings { get; set; }

        [XmlArray("Requirements")]
        public List<RequirementItem> Requirements_Dynamic_List { get; set; }

        /*[XmlArray("Chapters")]
        public List<string> chapters { get; set; }*/

        public root_file()
        {
            list_of_settings = new List<doc_settings>();
            Requirements_Dynamic_List = new List<RequirementItem>();
            //chapters = new List<string>();
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
