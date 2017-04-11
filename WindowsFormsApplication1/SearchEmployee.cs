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
        public static string employeeId;
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
            gridCleanUp();
            con = new System.Data.SqlClient.SqlConnection();
            con.ConnectionString = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\FERGAL O NEILL\\Documents\\Fergal Final Year Folder\\Software Engineering Project\\BiometricDb\\BiometricDb\\WindowsFormsApplication1\\InD.mdf;Integrated Security=True";
            con.Open();
            String commandString = createCommand();

          
            using (var cmd = new SqlCommand(commandString, con))
           
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


            
            int row = dsEmployeeSearch.Tables["EmployeeDetails"].Rows.Count -1;

            for (int r = 0; r <= row; r++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[r].Cells[0].Value = dsEmployeeSearch.Tables["EmployeeDetails"].Rows[r].ItemArray[0];
                dataGridView1.Rows[r].Cells[1].Value = dsEmployeeSearch.Tables["EmployeeDetails"].Rows[r].ItemArray[1];
                dataGridView1.Rows[r].Cells[2].Value = dsEmployeeSearch.Tables["EmployeeDetails"].Rows[r].ItemArray[2];
                dataGridView1.Rows[r].Cells[3].Value = dsEmployeeSearch.Tables["EmployeeDetails"].Rows[r].ItemArray[3];

            }
            button3.Enabled = true;
           

            }
        } 

        private void button3_Click(object sender, EventArgs e)
        {
            employeeId = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            this.Visible = false;
            EmployeeMaintenance SearchEmployeeForm = new EmployeeMaintenance();
            SearchEmployeeForm.ShowDialog();
            this.Visible = true;
        }

        public void gridCleanUp()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Rows.Clear();

        }

        public string createCommand()
        {
                string commandString;

                if (textBox1.Text != "" && textBox2.Text == "")
                {
                  commandString = "SELECT * FROM EmployeeDetails WHERE Id = @id OR Forename like '" + textBox1.Text + "%' ";
                  return commandString;
                }
                else if (textBox1.Text == "" && textBox2.Text != "")
                {
                    commandString = "SELECT * FROM EmployeeDetails WHERE Id = @id OR Surname like '" + textBox2.Text + "%' ";
                  return commandString;
                }
                else
                {
                     commandString = "SELECT * FROM EmployeeDetails WHERE Id = @id OR Forename like '" + textBox1.Text + "%' AND Surname like '" + textBox2.Text + "%' ";
                  return commandString;
                }

        }

    }
}
