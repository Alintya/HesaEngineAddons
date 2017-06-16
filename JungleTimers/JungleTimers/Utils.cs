﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;

namespace JungleTimers
{
    public class Utils
    {
        public static void PrintChat(string msg)
        {
            Chat.Print("<font color = \"#ffdead\">Jungle Timers:</font> <font color = \"#ffffff\">" + msg + "</font>");
        }

        public static string GetVersion()
        {
            string ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            return ver.Remove(ver.LastIndexOf('.'));
        }

    }
}
