using HesaEngine.SDK;
using HesaEngine.SDK.Enums;
using SharpDX;

namespace Run_it_down_mid
{
    public class Feeder : IScript
    {
        public void OnInitialize()
        {
            Game.OnGameLoaded += Game_OnGameLoaded;
        }

        public string Name => "Feeder";
        public string Version => Utils.GetVersion();
        public string Author => "Mystery";

        private static Menu RootMenu { get; set; }

        private static readonly Vector3 OrderSpawn = new Vector3(395, 460, 170);
        private static readonly Vector3 ChaosSpawn = new Vector3(14340, 14390, 180);

        private void Game_OnGameLoaded()
        {
            Logger.Log("Loading " + Name);

            // Init
            RootMenu = new Menu("Feeder");
            RootMenu.Add(new MenuCheckbox("enabled", "Enabled", false));

            // Event subscriptions
            Game.OnTick += Game_OnTick;

            Utils.PrintChat($"v{Version} by {Author} loaded!");
        }

        private void Game_OnTick()
        {
            if (!RootMenu.GetCheckbox("enabled"))
                return;
            if (ObjectManager.Me.IsDead)
                return;

            Orbwalker.MoveTo(ObjectManager.Me.Team == GameObjectTeam.Order ? ChaosSpawn : OrderSpawn);
        }
    }
}
