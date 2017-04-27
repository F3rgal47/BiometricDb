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
using MySql.Data;
using MySql.Data.MySqlClient;

namespace BiometricDb
{
    public partial class Terminal : Form
    {
        int EmployeeAccesLevel;
        int accessLevel, locationId, areaId;
        //String connectionAddress = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\FERGAL O NEILL\\Documents\\Fergal Final Year Folder\\Software Engineering Project\\BiometricDb\\BiometricDb\\WindowsFormsApplication1\\InD.mdf;Integrated Security=True";
        //String connectionAddress = "Data Source=jdickinson03.public.cs.qub.ac.uk;Initial Catalog=jdickinson03;User ID=jdickinson03;Password=5rmp7b1x2hzsv42f";
        //System.Data.SqlClient.SqlConnection con;
        String connectionAddress = "server=jdickinson03.students.cs.qub.ac.uk;user id=jdickinson03;pwd=pbx0c2qm8z5q733n;database=jdickinson03;persistsecurityinfo=True";
        //System.Data.SqlClient.SqlConnection con;
        MySqlConnection conn = new MySqlConnection("server=jdickinson03.students.cs.qub.ac.uk;user id=jdickinson03;pwd=pbx0c2qm8z5q733n;database=jdickinson03;persistsecurityinfo=True");           
       
        public Terminal()
        {
            InitializeComponent();
           
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connectionAddress;
            conn.Open();

            using (var cmd = new MySqlCommand("Select * from Locations", conn))
            {

                MySqlDataAdapter daLocation = new MySqlDataAdapter(cmd);
                DataSet dsLocationSearch = new DataSet("LocationSearch");
                daLocation.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                daLocation.Fill(dsLocationSearch, "Locations");

                DataTable tblLocationDetails;
                tblLocationDetails = dsLocationSearch.Tables["Locations"];

                foreach (DataRow drCurrent in tblLocationDetails.Rows)
                {
                    String locationName = drCurrent["LocationName"].ToString();
                    String locationAccessLevel = drCurrent["AccessLevel"].ToString();

                    comboBox1.Items.Add((locationName));
                    comboBox1.ValueMember = locationAccessLevel;
                    comboBox1.DisplayMember = locationName;

                }
            }
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            search();
        }

        public void search()
        {
  
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connectionAddress;
            conn.Open();


            using (var cmd = new MySqlCommand("Select * from EmployeeDetails where BiometricMarker = @bioMarker", conn))
            {
                int Biomarker = Int32.Parse(textBox10.Text);
                cmd.Parameters.AddWithValue("@bioMarker", Biomarker);

                MySqlDataAdapter daEmployee = new MySqlDataAdapter(cmd);
                DataSet dsEmployeeSearch = new DataSet("employeeSearch");
                daEmployee.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                daEmployee.Fill(dsEmployeeSearch, "EmployeeDetails");

                DataTable tblEmployeeDetails;
                tblEmployeeDetails = dsEmployeeSearch.Tables["EmployeeDetails"];

                foreach (DataRow drCurrent in tblEmployeeDetails.Rows)
                {
                    textBox2.Text = drCurrent["Id"].ToString();
                    textBox1.Text = drCurrent["Forename"].ToString();
                    textBox3.Text = drCurrent["Surname"].ToString();
                    textBox4.Text = drCurrent["AccessLevel"].ToString();
                    EmployeeAccesLevel = Int32.Parse(textBox4.Text);

                    if (drCurrent["Photo"].ToString() != "")
                    {
                        //BLOB is read into Byte array, then used to construct MemoryStream, then passed to PictureBox.
                        Byte[] byteBLOBData = new Byte[0];

                        byteBLOBData = (Byte[])(drCurrent["Photo"]);

                        MemoryStream stmBLOBData = new MemoryStream(byteBLOBData);
                        pictureBox1.Image = Image.FromStream(stmBLOBData);

                        //create a new Bitmap with the proper dimensions
                        Bitmap finalImg;
                        finalImg = new Bitmap(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);

                        //center the new image
                        pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;

                        //set the new image
                        pictureBox1.Image = finalImg;
                    }

                }


                buttonGo.Enabled = false;
                textBox10.Enabled = false;
               

                if (EmployeeAccesLevel >= accessLevel)
                {
                    textBox5.Text = "Granted";
                    textBox5.BackColor = Color.LimeGreen;

                }
                else if (EmployeeAccesLevel < accessLevel)
                {
                    textBox5.Text = "Denied";
                    textBox5.BackColor = Color.Red;

                }

            }
            conn.Close();
            updateAndcleanUp();
        }




