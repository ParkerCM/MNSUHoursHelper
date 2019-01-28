using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Threading.Tasks;

namespace MNSUHoursHelper
{
    public static class HoursSettingsHandler
    {

        ///////////////////////////////////////////////////////////
        /// Private Methods                                     ///
        ///////////////////////////////////////////////////////////

        /// <summary>
        /// Creates the default settings xml
        /// </summary>
        private static void CreateDefaultSettingsFile()
        {
            var settingsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/MNSUHoursHelperSettings";

            new XDocument(new XElement("Root",
                new XElement("Days",
                new XElement("Wednesday", true),
                new XElement("Thursday", true),
                new XElement("Friday", true),
                new XElement("Monday", true),
                new XElement("Tuesday", true),
                new XElement("Wednesday", true),
                new XElement("Thursday", true),
                new XElement("Friday", true),
                new XElement("Monday", true),
                new XElement("Tuesday", true)),
                new XElement("FullTime",
                new XElement("Wednesday", true),
                new XElement("Thursday", true),
                new XElement("Friday", true),
                new XElement("Monday", true),
                new XElement("Tuesday", true),
                new XElement("Wednesday", true),
                new XElement("Thursday", true),
                new XElement("Friday", true),
                new XElement("Monday", true),
                new XElement("Tuesday", true))))
                .Save(settingsLocation + "/DefaultSettings.xml");
        }

        /// <summary>
        /// Saves settings selection to xml
        /// </summary>
        /// <param name="daysWorked">Days selected</param>
        /// <param name="fullTime">Is the user working full time</param>
        /// <param name="file">0 = Session Settings; 1 = User Default</param>
        private static void SaveSettings(bool[] daysWorked, bool[] fullTime, int file)
        {
            var settingsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/MNSUHoursHelperSettings";
            string[] fileNames = new string[] { "/CurrentUserSettings.xml", "/UserDefaultSettings.xml" };

            new XDocument(new XElement("Root",
                new XElement("Days",
                new XElement("Wednesday", daysWorked[0]),
                new XElement("Thursday", daysWorked[1]),
                new XElement("Friday", daysWorked[2]),
                new XElement("Monday", daysWorked[3]),
                new XElement("Tuesday", daysWorked[4]),
                new XElement("Wednesday", daysWorked[5]),
                new XElement("Thursday", daysWorked[6]),
                new XElement("Friday", daysWorked[7]),
                new XElement("Monday", daysWorked[8]),
                new XElement("Tuesday", daysWorked[9])),
                new XElement("FullTime",
                new XElement("Wednesday", fullTime[0]),
                new XElement("Thursday", fullTime[1]),
                new XElement("Friday", fullTime[2]),
                new XElement("Monday", fullTime[3]),
                new XElement("Tuesday", fullTime[4]),
                new XElement("Wednesday", fullTime[5]),
                new XElement("Thursday", fullTime[6]),
                new XElement("Friday", fullTime[7]),
                new XElement("Monday", fullTime[8]),
                new XElement("Tuesday", fullTime[9]))))
                .Save(settingsLocation + fileNames[file]);
        }

