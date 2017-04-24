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


                }
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
            textBox10.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            checkBox1.Checked = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;
            textBox10.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            checkBox2.Checked = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true) // If searching by employee
            {
                String commandString = createCommand();


                using (var cmd = new SqlCommand(commandString, con))
                {

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
                    button3.Enabled = true;
                    button4.Enabled = true;
                }

            }

            else if (checkBox2.Checked == true) // If searching by Location
            {
                using (var cmd = new SqlCommand("Select EmployeeID, EmployeeForename, EmployeeSurname, AreaName, AccessType, TimeOfAccess, Date from EmployeeAccessHistory WHERE AreaId = @areaId AND Date = @date AND TimeOfAccess >= TimeOfAccess between @timeFrom and @timeTo", con))
            {
                string date = dateTimePicker1.Value.ToShortDateString();
                string timeFrom = dateTimePicker2.Value.ToShortTimeString();
                string timeTo = dateTimePicker3.Value.ToShortTimeString();
                string why = this.comboBox1.GetItemText(this.comboBox1.ValueMember);

                cmd.Parameters.AddWithValue("@areaId", locationId);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@timeFrom", timeFrom);
                cmd.Parameters.AddWithValue("@timeTo", timeTo);


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
                    string time = dsLocationSearch.Tables["Locations"].Rows[r].ItemArray[5].ToString();
                    dataGridView1.Rows[r].Cells[5].Value = time;
                    dataGridView1.Rows[r].Cells[6].Value = dsLocationSearch.Tables["Locations"].Rows[r].ItemArray[6];
                   

                }
                button3.Enabled = true;
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
                commandString = "Select EmployeeID, EmployeeForename, EmployeeSurname, AreaName, AccessType, TimeOfAccess, Date from EmployeeAccessHistory WHERE EmployeeId = @employeeid OR EmployeeForename like '" + textBox1.Text + "%' ";
                return commandString;
            }
            else if (textBox1.Text == "" && textBox2.Text != "")
            {
                commandString = "Select EmployeeID, EmployeeForename, EmployeeSurname, AreaName, AccessType, TimeOfAccess, Date from EmployeeAccessHistory WHERE EmployeeId = @employeeid OR EmployeeSurname like '" + textBox2.Text + "%' ";
                return commandString;
            }
            else
            {
                commandString = "Select EmployeeID, EmployeeForename, EmployeeSurname, AreaName, AccessType, TimeOfAccess, Date from EmployeeAccessHistory WHERE EmployeeId = @employeeid OR EmployeeForename like '" + textBox1.Text + "%' AND EmployeeSurname like '" + textBox2.Text + "%' ";
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
   }
 }

