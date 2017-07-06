using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwarenessEngine.Plugins;
using HesaEngine.SDK;

namespace AwarenessEngine
{
    public class AwarenessEngine : IScript
    {
        public void OnInitialize()
        {
            Game.OnGameLoaded += Game_OnGameLoaded;

            Utils.DebugOutput = true;
        }

        public string Name => "AwarenessEngine";
        public string Version => Utils.GetFullVersion();
        public string Author => "Mystery";

        public static Menu RootMenu { get; set; }
        private static Menu PluginMenu { get; set; }

        private List<IPlugin> PluginList { get; set; }


        private void Game_OnGameLoaded()
        {
            Logger.Log("Loading " + Name);

            // Init
            LoadPlugins();
            LoadMenu();

            // Event subscriptions
            Drawing.OnDraw += OnDraw;


            Utils.PrintChat($"v{Version} by {Author} loaded!");
        }

        private void LoadPlugins()
        {
            PluginList = new List<IPlugin>
            {
                new CloneRevealer(),
                new CooldownTracker(),
                new Misc(),
                new WardTracker(),
                new MapHack()
            };

        }

        private void LoadMenu()
        {
            RootMenu = Menu.AddMenu(Name);

            PluginMenu = RootMenu.AddSubMenu("Plugins");

            foreach (var plugin in PluginList)
                PluginMenu.Add(new MenuCheckbox(plugin.Name, plugin.Name)).OnValueChanged += OnPluginStateChanged;

        }

        private void OnPluginStateChanged(MenuCheckbox menuCheckbox, bool b)
        {
            var p = PluginList.Find(x => x.Name == menuCheckbox.Name);

            if (p == null)
                return;

            if (b)
                p.InitializePlugin();
            else
                p.UnloadPlugin();
            
        }

        private static void OnDraw(EventArgs args)
        {

        }
    }
}
