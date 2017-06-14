using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;

namespace JungleTimers
{
    public class Utils
    {
        public static void PrintChat(string msg)
        {
            Chat.Print("<font color = \"#000cef\">Jungle Timers:</font> <font color = \"#ffffff\">" + msg + "</font>");
        }

    }
}
