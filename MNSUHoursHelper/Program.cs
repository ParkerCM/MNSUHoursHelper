using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MNSUHoursHelper
{
    static class Home
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            HoursSettingsHandler.DoDefaultSettingsExist();
            HoursSettingsHandler.DeleteSessionSettingsFile();
            Application.Run(new HomeScreen());
        }
    }
}
