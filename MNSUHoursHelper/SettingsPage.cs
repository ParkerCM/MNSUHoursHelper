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
        private Dictionary<int, bool> daysSelected = new Dictionary<int, bool>();
        private CheckBox[] daysCheckBoxes = new CheckBox[10];

        public SettingsPage(Dictionary<int, bool> daysWorked)
        {
            this.daysSelected = daysWorked;
            
            InitializeComponent();
            SetUpCheckboxArray();
            CheckUncheckBoxes();
            SetCurrentPayPeriod();
            AddDateToCheckboxes();
        }

        private void CheckUncheckBoxes()
        {
            int altIndex;

            for (int index = 0; index < daysCheckBoxes.Length; index++)
            {
                altIndex = index;
                
                if (index >= 8)
                {
                    altIndex = index + 4;
                }
                else if (index >= 3)
                {
                    altIndex = index + 2;
                }

                if (!daysSelected[altIndex])
                {
                    daysCheckBoxes[index].Checked = false;
                }
                else
                {
                    daysCheckBoxes[index].Checked = true;
                }
            }
        }

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

        private void saveButton_Click(object sender, EventArgs e)
        {
            //CheckBox[] daysCheckBoxes = new CheckBox[] { day1Checkbox, day2Checkbox, day3Checkbox, day4Checkbox, day5Checkbox, day6Checkbox, day7Checkbox, day8Checkbox, day9Checkbox, day10Checkbox };
            int altIndex;

            for (int index = 0; index < daysCheckBoxes.Length; index++)
            {
                altIndex = index;

                if (index >= 8)
                {
                    altIndex = index + 4;
                }
                else if (index >= 3)
                {
                    altIndex = index + 2;
                }

                if (!daysCheckBoxes[index].Checked)
                {
                    this.daysSelected[altIndex] = false;
                }
                else
                {
                    this.daysSelected[altIndex] = true;
                }
            }

            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public Dictionary<int, bool> GetDaysWorked
        {
            get { return this.daysSelected; }
        }

        public bool GetFullTime
        {
            get { return fullTimeCheckbox.Checked; }
        }

        private void AddDateToCheckboxes()
        {
            //CheckBox[] daysCheckBoxes = new CheckBox[] { day1Checkbox, day2Checkbox, day3Checkbox, day4Checkbox, day5Checkbox, day6Checkbox, day7Checkbox, day8Checkbox, day9Checkbox, day10Checkbox };
            int altIndex;

            for (int index = 0; index < daysCheckBoxes.Length; index++)
            {
                altIndex = index;
                if (index >= 8)
                {
                    altIndex += 4;
                }
                else if (index >= 3)
                {
                    altIndex += 2;
                }

                daysCheckBoxes[index].Text += " " + startOfPayPeriod.AddDays(altIndex).Month.ToString() + "/" + startOfPayPeriod.AddDays(altIndex).Day.ToString();
            }
        }

        private void SetUpCheckboxArray()
        {
            daysCheckBoxes[0] = day1Checkbox;
            daysCheckBoxes[1] = day2Checkbox;
            daysCheckBoxes[2] = day3Checkbox;
            daysCheckBoxes[3] = day4Checkbox;
            daysCheckBoxes[4] = day5Checkbox;
            daysCheckBoxes[5] = day6Checkbox;
            daysCheckBoxes[6] = day7Checkbox;
            daysCheckBoxes[7] = day8Checkbox;
            daysCheckBoxes[8] = day9Checkbox;
            daysCheckBoxes[9] = day10Checkbox;
        }
    }
}
