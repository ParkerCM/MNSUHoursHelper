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
        private bool[] daysSelected = new bool[10];
        private CheckBox[] daysCheckBoxes = new CheckBox[10];

        public bool[] DaysSelected
        {
            get { return daysSelected; }
            set { daysSelected = value; }
        }

        public bool FullTime
        {
            get { return fullTimeCheckbox.Checked; }
            set { fullTimeCheckbox.Checked = value; }
        }

        public SettingsPage(bool[] daysWorked, bool fullTime)
        {
            InitializeComponent();

            DaysSelected = daysWorked;
            FullTime = fullTime;
            
            daysCheckBoxes = SetUpCheckboxArray(daysCheckBoxes);
            CheckUncheckBoxes(daysCheckBoxes, daysSelected);
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

        /*<----- Checkbox setup and formatting ----->*/

        /// <summary>
        /// Loads saved checkbox settings and applies them to the page by enabling/disabling the boxes.
        /// Default format is where every box is checked.
        /// </summary>
        /// <param name="boxes"></param>
        /// <param name="daysWorking"></param>
        private void CheckUncheckBoxes(CheckBox[] boxes, bool[] daysWorking)
        {
            for (int index = 0; index < boxes.Length; index++)
            {
                if (!daysWorking[index])
                {
                    boxes[index].Checked = false;
                }
                else
                {
                    boxes[index].Checked = true;
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

        /*<----- Button Actions ----->*/

        /// <summary>
        /// Logic for clicking the save button. Selected days of the week are saved and sent to the home screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < daysCheckBoxes.Length; index++)
            {
                if (!daysCheckBoxes[index].Checked)
                {
                    DaysSelected[index] = false;
                }
                else
                {
                    DaysSelected[index] = true;
                }
            }
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
    }
}
