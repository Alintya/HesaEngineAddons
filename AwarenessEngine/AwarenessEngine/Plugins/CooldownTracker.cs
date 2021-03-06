﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;

namespace AwarenessEngine.Plugins
{
    class CooldownTracker : IPlugin
    {
        public string Name => "Cooldown Tracker";

        public bool Initialized { get; set; }

        public Menu Menu { get; set; }

        public void InitializePlugin()
        {
            if(Initialized)
                return;

            // Init
            Menu = AwarenessEngine.RootMenu.AddSubMenu(Name);
            Menu.AddSeparator("Coming soon(tm)");

            Initialized = true;
        }

        public void UnloadPlugin()
        {
            if (!Initialized)
                return;

            Menu?.Remove();

            Initialized = false;
        }
    }
}