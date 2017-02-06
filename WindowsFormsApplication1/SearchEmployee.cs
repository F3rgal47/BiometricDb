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

namespace BiometricDb
{
    public partial class SearchEmployee : Form
    {
        System.Data.SqlClient.SqlConnection con;
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
            con = new System.Data.SqlClient.SqlConnection();
            con.ConnectionString = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\FERGAL O NEILL\\Documents\\Fergal Final Year Folder\\Software Engineering Project\\WindowsFormsApplication1\\WindowsFormsApplication1\\WindowsFormsApplication1\\InD.mdf;Integrated Security=True";
            con.Open();

            

            using (var cmd = new SqlCommand("select * from EmployeeDetails where Id = @id or Forename like '%@forname%'or Surname like '%@surname%'" , con))
            {
                cmd.Parameters.AddWithValue("@id", textBox10.Text);
                cmd.Parameters.AddWithValue("@forename", textBox1.Text);
                cmd.Parameters.AddWithValue("@surname", textBox2.Text);
                

                SqlDataAdapter daEmployee = new SqlDataAdapter(cmd);
                DataSet dsEmployeeSearch = new DataSet("employeeSearch");
                daEmployee.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                daEmployee.Fill(dsEmployeeSearch, "EmployeeDetails");

                
                this.Visible = false;
                SearchEmployee SearchEmployeeForm = new SearchEmployee();
                SearchEmployeeForm.ShowDialog();
                this.Visible = true;


            }
        }

        private void SearchEmployee_Load(object sender, EventArgs e)
        {

        }

    }
}
