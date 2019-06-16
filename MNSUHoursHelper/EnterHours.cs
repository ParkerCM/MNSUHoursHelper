using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MNSUHoursHelper
{
    class EnterHours
    {
        // Variables for use within this class
        private readonly String username;
        private readonly String password;
        private IWebDriver Driver;
        private readonly bool[] daysWorked = new bool[10];
        private readonly bool[] partTimeHours = new bool[10];

        // Log in screen
        private readonly String usernameField = "//*[@id='techid']";
        private readonly String passwordField = "//*[@id='pin']";
        private readonly String logInButton = "//*[@id='Submit']";
        private readonly String logInErrorMessage = "//*[@id='Job']/p[2]/span";
        private readonly String topCheckBox = "//*[@id='accept_tuition']";
        private readonly String bottomCheckBox = "//*[@id='understand_drop']";
        private readonly String continueButton = "//*[@id='Job']/p[3]/input";
        private readonly String estimatedBillContinue = "/html/body/div[5]/div/div/div[2]/div/form/p[2]/input";

        // Adding hours
        private readonly String studentEmploymentLink = "//*[@id='app-links']/ul/li[9]/a";
        private readonly String enterTimeWorkedLink = "//*[@id='main']/div[6]/a";
        private readonly String addTimeButton = "//*[@id='addTime']";
        private readonly String dateSelection = "//*[@id='date']";
        private readonly String startTimeSelection = "//*[@id='startTime']";
        private readonly String endTimeSelection = "//*[@id='endTime']";
        private readonly String saveTime = "//*[@id='timeSaveOrAddId']";
        private readonly String continueOnHolidayAlert = "//*[@id='continueId']";
        private readonly String deleteTimeButton = "//*[@id='deleteId']";

        /// <summary>
        /// Create webdriver and navigate to eservices. Collect user settings and store them
        /// </summary>
        /// <param name="username">StarID for user</param>
        /// <param name="password">Password for user</param>
        /// <param name="daysWorked">Dictionary of the days they worked this pay period</param>
        public EnterHours(String username, String password, bool[] daysWorked, bool[] partTimeHours)
        {
            this.username = username;
            this.password = password;
            this.daysWorked = daysWorked;
            this.partTimeHours = partTimeHours;
        }

        /// <summary>
        /// Debug method that deletes all entered hours
        /// </summary>
        public void Delete()
        {
            bool stillEntry = true;

            InitializeBrowser();
            LogIn();

            while (stillEntry)
            {
                try
                {
                    Driver.FindElement(By.XPath("//*[@id='date_0 }']")).Click();
                    Driver.FindElement(By.XPath(deleteTimeButton)).Click();
                }
                catch (NoSuchElementException)
                {
                    stillEntry = false;
                }
            }

            Driver.Dispose();
        }

        /// <summary>
        /// Calls methods in correct order. Used after the constructor
        /// </summary>
        public bool Add()
        {
            InitializeBrowser();
            bool success = LogIn();

            // If log in failed, then success will evaluate to false
            if (success)
            {
                AddHours();
            }
            else
            {
                return false;
            }

            Kill();

            return true;
        }

        /// <summary>
        /// Creates driver object and goes to eservices log in page
        /// </summary>
        private void InitializeBrowser()
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            ChromeOptions options = new ChromeOptions
            {
                LeaveBrowserRunning = true
            };
            options.AddArgument("--incognito");

            Driver = new ChromeDriver(chromeDriverService, options);
            Driver.Navigate().GoToUrl("https://www.mnsu.edu/eservices/");
        }

        /// <summary>
        /// Logs in to eservices with the provided username and password
        /// </summary>
        private bool LogIn()
        {
            var username = Driver.FindElement(By.XPath(usernameField));
            username.Click();
            username.SendKeys(this.username);

            var password = Driver.FindElement(By.XPath(passwordField));
            password.Click();
            password.SendKeys(this.password);

            var logIn = Driver.FindElement(By.XPath(logInButton));
            logIn.Click();

            try
            {
                // If credentials did not work close browser
                Driver.FindElement(By.XPath(logInErrorMessage));
                Driver.Dispose();

                return false;
            }
            catch (NoSuchElementException)
            {
                Driver.FindElement(By.XPath(topCheckBox)).Click();
                Driver.FindElement(By.XPath(bottomCheckBox)).Click();
                Driver.FindElement(By.XPath(continueButton)).Click();

                try
                {
                    Driver.FindElement(By.XPath(estimatedBillContinue)).Click();
                }
                catch (NoSuchElementException)
                {
               
                }

                Driver.FindElement(By.XPath(studentEmploymentLink)).Click();
                Driver.FindElement(By.XPath(enterTimeWorkedLink)).Click();

                return true;
            }
        }

        /// <summary>
        /// Adds hours into eservices based on the days worked and whether the user is fulltime
        /// </summary>
        private void AddHours()
        {
            for (int index = 0; index < daysWorked.Length; index++)
            {
                if (daysWorked[index])
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

                    Driver.FindElement(By.XPath(addTimeButton)).Click();
                    Driver.FindElement(By.XPath(dateSelection)).Click();

                    offsetIndex++;
                    Driver.FindElement(By.XPath("//*[@id='date']/option[" + offsetIndex.ToString() + "]")).Click();

                    Driver.FindElement(By.XPath(startTimeSelection)).Click();
                    Driver.FindElement(By.XPath("//*[@id='startTime']/option[10]")).Click();
                    Driver.FindElement(By.XPath(endTimeSelection)).Click();

                    if (!partTimeHours[index])
                    {
                        Driver.FindElement(By.XPath("//*[@id='endTime']/option[32]")).Click();
                    }
                    else
                    {
                        Driver.FindElement(By.XPath("//*[@id='endTime']/option[16]")).Click();
                    }

                    Driver.FindElement(By.XPath(saveTime)).Click();

                    try
                    {
                        Driver.FindElement(By.XPath(continueOnHolidayAlert)).Click();
                    }
                    catch (NoSuchElementException)
                    {
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Waits for the user to close Chrome window and then disposes the driver
        /// </summary>
        public void Kill()
        {
            while (true)
            {
                try
                {
                    // Keep trying to get window title. Will throw an error when the window closes
                    var validator = Driver.CurrentWindowHandle;
                    System.Threading.Thread.Sleep(100);
                }
                catch (Exception)
                {
                    break;
                }
            }
            Driver.Dispose();
        }
    }
}
