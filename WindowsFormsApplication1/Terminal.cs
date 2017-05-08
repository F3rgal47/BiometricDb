using System;
using System.Diagnostics;
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
using SourceAFIS.Simple;

namespace BiometricDb
{

    public partial class Terminal : Form
    {
        public static string restart = "0";
        int EmployeeAccesLevel;
        int accessLevel, locationId, areaId;
        Bitmap finalFingerImg;
        string fingerprintFilePath = @"\BiometricDb\BiometricDb\WindowsFormsApplication1\TemporaryFingerPhotos\";
        // String connectionAddress = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\FERGAL O NEILL\\Documents\\Fergal Final Year Folder\\Software Engineering Project\\BiometricDb\\BiometricDb\\WindowsFormsApplication1\\InD.mdf;Integrated Security=True";
        // String connectionAddress = "Data Source=jdickinson03.public.cs.qub.ac.uk;Initial Catalog=jdickinson03;User ID=jdickinson03;Password=5rmp7b1x2hzsv42f";
        // System.Data.SqlClient.SqlConnection con;
        String connectionAddress = "server=jdickinson03.students.cs.qub.ac.uk;user id=jdickinson03;pwd=pbx0c2qm8z5q733n;database=jdickinson03;persistsecurityinfo=True";
        // System.Data.SqlClient.SqlConnection con;
        MySqlConnection conn = new MySqlConnection("server=jdickinson03.students.cs.qub.ac.uk;user id=jdickinson03;pwd=pbx0c2qm8z5q733n;database=jdickinson03;persistsecurityinfo=True");           
       
