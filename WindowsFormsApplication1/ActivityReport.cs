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
using System.IO;

namespace BiometricDb
{
    public partial class ActivityReport : Form
    {
        String connectionAddress = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\FERGAL O NEILL\\Documents\\Fergal Final Year Folder\\Software Engineering Project\\BiometricDb\\BiometricDb\\WindowsFormsApplication1\\InD.mdf;Integrated Security=True";
        System.Data.SqlClient.SqlConnection con;
        int accessLevel, locationId;
        public ActivityReport()
        {
            InitializeComponent();
            con = new System.Data.SqlClient.SqlConnection();
            con.ConnectionString = connectionAddress;
            con.Open();

            using (var cmd = new SqlCommand("Select * from Locations", con))
            {
                String value, location;
                SqlDataAdapter daLocation = new SqlDataAdapter(cmd);
                DataSet dsLocationSearch = new DataSet("LocationSearch");
                daLocation.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                daLocation.Fill(dsLocationSearch, "Locations");

                DataTable tblLocationDetails;
                tblLocationDetails = dsLocationSearch.Tables["Locations"];
                var list = new List<Tuple<string, int>>();

                foreach (DataRow drCurrent in tblLocationDetails.Rows)
                {
                    String locationName = drCurrent["LocationName"].ToString();
                    String locationAccessLevel = drCurrent["AccessLevel"].ToString();
                    int accessLevelInt = Int32.Parse(locationAccessLevel);

                    comboBox1.Items.Add((locationName));
                    comboBox1.ValueMember = locationAccessLevel;
                    comboBox1.DisplayMember = locationName;

                    //setting up date/time pickers
                    dateTimePicker1.Value = DateTime.Today;
                    dateTimePicker2.Value = DateTime.Now;
                    dateTimePicker2.MaxDate = DateTime.Parse("23:59:59");
                    if (dateTimePicker2.Value >= DateTime.Parse("22:59:59"))
                    {
                        // setting the date/time picker to 1 min before minight otherwise system crashes
                        dateTimePicker3.Value = DateTime.Parse("23:59:59");
                    }
                    else
                    {
                        dateTimePicker3.Value = DateTime.Now.AddHours(+1);
                    }
                    dateTimePicker3.MaxDate = DateTime.Parse("23:59:59");

                }
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = false;
                comboBox1.Enabled = true;
                textBox10.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox2.Checked = false;
                comboBox1.Enabled = false;
                textBox10.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gridCleanUp();
            if (checkBox1.Checked == true) // If searching by employee
            {
                String commandString = createCommand();


                using (var cmd = new SqlCommand(commandString, con))
                {


                    string timeFrom = dateTimePicker2.Value.ToString("HH:mm:ss");
                    string timeTo = dateTimePicker3.Value.ToString("HH:mm:ss");

                    cmd.Parameters.AddWithValue("@areaId", locationId);
                    cmd.Parameters.AddWithValue("@timeFrom", timeFrom);
                    cmd.Parameters.AddWithValue("@timeTo", timeTo);
                    cmd.Parameters.AddWithValue("@employeeId", textBox10.Text);



                    // Fix this nai
                    SqlDataAdapter daLocation = new SqlDataAdapter(cmd);
                    DataSet dsLocationSearch = new DataSet("LocationSearch");
                    daLocation.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    daLocation.Fill(dsLocationSearch, "Locations");

                    DataTable tblLocationDetails;
                    tblLocationDetails = dsLocationSearch.Tables["Locations"];


                    int row = dsLocationSearch.Tables["Locations"].Rows.Count - 1;

                    for (int r = 0; r <= row; r++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[r].Cells[0].Value = dsLocationSearch.Tables["Locations"].Rows[r].ItemArray[0];
                        dataGridView1.Rows[r].Cells[1].Value = dsLocationSearch.Tables["Locations"].Rows[r].ItemArray[1];
                        dataGridView1.Rows[r].Cells[2].Value = dsLocationSearch.Tables["Locations"].Rows[r].ItemArray[2];
                        dataGridView1.Rows[r].Cells[3].Value = dsLocationSearch.Tables["Locations"].Rows[r].ItemArray[3];
                        dataGridView1.Rows[r].Cells[4].Value = dsLocationSearch.Tables["Locations"].Rows[r].ItemArray[4];
                        dataGridView1.Rows[r].Cells[5].Value = dsLocationSearch.Tables["Locations"].Rows[r].ItemArray[5];
                        dataGridView1.Rows[r].Cells[6].Value = dsLocationSearch.Tables["Locations"].Rows[r].ItemArray[6];


                    }

                    button4.Enabled = true;
                }

            }

            else if (checkBox2.Checked == true) // If searching by Location
            {
                using (var cmd = new SqlCommand("Select EmployeeID, EmployeeForename, EmployeeSurname, AreaName, AccessType, TimeOfAccess, Date from EmployeeAccessHistory WHERE Date like '" + dateTimePicker1.Value.ToShortDateString() + "%' and TimeOfAccess >= @timeFrom and TimeOfAccess <= @timeTo  ", con))
                {
                    string timeFrom = dateTimePicker2.Value.ToString("HH:mm:ss");
                    string timeTo = dateTimePicker3.Value.ToString("HH:mm:ss");

                    cmd.Parameters.AddWithValue("@areaId", locationId);
                    cmd.Parameters.AddWithValue("@timeFrom", timeFrom);
                    cmd.Parameters.AddWithValue("@timeTo", timeTo);


                    SqlDataAdapter daLocation = new SqlDataAdapter(cmd);
                    DataSet dsLocationSearch = new DataSet("LocationSearch");
                    daLocation.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    daLocation.Fill(dsLocationSearch, "LocationsActivity");

                    DataTable tblLocationDetails;
                    tblLocationDetails = dsLocationSearch.Tables["LocationsActivity"];


                    int row = dsLocationSearch.Tables["LocationsActivity"].Rows.Count - 1;

                    for (int r = 0; r <= row; r++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[r].Cells[0].Value = dsLocationSearch.Tables["LocationsActivity"].Rows[r].ItemArray[0];
                        dataGridView1.Rows[r].Cells[1].Value = dsLocationSearch.Tables["LocationsActivity"].Rows[r].ItemArray[1];
                        dataGridView1.Rows[r].Cells[2].Value = dsLocationSearch.Tables["LocationsActivity"].Rows[r].ItemArray[2];
                        dataGridView1.Rows[r].Cells[3].Value = dsLocationSearch.Tables["LocationsActivity"].Rows[r].ItemArray[3];
                        dataGridView1.Rows[r].Cells[4].Value = dsLocationSearch.Tables["LocationsActivity"].Rows[r].ItemArray[4];
                        string time = dsLocationSearch.Tables["LocationsActivity"].Rows[r].ItemArray[5].ToString();
                        dataGridView1.Rows[r].Cells[5].Value = time;
                        dataGridView1.Rows[r].Cells[6].Value = dsLocationSearch.Tables["LocationsActivity"].Rows[r].ItemArray[6];


                    }
                    button4.Enabled = true;
                }


            }

        }

        private void ActivityReport_Load(object sender, EventArgs e)
        {

        }


        public string createCommand()
        {
            string commandString;

            if (textBox1.Text != "" && textBox2.Text == "")
            {
                commandString = "Select EmployeeID, EmployeeForename, EmployeeSurname, AreaName, AccessType, TimeOfAccess, Date from EmployeeAccessHistory WHERE EmployeeId = @employeeid OR EmployeeForename like '" + textBox1.Text + "%' and Date like '" + dateTimePicker1.Value.ToShortDateString() + "%' and TimeOfAccess >= @timeFrom and TimeOfAccess <= @timeTo ";
                return commandString;
            }
            else if (textBox1.Text == "" && textBox2.Text != "")
            {
                commandString = "Select EmployeeID, EmployeeForename, EmployeeSurname, AreaName, AccessType, TimeOfAccess, Date from EmployeeAccessHistory WHERE EmployeeId = @employeeid OR EmployeeSurname like '" + textBox2.Text + "%' and Date like '" + dateTimePicker1.Value.ToShortDateString() + "%' and TimeOfAccess >= @timeFrom and TimeOfAccess <= @timeTo ";
                return commandString;
            }
            else
            {
                commandString = "Select EmployeeID, EmployeeForename, EmployeeSurname, AreaName, AccessType, TimeOfAccess, Date from EmployeeAccessHistory WHERE EmployeeId = @employeeid OR EmployeeForename like '" + textBox1.Text + "%' AND EmployeeSurname like '" + textBox2.Text + "%' and Date like '" + dateTimePicker1.Value.ToShortDateString() + "%' and TimeOfAccess >= @timeFrom and TimeOfAccess <= @timeTo ";
                return commandString;
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var cmd = new SqlCommand("Select * from Locations WHERE LocationName = @locationName", con))
            {

                cmd.Parameters.AddWithValue("@locationName", this.comboBox1.GetItemText(this.comboBox1.SelectedItem));
                SqlDataAdapter daLocation = new SqlDataAdapter(cmd);
                DataSet dsLocationSearch = new DataSet("LocationSearch");
                daLocation.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                daLocation.Fill(dsLocationSearch, "Locations");

                DataTable tblLocationDetails;
                tblLocationDetails = dsLocationSearch.Tables["Locations"];

                foreach (DataRow drCurrent in tblLocationDetails.Rows)
                {
                    String locationIdgrab = drCurrent["Id"].ToString();
                    locationId = Int32.Parse(locationIdgrab);
                    String accessLevelgrab = drCurrent["AccessLevel"].ToString();
                    accessLevel = Int32.Parse(accessLevelgrab);
                }
            }
        }


        public void gridCleanUp()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Rows.Clear();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // creating Excel Application
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();

            // creating new WorkBook within Excel application
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);


