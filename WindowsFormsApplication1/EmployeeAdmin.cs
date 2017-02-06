using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class EmployeeAdmin : Form
    {
        public EmployeeAdmin()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            NewEmployee newEmployeeForm = new NewEmployee();
            newEmployeeForm.ShowDialog();
            this.Visible = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            SearchEmployee SearchEmployeeForm = new SearchEmployee();
            SearchEmployeeForm.ShowDialog();
            this.Visible = true;
        }
    }
}
