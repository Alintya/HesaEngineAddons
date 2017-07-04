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

        public bool Initialized { get; set; }
        public Menu Menu { get; set; }

        public void InitializePlugin()
        {
            if (Initialized)
                return;

            // Init
            Menu = AwarenessEngine.RootMenu.AddSubMenu(Name);
            Menu.AddSeparator("Coming soon(tm)");

            // Event subscriptions


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
