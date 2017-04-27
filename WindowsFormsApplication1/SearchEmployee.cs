using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace BiometricDb
{
    public partial class SearchEmployee : Form
    {
        public static string employeeId;
        //String connectionAddress = "Data Source=jdickinson03.public.cs.qub.ac.uk;Initial Catalog=jdickinson03;User ID=jdickinson03;Password=5rmp7b1x2hzsv42f";       
        ////con.ConnectionString = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\FERGAL O NEILL\\Documents\\Fergal Final Year Folder\\Software Engineering Project\\BiometricDb\\BiometricDb\\WindowsFormsApplication1\\InD.mdf;Integrated Security=True";
        //System.Data.SqlClient.SqlConnection con;
        String connectionAddress = "server=jdickinson03.students.cs.qub.ac.uk;user id=jdickinson03;pwd=pbx0c2qm8z5q733n;database=jdickinson03;persistsecurityinfo=True";

        MySqlConnection conn = new MySqlConnection();

        public SearchEmployee()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gridCleanUp();
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connectionAddress;
            conn.Open();
            String commandString = createCommand();
        
            using (var cmd = new MySqlCommand(commandString, conn))
           
            {
                cmd.Parameters.AddWithValue("@id", textBox10.Text);
                //cmd.Parameters.AddWithValue("@forename", textBox1.Text);
                //cmd.Parameters.AddWithValue("@surname", textBox2.Text);


                MySqlDataAdapter daEmployee = new MySqlDataAdapter(cmd);
                DataSet dsEmployeeSearch = new DataSet("employeeSearch");
                daEmployee.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                daEmployee.Fill(dsEmployeeSearch, "EmployeeDetails");

                DataTable tblEmployeeDetails;
                tblEmployeeDetails = dsEmployeeSearch.Tables["EmployeeDetails"];


            
            int row = dsEmployeeSearch.Tables["EmployeeDetails"].Rows.Count -1;

            for (int r = 0; r <= row; r++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[r].Cells[0].Value = dsEmployeeSearch.Tables["EmployeeDetails"].Rows[r].ItemArray[0];
                dataGridView1.Rows[r].Cells[1].Value = dsEmployeeSearch.Tables["EmployeeDetails"].Rows[r].ItemArray[1];
                dataGridView1.Rows[r].Cells[2].Value = dsEmployeeSearch.Tables["EmployeeDetails"].Rows[r].ItemArray[2];
                dataGridView1.Rows[r].Cells[3].Value = dsEmployeeSearch.Tables["EmployeeDetails"].Rows[r].ItemArray[3];

            }
            button3.Enabled = true;
            button4.Enabled = true;

            }
        } 

        private void button3_Click(object sender, EventArgs e)
        {
            employeeId = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            this.Visible = false;
            EmployeeMaintenance SearchEmployeeForm = new EmployeeMaintenance();
            SearchEmployeeForm.ShowDialog();
            this.Visible = true;
        }

        public void gridCleanUp()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Rows.Clear();

        }

        public string createCommand()
        {
                string commandString;

                if (textBox10.Text != "" && textBox2.Text == "" && textBox1.Text == "")
                {
                    commandString = "SELECT * FROM EmployeeDetails WHERE Id = @id";
                    return commandString;
                }
                else if (textBox1.Text != "" && textBox2.Text == "")
                {
                  commandString = "SELECT * FROM EmployeeDetails WHERE Id = @id OR Forename like '" + textBox1.Text + "%' ";
                  return commandString;
                }
                else if (textBox1.Text == "" && textBox2.Text != "")
                {
                    commandString = "SELECT * FROM EmployeeDetails WHERE Id = @id OR Surname like '" + textBox2.Text + "%' ";
                  return commandString;
                }
                else
                {
                     commandString = "SELECT * FROM EmployeeDetails WHERE Id = @id OR Forename like '" + textBox1.Text + "%' AND Surname like '" + textBox2.Text + "%' ";
                  return commandString;
                }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // creating Excel Application
            Microsoft.Office.Interop.Excel._Application app  = new Microsoft.Office.Interop.Excel.Application();
 
            // creating new WorkBook within Excel application
            Microsoft.Office.Interop.Excel._Workbook workbook =  app.Workbooks.Add(Type.Missing);
           
 
            // creating new Excelsheet in workbook
             Microsoft.Office.Interop.Excel._Worksheet worksheet = null;                   
           
           // see the excel sheet behind the program
            app.Visible = true;
          
           // get the reference of first sheet. By default its name is Sheet1.
           // store its reference to worksheet
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
 
            // changing the name of active sheet
            worksheet.Name = "Exported from Employee Search";

            //Add title to the report
            DateTime dateTime = DateTime.UtcNow.Date;
            worksheet.Cells[1] = "Employee Search Report        " + dateTime.ToString("dd/MM/yyyy");
            worksheet.get_Range("A1", "A1").Font.Size = 14;
            worksheet.get_Range("A1", "A1").Font.Bold = true;

            // storing header part in Excel
            for(int i=1;i<dataGridView1.Columns.Count+1;i++)
            {
                worksheet.get_Range("A1", "D3").Font.Size = 12;
                worksheet.get_Range("A1", "D3").Font.Bold = true;
                worksheet.Cells[3, i] = dataGridView1.Columns[i-1].HeaderText;
                worksheet.Rows[3].Columns.Autofit();
            }
 
            // storing Each row and column value to excel sheet
            for (int i=0; i < dataGridView1.Rows.Count; i++)
            {
                for(int j=0;j<dataGridView1.Columns.Count;j++)
                {
                    worksheet.Cells[i + 4, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    //auto fit columns for any size of text
                    worksheet.Rows[i + 5].Autofit();
                    //apply border to cell to form a grid
                    worksheet.Cells[i + 4, j + 1].Borders.Color = System.Drawing.Color.Black.ToArgb();
                }
            }

            //sets email Column to autofit text
            worksheet.Columns["D"].AutoFit();
        }

        private void SearchEmployee_Load(object sender, EventArgs e)
        {

        }
 
    }
    
}
