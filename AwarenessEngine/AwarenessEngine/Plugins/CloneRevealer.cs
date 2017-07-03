using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;

namespace AwarenessEngine.Plugins
{
    class CloneRevealer : IPlugin
    {
        public string Name => "Clone Revealer";
        public bool Initialized { get; set; }
        public Menu Menu { get; set; }

        public void InitializePlugin()
        {
            if (Initialized)
                return;

            // Init
            Menu = AwarenessEngine.RootMenu.AddSubMenu(Name);

            // Even subscriptions
            Drawing.OnDraw += Drawing_OnDraw;

            Initialized = true;
        }

        public void UnloadPlugin()
        {
            Menu?.Remove();

            Drawing.OnDraw -= Drawing_OnDraw;

            Initialized = false;
        }

        private void Drawing_OnDraw(EventArgs args)
        {

        }
    }
}
