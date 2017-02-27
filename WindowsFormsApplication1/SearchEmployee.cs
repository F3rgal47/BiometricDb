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
            con.ConnectionString = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\FERGAL O NEILL\\Documents\\Fergal Final Year Folder\\Software Engineering Project\\BiometricDb\\BiometricDb\\WindowsFormsApplication1\\InD.mdf;Integrated Security=True";
            con.Open();



            using (var cmd = new SqlCommand("Select * from EmployeeDetails where Id = @id OR Forename like '" + textBox1.Text + "%' " + "OR Surname like '" + textBox2.Text + "%'", con))
           
            {
                cmd.Parameters.AddWithValue("@id", textBox10.Text);
                //cmd.Parameters.AddWithValue("@forename", textBox1.Text);
                //cmd.Parameters.AddWithValue("@surname", textBox2.Text);


                SqlDataAdapter daEmployee = new SqlDataAdapter(cmd);
                DataSet dsEmployeeSearch = new DataSet("employeeSearch");
                daEmployee.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                daEmployee.Fill(dsEmployeeSearch, "EmployeeDetails");

                DataTable tblEmployeeDetails;
                tblEmployeeDetails = dsEmployeeSearch.Tables["EmployeeDetails"];


            dataGridView1.Columns.Add("EmpID", "ID");
            dataGridView1.Columns.Add("FirstName", "FirstName");
            dataGridView1.Columns.Add("LastName", "LastName");
            dataGridView1.Columns.Add("email", "email");
            
            int row = dsEmployeeSearch.Tables["EmployeeDetails"].Rows.Count -1;

            for (int r = 0; r <= row; r++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[r].Cells[0].Value = dsEmployeeSearch.Tables["EmployeeDetails"].Rows[r].ItemArray[0];
                dataGridView1.Rows[r].Cells[1].Value = dsEmployeeSearch.Tables["EmployeeDetails"].Rows[r].ItemArray[1];
                dataGridView1.Rows[r].Cells[2].Value = dsEmployeeSearch.Tables["EmployeeDetails"].Rows[r].ItemArray[2];
                dataGridView1.Rows[r].Cells[3].Value = dsEmployeeSearch.Tables["EmployeeDetails"].Rows[r].ItemArray[3];

            }
        
            }
        }

    }
}
