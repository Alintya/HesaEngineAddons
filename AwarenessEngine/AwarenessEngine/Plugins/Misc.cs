using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;
using HesaEngine.SDK.GameObjects;

namespace AwarenessEngine.Plugins
{
    class Misc : IPlugin
    {
        public string Name => "Misc";

        public bool Enabled { get; set; }
        public Menu Menu { get; set; }

        public void InitializePlugin()
        {
            if (!Enabled)
                return;

            // Init
            Menu = AwarenessEngine.RootMenu.AddSubMenu(Name);
            Menu.Add(new MenuCheckbox("recallEnabled", "Recall Tracker"));

            // Event subscriptions
            
        }
    }
}
