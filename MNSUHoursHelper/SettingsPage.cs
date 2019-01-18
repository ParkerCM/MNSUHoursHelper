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
    public partial class SettingsPage : Form
    {
        private DateTime startOfPayPeriod;
        private DateTime endOfPayPeriod;
        private CheckBox[] daysCheckBoxes = new CheckBox[10];
        private CheckBox[] partTimeCheckBoxes = new CheckBox[10];

        public bool[] DaysSelected { get; set; } = new bool[10];

        public bool[] PartTimeDays { get; set; } = new bool[10];

        public bool FullTime
        {
            get { return fullTimeRadio.Checked; }
            set { fullTimeRadio.Checked = value; }
        }

        public SettingsPage(bool[] daysWorked, bool fullTime, bool[] partTimeHours)
        {
            InitializeComponent();

            DaysSelected = daysWorked;
            FullTime = fullTime;
            PartTimeDays = partTimeHours;
            
            daysCheckBoxes = SetUpCheckboxArray(daysCheckBoxes);
            partTimeCheckBoxes = SetUpHoursCheckboxArray(partTimeCheckBoxes);
            CheckUncheckBoxes(daysCheckBoxes, DaysSelected, partTimeCheckBoxes, PartTimeDays);
            SetCurrentPayPeriod();
            AddDateToCheckboxes(daysCheckBoxes, startOfPayPeriod);
        }

        /// <summary>
        /// Determines current pay period based on the current date
        /// Highlights the range on the calendar and adds the date range to the pay period label
        /// </summary>
        private void SetCurrentPayPeriod()
        {
            DateTime endOfAPayPeriod = new DateTime(2018, 12, 25);
            DateTime currentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            //DateTime currentTime = new DateTime(2019, 1, 20);

            while (currentTime > endOfAPayPeriod)
            {
                endOfAPayPeriod = endOfAPayPeriod.AddDays(14);
            }

            this.startOfPayPeriod = endOfAPayPeriod.Subtract(new TimeSpan(13, 0, 0, 0));
            this.endOfPayPeriod = endOfAPayPeriod;

            // Set pay period label
            payPeriodLabel.Text = "Current pay period is: " + startOfPayPeriod.Month.ToString() + "/" 
                + startOfPayPeriod.Day.ToString() + "/" + startOfPayPeriod.Year.ToString() + " - " 
                + endOfPayPeriod.Month.ToString() + "/" + endOfPayPeriod.Day.ToString() + "/" 
                + endOfPayPeriod.Year.ToString();

            // Highlight pay period on the calendar
            calendarWidget.SelectionStart = this.startOfPayPeriod;
            calendarWidget.SelectionEnd = this.endOfPayPeriod;
        }

        private void SaveCurrentSettings()
        {
            for (int index = 0; index < daysCheckBoxes.Length; index++)
            {
                // Save day checkboxes
                if (!daysCheckBoxes[index].Checked)
                {
                    DaysSelected[index] = false;
                }
                else
                {
                    DaysSelected[index] = true;
                }

                // Save hour checkboxes
                if (!partTimeCheckBoxes[index].Checked)
                {
                    PartTimeDays[index] = false;
                }
                else
                {
                    PartTimeDays[index] = true;
                }
            }
        }

        private void makeDefaultButton_Click(object sender, EventArgs e)
        {
            SaveCurrentSettings();

            DefaultSettingsHandler.CreateDefaultSettingsFile(DaysSelected, PartTimeDays);
        }

        /*<----- Checkbox setup and formatting ----->*/

        /// <summary>
        /// Loads saved checkbox settings and applies them to the page by enabling/disabling the boxes.
        /// Default format is where every box is checked.
        /// </summary>
        /// <param name="boxes"></param>
        /// <param name="daysWorking"></param>
        private void CheckUncheckBoxes(CheckBox[] daysBoxes, bool[] daysWorking, CheckBox[] hoursBoxes, bool[] partTime)
        {
            // Days of the week
            for (int index = 0; index < daysBoxes.Length; index++)
            {
                if (!daysWorking[index])
                {
                    daysBoxes[index].Checked = false;
                }
                else
                {
                    daysBoxes[index].Checked = true;
                }

                // Part time boxes
                if (!partTime[index])
                {
                    hoursBoxes[index].Checked = false;
                }
                else
                {
                    hoursBoxes[index].Checked = true;
                }
            }
        }

        /// <summary>
        /// Addes respective data in MM/DD format to the checkbox label
        /// </summary>
        /// <param name="boxes">Array of all day checkboxes on the page</param>
        private void AddDateToCheckboxes(CheckBox[] boxes, DateTime payPeriod)
        {
            for (int index = 0; index < boxes.Length; index++)
            {
                int offsetIndex = index;

                if (index >= 8)
                {
                    offsetIndex = index + 4;
                }
                else if (index >= 3)
                {
                    offsetIndex = index + 2;
                }

                boxes[index].Text += " " + payPeriod.AddDays(offsetIndex).Month.ToString() + "/" + payPeriod.AddDays(offsetIndex).Day.ToString();
            }
        }

        /// <summary>
        /// Add checkboxes to the checkbox array
        /// </summary>
        /// <param name="boxes">Empty array for holding checkboxes</param>
        /// <returns>A checkbox array with all checkboxes on the form</returns>
        private CheckBox[] SetUpCheckboxArray(CheckBox[] boxes)
        {
            boxes[0] = day1Checkbox;
            boxes[1] = day2Checkbox;
            boxes[2] = day3Checkbox;
            boxes[3] = day4Checkbox;
            boxes[4] = day5Checkbox;
            boxes[5] = day6Checkbox;
            boxes[6] = day7Checkbox;
            boxes[7] = day8Checkbox;
            boxes[8] = day9Checkbox;
            boxes[9] = day10Checkbox;

            return boxes;
        }

        /// <summary>
        /// Adds part time checkbox to the arrary
        /// </summary>
        /// <param name="boxes">Empty array for holding checkboxes</param>
        /// <returns>An array with all part time checkboxes</returns>
        private CheckBox[] SetUpHoursCheckboxArray(CheckBox[] boxes)
        {
            boxes[0] = day1PartTime;
            boxes[1] = day2PartTime;
            boxes[2] = day3PartTime;
            boxes[3] = day4PartTime;
            boxes[4] = day5PartTime;
            boxes[5] = day6PartTime;
            boxes[6] = day7PartTime;
            boxes[7] = day8PartTime;
            boxes[8] = day9PartTime;
            boxes[9] = day10PartTime;

            return boxes;
        }

        /*<----- Button Actions ----->*/

        /// <summary>
        /// Logic for clicking the save button. Selected days of the week are saved and sent to the home screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveCurrentSettings();

            // Send back OK result so the form data can be transfered back to home screen
            DialogResult = DialogResult.OK;
        }
        
        /// <summary>
        /// Logic for clicking cancel. Closes settings page without saving data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Checks all part time boxes if part time is selected. Deselects when full time is selected
        /// </summary>
        private void partTimeRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (partTimeRadio.Checked)
            {
                foreach (CheckBox box in partTimeCheckBoxes)
                {
                    box.Checked = true;
                }
            }
            else
            {
                foreach (CheckBox box in partTimeCheckBoxes)
                {
                    box.Checked = false;
                }
            }
        }

        // Below methods listen for a day of the week box to change state
        // If the box is unchecked, then the part time box will become unchecked and disabled
        // If the box is checked, then the part time box will become enabled

        private void day1Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (day1Checkbox.Checked)
            {
                day1PartTime.Enabled = true;
            }
            else
            {
                day1PartTime.Checked = false;
                day1PartTime.Enabled = false;
            }
        }

        private void day2Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (day2Checkbox.Checked)
            {
                day2PartTime.Enabled = true;
            }
            else
            {
                day2PartTime.Checked = false;
                day2PartTime.Enabled = false;
            }
        }

        private void day3Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (day3Checkbox.Checked)
            {
                day3PartTime.Enabled = true;
            }
            else
            {
                day3PartTime.Checked = false;
                day3PartTime.Enabled = false;
            }
        }

        private void day4Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (day4Checkbox.Checked)
            {
                day4PartTime.Enabled = true;
            }
            else
            {
                day4PartTime.Checked = false;
                day4PartTime.Enabled = false;
            }
        }

        private void day5Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (day5Checkbox.Checked)
            {
                day5PartTime.Enabled = true;
            }
            else
            {
                day5PartTime.Checked = false;
                day5PartTime.Enabled = false;
            }
        }

        private void day6Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (day6Checkbox.Checked)
            {
                day6PartTime.Enabled = true;
            }
            else
            {
                day6PartTime.Checked = false;
                day6PartTime.Enabled = false;
            }
        }

        private void day7Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (day7Checkbox.Checked)
            {
                day7PartTime.Enabled = true;
            }
            else
            {
                day7PartTime.Checked = false;
                day7PartTime.Enabled = false;
            }
        }

        private void day8Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (day8Checkbox.Checked)
            {
                day8PartTime.Enabled = true;
            }
            else
            {
                day8PartTime.Checked = false;
                day8PartTime.Enabled = false;
            }
        }

        private void day9Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (day9Checkbox.Checked)
            {
                day9PartTime.Enabled = true;
            }
            else
            {
                day9PartTime.Checked = false;
                day9PartTime.Enabled = false;
            }
        }

        private void day10Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (day10Checkbox.Checked)
            {
                day10PartTime.Enabled = true;
            }
            else
            {
                day10PartTime.Checked = false;
                day10PartTime.Enabled = false;
            }
        }
    }
}
