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
        int accessLevel;
        System.Data.SqlClient.SqlConnection con;
        public Terminal()
        {
            InitializeComponent();
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
            con.ConnectionString = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\FERGAL O NEILL\\Documents\\Fergal Final Year Folder\\Software Engineering Project\\BiometricDb\\BiometricDb\\WindowsFormsApplication1\\InD.mdf;Integrated Security=True";
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
                accessLevel = checkAccess(accessLevel);

                int EmployeeAccesLevel = 0;

                if (textBox4.Text == "1")
                {
                    EmployeeAccesLevel = 1;
                }
                else if (textBox4.Text == "2")
                {
                    EmployeeAccesLevel = 2;
                }
                else if (textBox4.Text == "3")
                {
                    EmployeeAccesLevel = 3;
                }

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
           
            cleanUp();
        }

        

        int checkAccess(int x)
        {
            
            if (comboBox1.SelectedItem == "Foyer")
            {
                return 1;
            }
            else if (comboBox1.SelectedItem == "Server Room")
            {
                return 3;
            }
            else if (comboBox1.SelectedItem == "Canteen")
            {
                return 1;
            }
            else if (comboBox1.SelectedItem == "Manager Lounge")
            {
                return 2;
            }
            else if (comboBox1.SelectedItem == "Supplies Room")
            {
                return 2;
            }
            else return 1;
        }

        public async void cleanUp()
        {
            await Task.Delay(5000);
            textBox10.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            comboBox1.Text = "";
            textBox5.Text = "";
            textBox5.BackColor = Color.White;
            textBox10.Enabled = true;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            pictureBox1.Image = null;
            


        }


    }
}
