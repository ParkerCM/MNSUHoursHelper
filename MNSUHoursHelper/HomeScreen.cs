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
        int debugModeCount = 0;
        private bool fullTime = false;

        public HomeScreen()
        {
            InitializeComponent();
            debugCredent.Hide();
            deleteHoursBtn.Hide();
        }

        /// <summary>
        /// Begin selenium and pass it the settings provided by the user (or default values)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButtonClicked(object sender, EventArgs e)
        {
            // Verify username and password have been entered
            if (usernameTextBox.Text == "" || passwordTextBox.Text == "")
            {
                MessageBox.Show("You must enter your StarID and password");
            }
            else
            {
                var bigData = HoursSettingsHandler.GetDaysAndFullTime();

                EnterHours enterHours = new EnterHours(usernameTextBox.Text, passwordTextBox.Text, bigData.Item1, fullTime, bigData.Item2);
                bool success = enterHours.Add();

                if (!success)
                {
                    MessageBox.Show("Error. Please verify StarID and password are correct and try again");
                }
            }
        }

        /// <summary>
        /// Open settings page and save the information given to that page when it is saved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsButtonClicked(object sender, EventArgs e)
        {
            var bigData = HoursSettingsHandler.GetDaysAndFullTime();

            using (SettingsPage settingsPage = new SettingsPage(bigData.Item1, fullTime, bigData.Item2))
            {
                if (settingsPage.ShowDialog() == DialogResult.OK)
                {
                    this.fullTime = settingsPage.FullTime;
                }
            }
        }

        /// <summary>
        /// Adds provided credentials to textboxes to quickly be able to log in to eservices
        /// Useful when debugging selenium portion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DebugCredentialsClicked(object sender, EventArgs e)
        {
            usernameTextBox.Text = "pr4715zm";
            passwordTextBox.Text = "N0rm@nD@C@t";
        }

        /// <summary>
        /// Deletes the hours for the provided username and password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteHoursButtonClicked(object sender, EventArgs e)
        {
            var bigData = HoursSettingsHandler.GetDaysAndFullTime();

            EnterHours deleteHours = new EnterHours(usernameTextBox.Text, passwordTextBox.Text, bigData.Item1, fullTime, bigData.Item2);
            deleteHours.Delete();
        }

        /// <summary>
        /// Clicking the starid label five times will enabled debug mode
        /// Entering this mode will unhide two buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UsernameLabelClicked(object sender, EventArgs e)
        {
            debugModeCount++;

            if (debugModeCount >= 5)
            {
                deleteHoursBtn.Show();
                debugCredent.Show();
            }
        }

        /// <summary>
        /// Delete session settings file before closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HomeScreenFormClosing(object sender, FormClosingEventArgs e)
        {
            HoursSettingsHandler.DeleteSettingsFile(0);
        }
    }
}
