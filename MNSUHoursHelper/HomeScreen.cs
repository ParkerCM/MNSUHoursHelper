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

        private Dictionary<int, bool> daysWorked = new Dictionary<int, bool>()
        {
            {0, true },
            {1, true },
            {2, true },
            {3, false },
            {4, false },
            {5, true },
            {6, true },
            {7, true },
            {8, true },
            {9, true },
            {10, false },
            {11, false },
            {12, true },
            {13, true }
        };
        private bool fullTime = false;

    public HomeScreen()
        {
            InitializeComponent();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (usernameTextBox.Text == "" || passwordTextBox.Text == "")
            {
                MessageBox.Show("You must enter your StarID and password");
            }
            else
            {
                EnterHours enterHours = new EnterHours(usernameTextBox.Text, passwordTextBox.Text, daysWorked, fullTime);
                this.Close();
            }
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            //this.settingsPage.Show();

            using (SettingsPage settingsPage = new SettingsPage())
            {
                if (settingsPage.ShowDialog() == DialogResult.OK)
                {
                    this.daysWorked = settingsPage.GetDaysWorked;
                    this.fullTime = settingsPage.GetFullTime;
                }
            }
        }

        private void debugCredent_Click(object sender, EventArgs e)
        {
            // Add credentials here to make debugging selenium quicker
            usernameTextBox.Text = "pr4715zm";
            passwordTextBox.Text = "";
        }
    }
}
