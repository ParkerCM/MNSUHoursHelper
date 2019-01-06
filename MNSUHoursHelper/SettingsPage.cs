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
        private Dictionary<int, bool> daysSelected = new Dictionary<int, bool>()
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

        public SettingsPage()
        {
            InitializeComponent();
            SetCurrentPayPeriod();
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
            CheckBox[] daysCheckBoxes = new CheckBox[] { day1Checkbox, day2Checkbox, day3Checkbox, day4Checkbox, day5Checkbox, day6Checkbox, day7Checkbox, day8Checkbox, day9Checkbox, day10Checkbox };
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

        private void SettingsPage_Load(object sender, EventArgs e)
        {

        }
    }
}
