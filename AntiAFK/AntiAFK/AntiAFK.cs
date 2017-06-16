using System;
using System.Diagnostics.CodeAnalysis;
using HesaEngine.SDK;
using HesaEngine.SDK.Enums;
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

        private readonly Random Random = new Random();
        
        private static Menu RootMenu { get; set; }
        private static bool IsEnabled => RootMenu["enabled"].GetValue<bool>();
        private static int RefreshRate => RootMenu["refresh"].GetValue<int>() * 1000;
        private static bool Randomize => RootMenu["randomize"].GetValue<bool>();
        private static int RandomizeFactor => RootMenu["randomizeAmount"].GetValue<int>();

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
            Obj_AI_Base.OnProcessSpellCast += (x, y) =>
            {
                if (x.IsMe)
                    _lastActionTick = Game.GameTimeTickCount;
            };

            Obj_AI_Base.OnIssueOrder += (x, y) =>
            {
                if (x.IsMe)
                    _lastActionTick = Game.GameTimeTickCount;
            };

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
                Orbwalker.MoveTo(ObjectManager.Player.Position + 
                    (Randomize ? Random.NextVector3(-RandomizeFactor * Vector3.One, RandomizeFactor * Vector3.One) : Vector3.Zero));
                Logger.Log("Issued move command");
            }
                
        }

/*
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
*/
        private static void LoadMenu()
        {
            RootMenu = Menu.AddMenu("AntiAFK");
            RootMenu.Add(new MenuCheckbox("enabled", "Enabled", true));
            RootMenu.Add(new MenuSlider("refresh", "Refresh in Secs", 30, 179, 60));
            RootMenu.AddSeparator("");
            RootMenu.Add(new MenuCheckbox("randomize", "Randomize move destination"));
            RootMenu.Add(new MenuSlider("randomizeAmount", "Randomization Range", 1, 100, 1));

        }
    }
}
