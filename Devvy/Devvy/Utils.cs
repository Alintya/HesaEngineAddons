﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;

namespace Devvy
{
    public class Utils
    {
        public static void PrintChat(string msg)
        {
            Chat.Print("<font color = \"#ffdead\">Jungle Timers:</font> <font color = \"#ffffff\">" + msg + "</font>");
        }

    }
}