            // creating new Excelsheet in workbook
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            // see the excel sheet behind the program
            app.Visible = true;

            // get the reference of first sheet. By default its name is Sheet1.
            // store its reference to worksheet
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;

            // changing the name of active sheet
            worksheet.Name = "Exported from Activity Report";

            //Add title to the report
            DateTime dateTime = DateTime.UtcNow.Date;
            worksheet.Cells[1] = "Employee Activity Report        " + dateTime.ToString("dd/MM/yyyy");
            worksheet.get_Range("A1", "A1").Font.Size = 14;
            worksheet.get_Range("A1", "A1").Font.Bold = true;

            // storing header part in Excel
            for (int i = 2; i < dataGridView1.Columns.Count + 2; i++)
            {
                worksheet.get_Range("A1", "H3").Font.Size = 12;
                worksheet.get_Range("A1", "H3").Font.Bold = true;
                worksheet.Cells[3, i] = dataGridView1.Columns[i - 2].HeaderText;
                worksheet.Rows[3].Columns.Autofit();
            }

            // storing Each row and column value to excel sheet
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    worksheet.Cells[i + 4, j + 2] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    //auto fit columns for any size of text
                    worksheet.Rows[i + 5].Autofit();
                    //apply border to cell to form a grid
                    worksheet.Cells[i + 4, j + 2].Borders.Color = System.Drawing.Color.Black.ToArgb();
                }
            }

            //sets Columns to autofit text
            worksheet.Columns.AutoFit();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); // cancel button returns to previous form
        }


    }
   }
 


