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
        /// <summary>
        /// Check if the settings folder exists
        /// </summary>
        /// <returns>True if settings directory exists. False otherwise</returns>
        public static bool DoDefaultSettingsExist()
        {
            var settingsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/MNSUHoursHelperSettings";

            // Check if settings directory exists
            if (Directory.Exists(settingsLocation))
            {
                // Directory exists. See if settings file exists
                if (File.Exists(settingsLocation + "DefaultSettings.xml"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // Directory does not exist. Create one in documents folder
            else
            {
                Directory.CreateDirectory(settingsLocation);
                CreateDefaultSettingsFile();

                return false;
            }
        }

        /// <summary>
        /// Saves settings selection to xml
        /// </summary>
        /// <param name="daysWorked">Days selected</param>
        /// <param name="fullTime">Is the user working full time</param>
        /// <param name="file">0 = Session Settings; 1 = User Default</param>
        public static void SaveSettings(bool[] daysWorked, bool[] fullTime, int file)
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
                new XElement("Wednesday", false),
                new XElement("Thursday", false),
                new XElement("Friday", false),
                new XElement("Monday", false),
                new XElement("Tuesday", false),
                new XElement("Wednesday", false),
                new XElement("Thursday", false),
                new XElement("Friday", false),
                new XElement("Monday", false),
                new XElement("Tuesday", false))))
                .Save(settingsLocation + "/DefaultSettings.xml");
        }

        /// <summary>
        /// Deletes the session settings file before the form is closed
        /// </summary>
        /// <param name="file">0 = Session Settings; 1 = User Default</param>
        public static void DeleteSettingsFile(int file)
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

        public static Tuple<bool[], bool[]> GetDaysAndFullTime(int forceSelection = -1)
        {
            var settingsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/MNSUHoursHelperSettings";
            String[] choices = new String[] { "/CurrentUserSettings.xml", "/UserDefaultSettings.xml", "/DefaultSettings.xml"};
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

    }
}
