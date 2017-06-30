using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;

namespace AwarenessEngine
{
    public class Utils
    {
        public static void PrintChat(string msg)
        {
            Chat.Print("<font color = \"#ffdead\">[AwarenessEngine]:</font> <font color = \"#ffffff\">" + msg + "</font>");
        }

        public static string GetVersion()
        {
            string ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            return ver.Remove(ver.LastIndexOf('.'));
        }

        public static string GetFullVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
