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
    public partial class LocationMaintenance : Form
    {
        int accessLevel, locationId;
        //System.Data.SqlClient.SqlConnection con;
        //String connectionAddress = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\FERGAL O NEILL\\Documents\\Fergal Final Year Folder\\Software Engineering Project\\BiometricDb\\BiometricDb\\WindowsFormsApplication1\\InD.mdf;Integrated Security=True";
        //String connectionAddress = "Data Source=jdickinson03.public.cs.qub.ac.uk;Initial Catalog=jdickinson03;User ID=jdickinson03;Password=5rmp7b1x2hzsv42f";

        String connectionAddress = "server=jdickinson03.students.cs.qub.ac.uk;user id=jdickinson03;pwd=pbx0c2qm8z5q733n;database=jdickinson03;persistsecurityinfo=True";

        MySqlConnection conn = new MySqlConnection();
        public LocationMaintenance()
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

                    comboBox2.Items.Add((locationName));
                    comboBox2.ValueMember = locationAccessLevel;
                    comboBox2.DisplayMember = locationName;

                }
            }
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            textBox10.Enabled = false;
            textBox1.Enabled = true;         
            comboBox1.Enabled = true; 
            buttonSave.Enabled = true;      
            buttonSearch.Enabled = false;

        }


        public void search(string s)
        {

            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connectionAddress;
            conn.Open();


            using (var cmd = new MySqlCommand("Select * from Locations where Id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", locationId);

                MySqlDataAdapter daLocation = new MySqlDataAdapter(cmd);
                DataSet dsLocationSearch = new DataSet("LocationSearch");
                daLocation.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                daLocation.Fill(dsLocationSearch, "Locations");

                DataTable tblLocationDetails;
                tblLocationDetails = dsLocationSearch.Tables["Locations"];

                foreach (DataRow drCurrent in tblLocationDetails.Rows)
                {
                    textBox10.Text = drCurrent["Id"].ToString();
                    textBox1.Text = drCurrent["LocationName"].ToString();            
                    comboBox1.Text = drCurrent["AccessLevel"].ToString();                
                }
                textBox1.Enabled = true;
                comboBox1.Enabled = true;
                buttonSave.Enabled = true;
                buttonSearch.Enabled = false;
                textBox10.Enabled = false;
                buttonNew.Enabled = false;
                buttonDelete.Enabled = true;

            }

        }


        private void buttonDelete_Click_1(object sender, EventArgs e)
        {
            string query = "DELETE FROM Locations WHERE Id = @id";
            MySqlCommand cmd1 = new MySqlCommand(query, conn);
            cmd1.Parameters.AddWithValue("@id", textBox10.Text);

            int result = cmd1.ExecuteNonQuery();

            if (result > 0)
            {
                MessageBox.Show("Location successfully deleted");
            }
            else
            {
                MessageBox.Show("Error deleting Location");
            }

            cleanup();
        }

        public void cleanup()
        {

            textBox10.Text = "";
            textBox10.Enabled = false;
            textBox1.Enabled = false;
            comboBox1.Enabled = false;
            textBox1.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            buttonSave.Enabled = false;  
            buttonSearch.Enabled = true;

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connectionAddress;
            conn.Open();
            string query;
            if (textBox10.Text != "")
            {
                query = "UPDATE Locations SET LocationName=@locationName, AccessLevel=@accessLevel WHERE Id =" + textBox10.Text;

            }
            else
            {
                query = "INSERT INTO Locations(LocationName, AccessLevel) VALUES(@locationName,@accessLevel)";

            }
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {

                int accessLevel = 1;
                if (comboBox1.SelectedItem == "Level 1")
                {
                    accessLevel = 1;
                }
                else if (comboBox1.SelectedItem == "Level 2")
                {
                    accessLevel = 2;
                }
                else if (comboBox1.SelectedItem == "Level 3")
                {
                    accessLevel = 3;
                }

                cmd.Parameters.AddWithValue("@locationName", textBox1.Text);
                cmd.Parameters.AddWithValue("@accessLevel", accessLevel);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully Added");
                conn.Close();
                cleanup();


            }
        }

        private void buttonSearch_Click_1(object sender, EventArgs e)
        {
            search(textBox10.Text);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LocationMaintenance_Load(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            // this code collects the area Id of the selected search for later use
             using (var cmd = new MySqlCommand("Select * from Locations WHERE LocationName = @locationName", conn))
            {

                cmd.Parameters.AddWithValue("@locationName", this.comboBox2.GetItemText(this.comboBox2.SelectedItem));
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
                }
            }
        
        }
        }

       

    }

