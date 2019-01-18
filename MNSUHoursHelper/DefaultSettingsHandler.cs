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

        public static void CreateDefaultSettingsFile()
        {
            var settingsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/MNSUHoursHelperSettings";

            new XDocument(new XElement("Root",
                new XElement("Days",
                new XElement("Wednesday", "test"),
                new XElement("Thursday", "test"),
                new XElement("Friday", "test"),
                new XElement("Monday", "test"),
                new XElement("Tuesday", "test"),
                new XElement("Wednesday", "test"),
                new XElement("Thursday", "test"),
                new XElement("Friday", "test"),
                new XElement("Monday", "test"),
                new XElement("Tuesday", "test")),
                new XElement("FullTime",
                new XElement("Wednesday", "test"),
                new XElement("Thursday", "test"),
                new XElement("Friday", "test"),
                new XElement("Monday", "test"),
                new XElement("Tuesday", "test"),
                new XElement("Wednesday", "test"),
                new XElement("Thursday", "test"),
                new XElement("Friday", "test"),
                new XElement("Monday", "test"),
                new XElement("Tuesday", "test"))))
                .Save(settingsLocation + "/DefaultSettings.xml");
        }

        public static void DeleteSettingsFile()
        {

        }



    }
}
