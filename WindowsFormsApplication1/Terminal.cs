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
    public partial class Terminal : Form
    {
        int EmployeeAccesLevel;
        int accessLevel, locationId;
        String connectionAddress = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\FERGAL O NEILL\\Documents\\Fergal Final Year Folder\\Software Engineering Project\\BiometricDb\\BiometricDb\\WindowsFormsApplication1\\InD.mdf;Integrated Security=True";
        System.Data.SqlClient.SqlConnection con;
        public Terminal()
        {
            InitializeComponent();
            con = new System.Data.SqlClient.SqlConnection();
            con.ConnectionString = connectionAddress;
            con.Open();

            using (var cmd = new SqlCommand("Select * from Locations", con))
            {

                SqlDataAdapter daLocation = new SqlDataAdapter(cmd);
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
            //if (textBox10.Text = ){
            //    MessageBox.Show("Please Enter Employee Id");
            //} 
            //else{
            con = new System.Data.SqlClient.SqlConnection();
            con.ConnectionString = connectionAddress;
            con.Open();


            using (var cmd = new SqlCommand("Select * from EmployeeDetails where BiometricMarker = @bioMarker", con))
            {
                int Biomarker = Int32.Parse(textBox10.Text);
                cmd.Parameters.AddWithValue("@bioMarker", Biomarker);

                SqlDataAdapter daEmployee = new SqlDataAdapter(cmd);
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


                    //if (drCurrent["Photo"].ToString() != "")
                    //{
                    //    var data = (Byte[])(drCurrent["Photo"]);

                    //    using (MemoryStream ms = new MemoryStream(data.FrontImage))
                    //    {
                    //        img = Image.FromStream(ms);
                    //        pictureBox1.Image = img;
                    //    }
                    //        //Image newImage;
                    //        //MemoryStream ms = new MemoryStream(data);

                    //        ////Set image variable value using memory stream.
                    //        //newImage = Image.FromStream(ms);
                    //        ////set picture


                    //}

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
            con.Close();
            updateAndcleanUp();
        }




        public async void updateAndcleanUp()
        {

            if (textBox5.Text == "Granted")
            {
                con.ConnectionString = connectionAddress;
                con.Open();

                String query = "INSERT INTO EmployeeAccessHistory(EmployeeId, AreaId, Time, AccessType, Date) VALUES(@employeeId, @areaId, @time, @accessType, @date)";
               

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                  
                    cmd.Parameters.AddWithValue("@employeeId", textBox2.Text);
                    cmd.Parameters.AddWithValue("@areaId", locationId);

                    if (comboBox2.SelectedItem == "Enter")
                    {
                    cmd.Parameters.AddWithValue("@accessType", "Enter");
                    }
                    else
                    {
                    cmd.Parameters.AddWithValue("@accessType", "Exit");
                    }
                  

                    cmd.Parameters.AddWithValue("time", DateTime.Now.ToString("HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@date", DateTime.Now.Date);

                    // execute the insert statement and store the result
                    cmd.ExecuteNonQuery();
                    con.Close();

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
            textBox10.Enabled = true;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            pictureBox1.Image = null;
            buttonGo.Enabled = true;



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
            String accessMessage = "This Area Requires an Access Level of " + accessLevel;
            label7.Text = accessMessage;
            textBox10.Enabled = true;
            buttonGo.Enabled = true;
        }                                
        
    }
}
