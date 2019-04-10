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

namespace function_namespace
{
    public class myFunctions
    {
        public static bool Paths(string sourcepath, string testpath, string xmlpath)
        {
            if (sourcepath == null)
            {
                MessageBox.Show("Please Select the Project Folder!");
                return false;
            }
            else
                if (testpath == null)
            {
                MessageBox.Show("Please Select the Testing Folder!");
                return false;
            }
            else if (xmlpath == null)
            {
                MessageBox.Show("No xml file is loaded! Please open a xml file!");
                return false;
            }
            /* everighing is fine, return true */
            return true;
        }

        public static string getPath(string sourcepath, string testpath, string ToTest)
        {
            if (ToTest == "src")
            {
                return sourcepath;
            }
            else if (ToTest == "tst")
            {
                return testpath;
            }
            else if (ToTest == "src,txt")
            {
                /* TODO */
                return null;
            }
            else
            {
                MessageBox.Show("Error! item ToTest is not valid!");
                return null;
            }
        }

        public static void ReqNotFound_AddRow(DataGridView myGrid, string Req_Id)
        {
            int rowId = myGrid.Rows.Add();
            /* grab the new row */
            DataGridViewRow row = myGrid.Rows[rowId];
            /* add the data */
            row.Cells[0].Value = Req_Id;
            row.Cells[1].Value = "Uncovered";
            row.Cells[2].Value = "N/A";
            /* set the color */
            row.DefaultCellStyle.BackColor = Color.Red;
        }
    }
}