        public Terminal()
        {
            InitializeComponent();
            // Filepaths for photos temporaily saved to folders, these are cleared during cleanup.
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connectionAddress;
            conn.Open();


           
            //PopulateTemporaryFolders();

            // Populate the locations comboBox before opening the terminal.
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
                    string locationName = drCurrent["LocationName"].ToString();
                    string locationAccessLevel = drCurrent["AccessLevel"].ToString();

                    comboBox1.Items.Add((locationName));
                    comboBox1.ValueMember = locationAccessLevel;
                    comboBox1.DisplayMember = locationName;
                }
            }
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            GetFingerprintandMatch();
        }

        //public void Search()
        //{
        //    conn = new MySql.Data.MySqlClient.MySqlConnection();
        //    conn.ConnectionString = connectionAddress;
        //    conn.Open();

        //    // search for a employee using the fingerprint they input
        //    using (var cmd = new MySqlCommand("Select * from EmployeeDetails where Id = @id", conn))
        //    {

        //        cmd.Parameters.AddWithValue("@id", textBox2.Text);

        //        MySqlDataAdapter daEmployee = new MySqlDataAdapter(cmd);
        //        DataSet dsEmployeeSearch = new DataSet("employeeSearch");
        //        daEmployee.MissingSchemaAction = MissingSchemaAction.AddWithKey;
        //        daEmployee.Fill(dsEmployeeSearch, "EmployeeDetails");

        //        DataTable tblEmployeeDetails;
        //        tblEmployeeDetails = dsEmployeeSearch.Tables["EmployeeDetails"];

        //        foreach (DataRow drCurrent in tblEmployeeDetails.Rows)
        //        {
        //            textBox2.Text = drCurrent["Id"].ToString();
        //            textBox1.Text = drCurrent["Forename"].ToString();
        //            textBox3.Text = drCurrent["Surname"].ToString();
        //            textBox4.Text = drCurrent["AccessLevel"].ToString();
        //            EmployeeAccesLevel = Int32.Parse(textBox4.Text);

        //            if (drCurrent["Photo"].ToString() != "")
        //            {
        //                // BLOB is read into Byte array, then used to construct MemoryStream, then passed to PictureBox.
        //                Byte[] byteBLOBData = new Byte[0];
        //                byteBLOBData = (Byte[])(drCurrent["Photo"]);
        //                MemoryStream stmBLOBData = new MemoryStream(byteBLOBData);
        //                pictureBox1.Image = Image.FromStream(stmBLOBData);

        //                // Create a new Bitmap with the proper dimensions.
        //                Bitmap finalImg;
        //                finalImg = new Bitmap(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);

        //                // Center the new image.
        //                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;

        //                // Set the new image.
        //                pictureBox1.Image = finalImg;
        //            }

        //        }


        //        buttonGo.Enabled = false;
                
               
        //        //check the access level of the employee compared to the required access level.
        //        if (EmployeeAccesLevel >= accessLevel)
        //        {
        //            textBox5.Text = "Granted";
        //            textBox5.BackColor = Color.LimeGreen;

        //        }
        //        else if (EmployeeAccesLevel < accessLevel)
        //        {
        //            textBox5.Text = "Denied";
        //            textBox5.BackColor = Color.Red;

        //        }

        //    }
        //    conn.Close();
        //    UpdateAndCleanUp();
        //}




        public async void UpdateAndCleanUp()
        {
            // Inserting employees details in employeeAccessHistory table. 
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

                    // Execute the insert statement and store the result.
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }
            }
            // Wait five seconds then refresh the terminal.
            await Task.Delay(5000);
            
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox5.BackColor = Color.White;
            
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            pictureBox1.Image = null;
            buttonGo.Enabled = false;
            comboBox2.Text = "";

            restart = "1";
            this.Close();
            //Terminal accessForm = new Terminal();
            //accessForm.ShowDialog();         
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            // This code collects the Location Id of the selected location for later use.
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

            using (var cmd = new MySqlCommand("Select * from LocationAreas WHERE LocationId = " + locationId, conn)) 

            {
                // Populate Area combobox with areas from selected location.
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
                       
        }


        public void PopulateTemporaryFolders()
        {
            // Populate the temporary folder with each fingerprint Image in the enployeeDetails table for use in employee matching.
             using (var cmd = new MySqlCommand("Select * from EmployeeDetails ", conn))
             {
                 MySqlDataAdapter daFinger = new MySqlDataAdapter(cmd);
                 DataSet dsFinger = new DataSet("FingerSearch");
                 daFinger.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                 daFinger.Fill(dsFinger, "Fingers");

                 DataTable tblArea;
                 tblArea = dsFinger.Tables["Fingers"];
                 
                 foreach (DataRow drCurrent in tblArea.Rows)
                 {
                     string id = drCurrent["Id"].ToString();
                     if (drCurrent["Photo"].ToString() != "")
                     {
                         if (drCurrent["BiometricMarker"].ToString() != "")
                         {
                             // BLOB is read into Byte array, then used to construct MemoryStream, then passed to PictureBox.
                             Byte[] byteBLOBData = new Byte[0];

                             byteBLOBData = (Byte[])(drCurrent["BiometricMarker"]);

                             MemoryStream stmBLOBData = new MemoryStream(byteBLOBData);
                             pictureBox2.Image = Image.FromStream(stmBLOBData);

                             // Create a new Bitmap with the proper dimensions.

                             finalFingerImg = new Bitmap(pictureBox2.Image, pictureBox2.Width, pictureBox2.Height);

                             // Center the new image.
                             pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;

                             // Set the new image.
                             pictureBox2.Image = finalFingerImg;

                             // Save the Image in a temporary folder.                             
                             pictureBox2.Image.Save(fingerprintFilePath + id + ".jpg");

                         }

                     }
                 }
             }

        }


        public void GetFingerprintandMatch()
        {

            /* Call FingerprintFetch exectuable.
             * Purpose: Fetches fingerprint from connected device, create bitmap image in directory.
             */
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "\\FingerprintFetch\\Debug\\FingerprintFetch.exe";
            startInfo.Arguments = "";
            var proc = Process.Start(startInfo);
            proc.WaitForExit();
            fingerprintFilePath = @"E:\BiometricDb\BiometricDb\WindowsFormsApplication1\bin\Debug\test10.bmp";
            proc.Refresh();
            // Create Person Object.
            Person scannedPerson = new Person();

            /* Create Fingerprint Object.
             * Initalising with fingerprint image created from FingerprintFetch.exe.
             */
            Fingerprint scannedFP = new Fingerprint();
            scannedFP.AsBitmap = new Bitmap(fingerprintFilePath);

            // Add fingerprint object to Person object.
            scannedPerson.Fingerprints.Add(scannedFP);


            /* Iterate database in turn to determine a match for scanned fingerprint.
             * Creates Person and Fingerprint object in turn, then fingerprints are compared to determine match.
             * If -> match employee access granted.
             * Else -> no match continues to search database.
             * Process ends on confirmed match or reached end of database.
             */
            using (var cmd = new MySqlCommand("Select * from EmployeeDetails ", conn))
            {
                MySqlDataAdapter daFinger = new MySqlDataAdapter(cmd);
                DataSet dsFinger = new DataSet("FingerSearch");
                daFinger.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                daFinger.Fill(dsFinger, "Fingers");

                DataTable tblArea;
                tblArea = dsFinger.Tables["Fingers"];


                // Fingerprint engine that extracts template and performs matching.
                AfisEngine abc = new AfisEngine();

                // Person and fingerprint declaration for db BioMarker record.
                Person dbPerson;
                Fingerprint dbFingerprint;

                // Position count in database.
                int count = 0;

                /* Iterate through database initlising a person and fingerprint.
                 * Match person object created from database against Fingerprint from scanner.
                 * Looks for a match in databse and breaks if found else loop run until end of database.
                 */
                foreach (DataRow drCurrent in tblArea.Rows)
                {
                    if (drCurrent["BiometricMarker"].ToString() == "")
                    {
                        continue;
                    }
                    Byte[] byteBLOBData = new Byte[0];
                    byteBLOBData = (Byte[])(drCurrent["BiometricMarker"]);
                    
                    MemoryStream stmBLOBData = new MemoryStream(byteBLOBData);
                    

                    // Person and Fingerprint intialsation.
                    dbPerson = new Person();
                    dbFingerprint = new Fingerprint();

                    // Fingerprint is set with bitmap from database and added to its person object.
                    dbFingerprint.AsBitmap = new Bitmap(Image.FromStream(stmBLOBData));
                    dbPerson.Fingerprints.Add(dbFingerprint);

                    // Fingerprints templates extracted for matching.
                    abc.Extract(scannedPerson);
                    abc.Extract(dbPerson);

                    // Fingerprints are match and a score termines the similiarlites.
                    float score = abc.Verify(scannedPerson, dbPerson);

                    // Determine match on threshold 0.
                    bool match = (score > 0);

                    // If match set terminal field to show employee then break from loop else continue incremeting count.
                    if (match)
                    {
                        int id = (int)drCurrent["Id"];
                        // Fetch match.
                        textBox2.Text = drCurrent["Id"].ToString();
                        textBox1.Text = drCurrent["Forename"].ToString();
                        textBox3.Text = drCurrent["Surname"].ToString();
                        textBox4.Text = drCurrent["AccessLevel"].ToString();
                        EmployeeAccesLevel = Int32.Parse(textBox4.Text);

                        if (drCurrent["Photo"].ToString() != "")
                        {
                            // BLOB is read into Byte array, then used to construct MemoryStream, then passed to PictureBox.
                            Byte[] byteBLOBPhotoData = new Byte[0];
                            byteBLOBPhotoData = (Byte[])(drCurrent["Photo"]);
                            MemoryStream stmBLOBPhotoData = new MemoryStream(byteBLOBPhotoData);
                            pictureBox1.Image = Image.FromStream(stmBLOBPhotoData);

                            // Create a new Bitmap with the proper dimensions.
                            Bitmap finalImg;
                            finalImg = new Bitmap(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);

                            // Center the new image.
                            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;

                            // Set the new image.
                            pictureBox1.Image = finalImg;
                        }

                    }


                    buttonGo.Enabled = false;


                    //check the access level of the employee compared to the required access level.
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
                UpdateAndCleanUp();
            }
        }

        
 

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int accessLevelString = 1;
            // Grab Area Id from the selected area in combobox3.
            using (var cmd = new MySqlCommand("Select * from LocationAreas WHERE AreaName = @areaName", conn)) 
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
                    String locationAccessLevel = drCurrent["AccessLevel"].ToString();
                    accessLevelString = Int32.Parse(locationAccessLevel);
                }
            }
            String accessMessage = "This Area Requires an Access Level of " + accessLevelString;
            label7.Text = accessMessage;
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                buttonGo.Enabled = true;
            }
        }                           
        
    }
}
