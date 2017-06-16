﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;

namespace AntiAFK
{
    public class Utils
    {
        public static void PrintChat(string msg)
        {
            Chat.Print("<font color = \"#ffdead\">AntiAFK:</font> <font color = \"#ffffff\">" + msg + "</font>");
        }

        public static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

    }
}