        public async void updateAndcleanUp()
        {

            if (textBox5.Text == "Granted")
            {
                conn.ConnectionString = connectionAddress;
                conn.Open();

                String query = "INSERT INTO EmployeeAccessHistory(EmployeeId,EmployeeForename,EmployeeSurname, AreaId,AreaName, TimeOfAccess, AccessType, Date, LocationId, LocationName) VALUES(@employeeId, @employeeForename, @employeeSurname, @areaId, @areaName, @time, @accessType, @date, @locationId, @locationName)";
               

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                  
                    cmd.Parameters.AddWithValue("@employeeId", textBox2.Text);
                    cmd.Parameters.AddWithValue("@employeeForename", textBox1.Text);
                    cmd.Parameters.AddWithValue("@employeeSurname", textBox3.Text);
                    cmd.Parameters.AddWithValue("@areaId", areaId);
                    cmd.Parameters.AddWithValue("@areaName", this.comboBox3.GetItemText(this.comboBox3.SelectedItem));
                    cmd.Parameters.AddWithValue("@locationId", locationId);
                    cmd.Parameters.AddWithValue("@locationName", this.comboBox1.GetItemText(this.comboBox1.SelectedItem));

                    if (comboBox2.SelectedItem == "Enter")
                    {
                    cmd.Parameters.AddWithValue("@accessType", "Enter");
                    }
                    else
                    {
                    cmd.Parameters.AddWithValue("@accessType", "Exit");
                    }
                    string currentTime = DateTime.Now.ToString("HH:mm");
                    string currentDate = DateTime.Now.ToString("dd/MM/yyyy");
                    cmd.Parameters.AddWithValue("@time",  DateTime.Parse(currentTime));
                   
                    cmd.Parameters.AddWithValue("@date", currentDate);

                    // execute the insert statement and store the result
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }
            }
        
            await Task.Delay(5000);
            textBox10.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox5.BackColor = Color.White;
            textBox10.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            pictureBox1.Image = null;
            buttonGo.Enabled = false;
            comboBox2.Text = "";
            
            



        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            // this code collects the Location Id of the selected search for later use
            using (var cmd = new MySqlCommand("Select * from Locations WHERE LocationName = @locationName", conn))
            {

                cmd.Parameters.AddWithValue("@locationName", this.comboBox1.GetItemText(this.comboBox1.SelectedItem));
                MySqlDataAdapter daLocation = new MySqlDataAdapter(cmd);
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
                    comboBox3.Enabled = true;                
                }
            }

            using (var cmd = new MySqlCommand("Select * from LocationAreas WHERE LocationId = " + locationId, conn)) // populate combobox 3 with areas from selected location
            {

                MySqlDataAdapter daArea = new MySqlDataAdapter(cmd);
                DataSet dsArea = new DataSet("AreaSearch");
                daArea.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                daArea.Fill(dsArea, "Areas");

                DataTable tblArea;
                tblArea = dsArea.Tables["Areas"];

                foreach (DataRow drCurrent in tblArea.Rows)
                {
                    String AreaName = drCurrent["AreaName"].ToString();
                    String locationAccessLevel = drCurrent["AccessLevel"].ToString();

                    comboBox3.Items.Add((AreaName));
                    comboBox3.ValueMember = locationAccessLevel;
                    comboBox3.DisplayMember = AreaName;

                }
            }
            String accessMessage = "This Area Requires an Access Level of " + accessLevel;
            label7.Text = accessMessage;
           
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var cmd = new MySqlCommand("Select * from LocationAreas WHERE AreaName = @areaName", conn)) // grab Area Id
            {

                cmd.Parameters.AddWithValue("@areaName", this.comboBox3.GetItemText(this.comboBox3.SelectedItem));

                MySqlDataAdapter daArea = new MySqlDataAdapter(cmd);
                DataSet dsArea = new DataSet("AreaSearch");
                daArea.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                daArea.Fill(dsArea, "Areas");

                DataTable tblArea;
                tblArea = dsArea.Tables["Areas"];

                foreach (DataRow drCurrent in tblArea.Rows)
                {
                    String areaIdgrab = drCurrent["Id"].ToString();
                    areaId = Int32.Parse(areaIdgrab);
                }
            }
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                textBox10.Enabled = true;
                buttonGo.Enabled = true;
            }
        }                           
        
    }
}
