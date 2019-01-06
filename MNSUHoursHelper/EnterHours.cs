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
        private String username;
        private String password;
        private IWebDriver Driver;
        private Dictionary<int, bool> daysWorked = new Dictionary<int, bool>();
        private readonly bool fullTime;

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

        public EnterHours(String username, String password, Dictionary<int, bool> daysWorked, bool fullTime)
        {
            this.username = username;
            this.password = password;
            this.daysWorked = daysWorked;
            this.fullTime = fullTime;

            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;

            ChromeOptions options = new ChromeOptions();
            options.LeaveBrowserRunning = true;
            options.AddArgument("--incognito");

            Driver = new ChromeDriver(chromeDriverService, options);
            Driver.Navigate().GoToUrl("https://www.mnsu.edu/eservices/");

            this.Main();
        }

        private void Main()
        {
            this.LogIn();
        }

        public void LogIn()
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
                Driver.FindElement(By.XPath(logInErrorMessage));
                Driver.Dispose();
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
                finally
                {
                    this.AddHours();
                }
            }
        }

        private void AddHours()
        {
            Driver.FindElement(By.XPath(studentEmploymentLink)).Click();
            Driver.FindElement(By.XPath(enterTimeWorkedLink)).Click();

            for (int index = 1; index <= 14; index++)
            {
                if (daysWorked[index - 1])
                {
                    Driver.FindElement(By.XPath(addTimeButton)).Click();
                    Driver.FindElement(By.XPath(dateSelection)).Click();

                    Driver.FindElement(By.XPath("//*[@id='date']/option[" + index.ToString() + "]")).Click();

                    Driver.FindElement(By.XPath(startTimeSelection)).Click();
                    Driver.FindElement(By.XPath("//*[@id='startTime']/option[10]")).Click();
                    Driver.FindElement(By.XPath(endTimeSelection)).Click();

                    if (fullTime)
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
    }
}
