﻿using System;
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
        private bool[] daysWorked = new bool[] { true, true, true, true, true, true, true, true, true, true };
        private bool[] partTimeHours = new bool[] { true, true, true, true, true, true, true, true, true, true };

        private bool fullTime = false;

        public HomeScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Begin selenium and pass it the settings provided by the user (or default values)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void submitButton_Click(object sender, EventArgs e)
        {
            // Verify username and password have been entered
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

        /// <summary>
        /// Open settings page and save the information given to that page when it is saved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingsButton_Click(object sender, EventArgs e)
        {
            using (SettingsPage settingsPage = new SettingsPage(daysWorked, fullTime, partTimeHours))
            {
                if (settingsPage.ShowDialog() == DialogResult.OK)
                {
                    this.daysWorked = settingsPage.DaysSelected;
                    this.fullTime = settingsPage.FullTime;
                    this.partTimeHours = settingsPage.PartTimeDays;
                }
            }
        }

        /// <summary>
        /// Adds provided credentials to textboxes to quickly be able to log in to eservices
        /// Useful when debugging selenium portion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void debugCredent_Click(object sender, EventArgs e)
        {
            usernameTextBox.Text = "pr4715zm";
            passwordTextBox.Text = "";
        }
    }
}
