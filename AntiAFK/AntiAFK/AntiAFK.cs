using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;
using HesaEngine.SDK.Args;
using HesaEngine.SDK.GameObjects;
using SharpDX;

namespace AntiAFK
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AntiAFK : IScript
    {
        public void OnInitialize()
        {
            Game.OnGameLoaded += OnGameLoaded;
        }

        public string Name => "AntiAFK";
        public string Version => Utils.GetVersion();
        public string Author => "Mystery";

        private Random Random = new Random();

        private static Menu RootMenu { get; set; }
        private static bool IsEnabled => RootMenu["enabled"].GetValue<bool>();
        private static int RefreshRate => RootMenu["refresh"].GetValue<int>() * 1000;

        //private bool IsAFK { get; set; } = false;
        
        private int _lastActionTick;
        private int _lastTick;

        private const int _tickLimit = 100;

        private void OnGameLoaded()
        {
            Logger.Log("Loading AntiAFK");

            // Init
            LoadMenu();

            _lastTick = Game.GameTimeTickCount;
            _lastActionTick = Game.GameTimeTickCount;
            

            // Event subscriptions
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Obj_AI_Base.OnIssueOrder += OnIssueOrder;
            Game.OnTick += OnTick;

            Utils.PrintChat($"{this.Name} v{this.Version} by {this.Author} loaded!");
        }

        private void OnTick()
        {
            if (!IsEnabled)
                return;

            if ((Game.GameTimeTickCount - _lastTick) < _tickLimit) 
                return;

            _lastTick = Game.GameTimeTickCount;

            if (Game.GameTimeTickCount - _lastActionTick > RefreshRate)
            {
                Orbwalker.MoveTo(ObjectManager.Player.Position + Random.NextVector3(-Vector3.One, Vector3.One));
                Logger.Log("Moved champ");
            }
                
        }

        private void OnIssueOrder(Obj_AI_Base sender, GameObjectIssueOrderEventArgs args)
        {
            if (sender.IsMe)
                _lastActionTick = Game.GameTimeTickCount;
        }

        private void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
                _lastActionTick = Game.GameTimeTickCount;
        }

        private static void LoadMenu()
        {
            RootMenu = Menu.AddMenu("AntiAFK");
            RootMenu.Add(new MenuCheckbox("enabled", "Enabled", true));
            RootMenu.Add(new MenuSlider("refresh", "Refresh in Secs", 30, 150, 60));

        }
    }
}
