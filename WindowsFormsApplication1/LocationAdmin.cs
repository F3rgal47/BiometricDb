using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiometricDb
{
    public partial class LocationAdmin : Form
    {
        public LocationAdmin()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            LocationMaintenance LocationMaintenanceForm = new LocationMaintenance();
            LocationMaintenanceForm.ShowDialog();
            this.Visible = true;
        }

        private void LocationAdmin_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AreaMaintenance AreaMaintenanceForm = new AreaMaintenance();
            AreaMaintenanceForm.ShowDialog();
            this.Visible = true;
        }

        private void LocationAdmin_Load_1(object sender, EventArgs e)
        {

        }
    }
}
