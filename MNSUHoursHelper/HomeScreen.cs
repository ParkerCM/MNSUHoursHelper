using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MNSUHoursHelper
{
    public partial class HomeScreen : Form
    {
        private SettingsPage settingsPage;

        public HomeScreen()
        {
            InitializeComponent();
            this.settingsPage = new SettingsPage();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            this.settingsPage.Show();
        }
    }
}