        /// <summary>
        /// Determines which settings should be loaded
        /// </summary>
        /// <returns>0 = Session Settings; 1 = User Default; 2 = Program Default</returns>
        private static int DetermineWhichSettingsToUse()
        {
            var settingsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/MNSUHoursHelperSettings";

            if (File.Exists(settingsLocation + "/CurrentUserSettings.xml"))
            {
                return 0;
            }
            else if (File.Exists(settingsLocation + "/UserDefaultSettings.xml"))
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        /// <summary>
        /// Converts a string to a bool
        /// </summary>
        /// <param name="value">String to convert</param>
        /// <returns>A bool version of the string</returns>
        private static bool CastStringToBool(string value)
        {
            if (value == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes the session or user default settings file
        /// </summary>
        /// <param name="file">0 = Session Settings; 1 = User Default</param>
        private static void DeleteSettingsFile(int file)
        {
            var settingsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/MNSUHoursHelperSettings";

            if (file == 0)
            {
                settingsLocation += "/CurrentUserSettings.xml";
            }
            else if (file == 1)
            {
                settingsLocation += "/UserDefaultSettings.xml";
            }

            if (File.Exists(settingsLocation))
            {
                File.Delete(settingsLocation);
            }
        }

        ///////////////////////////////////////////////////////////
        /// Public Methods                                      ///
        ///////////////////////////////////////////////////////////

        /// <summary>
        /// Check if the settings folder exists
        /// </summary>
        /// <returns>True if settings directory exists. False otherwise</returns>
        public static void DoDefaultSettingsExist()
        {
            var settingsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/MNSUHoursHelperSettings";

            // Check if settings directory exists
            if (Directory.Exists(settingsLocation))
            {
                // Directory exists. See if settings file exists
                if (!File.Exists(settingsLocation + "DefaultSettings.xml"))
                {
                    CreateDefaultSettingsFile();
                }
            }
            // Directory does not exist. Create one in documents folder
            else
            {
                Directory.CreateDirectory(settingsLocation);
                CreateDefaultSettingsFile();
            }
        }

        /// <summary>
        /// Gets days worked and full time from an xml. Automatically picks a file if no parameters are given
        /// </summary>
        /// <param name="forceSelection">0 = Session; 1 = User Default; 2 = Default</param>
        /// <returns>A tuple with days as the first item and full time status as the second</returns>
        public static Tuple<bool[], bool[]> GetDaysAndFullTime(int forceSelection = -1)
        {
            var settingsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/MNSUHoursHelperSettings";
            String[] choices = new String[] { "/CurrentUserSettings.xml", "/UserDefaultSettings.xml", "/DefaultSettings.xml" };
            int choice;

            if (forceSelection == -1)
            {
                choice = DetermineWhichSettingsToUse();
            }
            else
            {
                choice = forceSelection;
            }

            bool[] dayData = new bool[10];
            bool[] fullTimeData = new bool[10];

            XmlDocument doc = new XmlDocument();

            doc.Load(settingsLocation + choices[choice]);

            XmlNodeList days = doc.GetElementsByTagName("Days");
            XmlNodeList fullTime = doc.GetElementsByTagName("FullTime");
            XmlNodeList dayNodes = days[0].ChildNodes;
            XmlNodeList fullTimeNodes = fullTime[0].ChildNodes;

            for (int index = 0; index < 10; index++)
            {
                dayData[index] = CastStringToBool(dayNodes[index].InnerText);
                fullTimeData[index] = CastStringToBool(fullTimeNodes[index].InnerText);
            }

            return Tuple.Create(dayData, fullTimeData);
        }

        /// <summary>
        /// Delete the user's default settings file
        /// </summary>
        public static void DeleteUserDefaultSettingsFile()
        {
            DeleteSettingsFile(1);
        }

        /// <summary>
        /// Deletes the users current session settings file
        /// </summary>
        public static void DeleteSessionSettingsFile()
        {
            DeleteSettingsFile(0);
        }

        /// <summary>
        /// Saves user default settings to the xml file
        /// </summary>
        /// <param name="daysWorked">Days which have been worked in a bool array</param>
        /// <param name="fullTime">Whether the day was full time or part time in a bool array</param>
        public static void SaveUserDefaultSettings(bool[] daysWorked, bool[] fullTime)
        {
            SaveSettings(daysWorked, fullTime, 1);
        }

        /// <summary>
        /// Saves session settings to the xml file
        /// </summary>
        /// <param name="daysWorked">Days which have been worked in a bool array</param>
        /// <param name="fullTime">Whether the day was full time or part time in a bool array</param>
        public static void SaveSessionSettings(bool[] daysWorked, bool[] fullTime)
        {
            SaveSettings(daysWorked, fullTime, 0);
        }
    }
}
