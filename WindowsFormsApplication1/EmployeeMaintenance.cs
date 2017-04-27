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
    public partial class EmployeeMaintenance : Form
    {

        Bitmap finalImg;
        //System.Data.SqlClient.SqlConnection con;
        ////String connectionAddress = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\FERGAL O NEILL\\Documents\\Fergal Final Year Folder\\Software Engineering Project\\BiometricDb\\BiometricDb\\WindowsFormsApplication1\\InD.mdf;Integrated Security=True";
        //String connectionAddress = "Data Source=jdickinson03.public.cs.qub.ac.uk;Initial Catalog=jdickinson03;User ID=jdickinson03;Password=5rmp7b1x2hzsv42f";

        String connectionAddress = "server=jdickinson03.students.cs.qub.ac.uk;user id=jdickinson03;pwd=pbx0c2qm8z5q733n;database=jdickinson03;persistsecurityinfo=True";
        //System.Data.SqlClient.SqlConnection con;
        MySqlConnection conn = new MySqlConnection("server=jdickinson03.students.cs.qub.ac.uk;user id=jdickinson03;pwd=pbx0c2qm8z5q733n;database=jdickinson03;persistsecurityinfo=True");           
       

        public EmployeeMaintenance()//checks if the user has opened the maintenance form from search employees
        {
            string CheckIdFromSearch = SearchEmployee.employeeId;
            InitializeComponent();
            if (CheckIdFromSearch != null)
            {
                textBox10.Text = CheckIdFromSearch;
                search(CheckIdFromSearch);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // open file dialog 
            OpenFileDialog open = new OpenFileDialog();
            // image filters
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                // Load local image to PictureBox control

                pictureBox1.Load(open.FileName);

                //create a new Bitmap with the proper dimensions

                finalImg = new Bitmap(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);

                //center the new image
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;

                //set the new image
                pictureBox1.Image = finalImg;

                pictureBox1.Show();
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void button_Save_Click(object sender, EventArgs e)
        {
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = connectionAddress;
            conn.Open();

            if (textBox1.Text == "")
            {
                MessageBox.Show("Please enter first name!");
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Please enter Surname!");
            }
            else if (textBox3.Text == "")
            {
                MessageBox.Show("Please enter company email!");
            }
            else if (textBox4.Text == "")
            {
                MessageBox.Show("Please enter telephone!");
            }
            else if (comboBox1.Text == "")
            {
                MessageBox.Show("Please select access level!");
            }
            else if (textBox11.Text == "")
            {
                MessageBox.Show("Please register fingerprint!");
            }
            else if (textBox5.Text == "")
            {
                MessageBox.Show("Please enter address!");
            }
            else if (textBox8.Text == "")
            {
                MessageBox.Show("Please enter city!");
            }
            else if (textBox9.Text == "")
            {
                MessageBox.Show("Please enter postcode!");
            }
            else if (textBox12.Text == "" || textBox13.Text == "")
            {
                MessageBox.Show("Please enter App Password!");
            }
            else if (textBox12.Text != textBox13.Text)
            {
                MessageBox.Show("Passwords do not match! Please re-enter Password");
            }


            else
            {
            ////convert picture into byte to store to db
            //System.IO.MemoryStream defaultImageStream = new System.IO.MemoryStream();
            //Bitmap NewImage = new Bitmap(finalImg, new Size(720, 227));
            //Image b = (Image)NewImage;
            //b.Save(defaultImageStream, System.Drawing.Imaging.ImageFormat.Bmp);
            //byte[] defaultImageData = new byte[defaultImageStream.Length];           
                string query;
                if (textBox10.Text != "")
                {
                    query = "UPDATE EmployeeDetails SET Forename=@forename, Surname=@surname, Email=@email, TelephoneNo=@phoneNo, AccessLevel=@accessLevel,BiometricMarker=@bioMarker, AddressLine1=@addressLine1, AddressLine2=@addressLine2, AddressLine3= @addressLine3, City=@city, Postcode=@postcode, Password=@password WHERE Id =" + textBox10.Text;
                    //, Photo=@photo
                    //BiometricMarker=@bioMarker,
                }
                else
                {
                    query = "INSERT INTO EmployeeDetails(Forename, Surname, Email, TelephoneNo, BiometricMarker, AccessLevel, AddressLine1, AddressLine2, AddressLine3, City, Postcode, Password) VALUES(@forename, @surname, @email, @phoneNo, @bioMarker, @accessLevel, @addressLine1, @addressLine2, @addressLine3, @city, @postcode,@password)";
                    //, @photo , Photo
                    //BiometricMarker, @bioMarker,
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

                    cmd.Parameters.AddWithValue("@forename", textBox1.Text);
                    cmd.Parameters.AddWithValue("@surname", textBox2.Text);
                    cmd.Parameters.AddWithValue("@email", textBox3.Text);
                    cmd.Parameters.AddWithValue("@phoneNo", textBox4.Text);
                    cmd.Parameters.AddWithValue("@accessLevel", accessLevel);

                    int Biomarker = Int32.Parse(textBox11.Text);
                    cmd.Parameters.AddWithValue("@bioMarker", Biomarker);

                    cmd.Parameters.AddWithValue("@addressLine1", textBox5.Text);
                    cmd.Parameters.AddWithValue("@addressLine2", textBox6.Text);
                    cmd.Parameters.AddWithValue("@addressLine3", textBox7.Text);
                    cmd.Parameters.AddWithValue("@city", textBox8.Text);
                    cmd.Parameters.AddWithValue("@postcode", textBox9.Text);
                    cmd.Parameters.AddWithValue("@password", textBox13.Text);
                    //cmd.Parameters.AddWithValue("@photo", defaultImageData);
                    // execute the insert statement and store the result
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully Added");
                    conn.Close();
                    cleanup();

                }
            }
        }

        private void button_New_Click(object sender, EventArgs e)
        {
            textBox10.Enabled = false;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            comboBox1.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox7.Enabled = true;
            textBox8.Enabled = true;
            textBox9.Enabled = true;
            textBox11.Enabled = true;
            textBox12.Enabled = true;
            textBox13.Enabled = true;
            buttonSave.Enabled = true;
            buttonFingerReg.Enabled = true;
            buttonBrowsePhoto.Enabled = true;
            buttonSearch.Enabled = false;
           
        }
        

        private void button_Search_Click(object sender, EventArgs e)
        {
            search(textBox10.Text);   
        }

        public void cleanup()
        {

            textBox10.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            comboBox1.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
            textBox10.Enabled = true;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            comboBox1.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            textBox9.Enabled = false;
            textBox11.Enabled = false;
            textBox12.Enabled = false;
            textBox13.Enabled = false;
            buttonSave.Enabled = false;
            buttonFingerReg.Enabled = false;
            buttonBrowsePhoto.Enabled = false;
            buttonSearch.Enabled = true;
            pictureBox1.Image = null;          

        }

        public void search(string s)
        {
        
            if (textBox10.Text == "")
            {
                MessageBox.Show("Please enter valid Employee ID!");
                
            }
           
            else
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = connectionAddress;
                conn.Open();


                using (var cmd = new MySqlCommand("Select * from EmployeeDetails where Id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", s);

                    MySqlDataAdapter daEmployee = new MySqlDataAdapter(cmd);
                    DataSet dsEmployeeSearch = new DataSet("employeeSearch");
                    daEmployee.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    daEmployee.Fill(dsEmployeeSearch, "EmployeeDetails");

                    DataTable tblEmployeeDetails;
                    tblEmployeeDetails = dsEmployeeSearch.Tables["EmployeeDetails"];

                    foreach (DataRow drCurrent in tblEmployeeDetails.Rows)
                    {
                        textBox10.Text = drCurrent["Id"].ToString();
                        textBox1.Text = drCurrent["Forename"].ToString();
                        textBox2.Text = drCurrent["Surname"].ToString();
                        textBox3.Text = drCurrent["Email"].ToString();
                        textBox4.Text = drCurrent["TelephoneNo"].ToString();
                        comboBox1.Text = drCurrent["AccessLevel"].ToString();
                        textBox11.Text = drCurrent["BiometricMarker"].ToString();
                        textBox5.Text = drCurrent["AddressLine1"].ToString();
                        textBox6.Text = drCurrent["AddressLine2"].ToString();
                        textBox7.Text = drCurrent["AddressLine3"].ToString();
                        textBox8.Text = drCurrent["City"].ToString();
                        textBox9.Text = drCurrent["Postcode"].ToString();
                        textBox12.Text = drCurrent["Password"].ToString();
                        textBox13.Text = drCurrent["Password"].ToString();
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
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    textBox3.Enabled = true;
                    textBox4.Enabled = true;
                    comboBox1.Enabled = true;
                    textBox5.Enabled = true;
                    textBox6.Enabled = true;
                    textBox7.Enabled = true;
                    textBox8.Enabled = true;
                    textBox9.Enabled = true;
                    textBox11.Enabled = true;
                    textBox12.Enabled = true;
                    textBox13.Enabled = true;
                    buttonSave.Enabled = true;
                    buttonFingerReg.Enabled = true;
                    buttonSearch.Enabled = false;
                    buttonBrowsePhoto.Enabled = true;
                    textBox10.Enabled = false;
                    buttonNew.Enabled = false;
                    buttonDelete.Enabled = true;

                }
            }

        }


        private void button_Delete_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM EmployeeDetails WHERE Id = @id";
            MySqlCommand cmd1 = new MySqlCommand(query, conn);
            cmd1.Parameters.AddWithValue("@id", textBox10.Text);

            int result = cmd1.ExecuteNonQuery();

            if (result > 0)
            {
                MessageBox.Show("Employee successfully deleted");
            }
            else
            {
                MessageBox.Show("Error deleting employee");
            }

            cleanup();

        }

        private void button_FingerReg_Click(object sender, EventArgs e)
        {

        }


        //public static Bitmap ByteToImage(byte[] blob)
        //{
        //    MemoryStream mStream = new MemoryStream();
        //    byte[] pData = blob;
        //    mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
        //    Bitmap bm = new Bitmap(mStream, false);
        //    mStream.Dispose();
        //    return bm;
        //}

       

    }
        
    }
    


