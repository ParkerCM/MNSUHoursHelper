using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNSUHoursHelper
{
    public static class DefaultSettingsHandler
    {
        public static bool DoDefaultSettingsExist()
        {
            if (System.IO.Directory.Exists("C:/MNSUHoursHelper"))
            {
                Console.WriteLine("success");
                return true;
            }
            else
            {
                Console.WriteLine("failure");
                return false;
            }
        }

    }
}
