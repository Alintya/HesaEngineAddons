using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;

namespace AwarenessEngine
{
    public class AwarenessEngine : IScript
    {
        public void OnInitialize()
        {
            Game.OnGameLoaded += Game_OnGameLoaded;
        }

        public string Name => "AwarenessEngine";
        public string Version => Utils.GetFullVersion();
        public string Author => "Mystery";

        public static Menu RootMenu { get; set; }

        //private bool test = RootMenu.Get<MenuCheckbox>("CQ").Checked;


        private void Game_OnGameLoaded()
        {
            Logger.Log("Loading " + Name);

            // Init
            LoadMenu();

            // Event subscriptions
            Drawing.OnDraw += OnDraw;


            Utils.PrintChat($"{Name} v{Version} by {Author} loaded!");
        }

        private static void OnDraw(EventArgs args)
        {

        }

        private void LoadMenu()
        {
            RootMenu = Menu.AddMenu(Name);

            RootMenu.AddSubMenu("Misc");

        }
    }
}
