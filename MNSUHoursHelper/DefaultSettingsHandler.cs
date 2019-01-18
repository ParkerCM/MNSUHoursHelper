using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace MNSUHoursHelper
{
    public static class DefaultSettingsHandler
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

                return false;
            }
        }

        public static void CreateDefaultSettingsFile(bool[] daysWorked, bool[] fullTime)
        {
            var settingsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/MNSUHoursHelperSettings";

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
                .Save(settingsLocation + "/DefaultSettings.xml");
        }

        public static void DeleteSettingsFile()
        {

        }



    }
}
