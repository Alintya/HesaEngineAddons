using System;
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

        public bool Enabled { get; set; }

        public Menu Menu { get; set; }

        public void InitializePlugin()
        {
            if(!Enabled)
                return;

            // Init

        }
    }
}