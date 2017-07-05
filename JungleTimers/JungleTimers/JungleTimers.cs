using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HesaEngine.SDK;
using HesaEngine.SDK.Enums;
using HesaEngine.SDK.GameObjects;
using SharpDX;

namespace JungleTimers
{
    public class JungleTimers : IScript
    {
        public void OnInitialize()
        {
            Game.OnGameLoaded += Game_OnGameLoaded;
        }

        public string Name => "Jungle Timers";
        public string Version => Utils.GetVersion();
        public string Author => "Mystery";


        public static Menu Root;

        public static List<JungleCamp> JungleCamps { get; set; }
        public static IEnumerable<JungleCamp> DeadJungleCamps => JungleCamps.Where(x => x.Dead);


        private void Game_OnGameLoaded()
        {
            Logger.Log("Loading Jungle Timers");

            if (Game.MapId != GameMapId.SummonersRift && Game.MapId != GameMapId.TwistedTreeline)
            {
                Logger.Log("Unloading Jungle Timers - Map unsupported");
                Utils.PrintChat("Unsupported Map");
                return;
            }

            // Init
            LoadMenu();

            LoadJungleCamps();

            // Event subscriptions
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
            Drawing.OnEndScene += Drawing_OnEndScene;

            Utils.PrintChat($"{this.Name} v{this.Version} by {this.Author} loaded!");
        }

        private static void LoadMenu()
        {
            Root = Menu.AddMenu("Jungle Timers");

            Root.Add(new MenuCheckbox("enabled", "Enabled", true));
//            Root.AddSeparator("test");
//            Root.Add(new MenuSlider("fontSize", "Text Size", 1, 20, 10));

        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (Root["enabled"].GetValue<bool>() == false)
                return;
            
            foreach (var camp in DeadJungleCamps.Where(x => x.NextRespawnTime - Environment.TickCount > 0))
            {
                var timeSpan = TimeSpan.FromMilliseconds(camp.NextRespawnTime - Environment.TickCount);
                var text = timeSpan.ToString(@"m\:ss");
                var pos = new Vector2((int)camp.MinimapPosition.X, (int)camp.MinimapPosition.Y);

                Drawing.DrawText(pos, Color.White, text);
            }
        }

        private static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            if (!sender.Name.StartsWith(Game.MapId == GameMapId.SummonersRift ? "SRU" : "TT"))
                return;
            if (Root["enabled"].GetValue<bool>() == false)
                return;
            if (sender.ObjectType != GameObjectType.obj_AI_Minion)
                return;

            var camp = JungleCamps.FirstOrDefault(
                x => x.MobNames.Select(y => y.ToLower()).Any(z => z.Equals(sender.Name.ToLower())));

            if (camp == null)
                return;

            camp.ObjectsDead.Add(sender.Name);
            camp.ObjectsAlive.Remove(sender.Name);

            if (camp.ObjectsDead.Count == camp.MobNames.Length)
            {
                camp.Dead = true;
                camp.NextRespawnTime = Environment.TickCount + camp.RespawnTime - 3000;
            }
        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (!sender.Name.StartsWith(Game.MapId == GameMapId.SummonersRift ? "SRU" : "TT"))
                return;
            if (Root["enabled"].GetValue<bool>() == false)
                return;
            if (sender.ObjectType != GameObjectType.obj_AI_Minion)
                return;

            var camp =
                JungleCamps.FirstOrDefault(
                    x => x.MobNames.Select(y => y.ToLower()).Any(z => z.Equals(sender.Name.ToLower())));

            if (camp == null)
                return;

            camp.ObjectsAlive.Add(sender.Name);
            camp.ObjectsDead.Remove(sender.Name);

            if (camp.ObjectsAlive.Count == camp.MobNames.Length)
            {
                camp.Dead = false;
                camp.NextRespawnTime = 0;
            }

        }

        private static void LoadJungleCamps()
        {
            #region Init JungleCamp List

            JungleCamps = new List<JungleCamp>
            {
                new JungleCamp(
                    75000,
                    new Vector3(6078.15f, -98.63f, 6094.45f),
                    new[] { "TT_NWolf3.1.1", "TT_NWolf23.1.2", "TT_NWolf23.1.3" },
                    GameMapId.TwistedTreeline,
                    GameObjectTeam.Order),
                new JungleCamp(
                    150000,
                    new Vector3(6943.41f, 52.62f, 5422.61f),
                    new[] { "SRU_Razorbeak3.1.1", "SRU_RazorbeakMini3.1.2", "SRU_RazorbeakMini3.1.3",
                        "SRU_RazorbeakMini3.1.4", "SRU_RazorbeakMini3.1.5", "SRU_RazorbeakMini3.1.6" },
                    GameMapId.SummonersRift,
                    GameObjectTeam.Order),
                new JungleCamp(
                    150000,
                    new Vector3(2164.34f, 51.78f, 8383.02f),
                    new[] { "SRU_Gromp13.1.1" },
                    GameMapId.SummonersRift,
                    GameObjectTeam.Order),
                new JungleCamp(
                    150000,
                    new Vector3(8370.58f, 51.09f, 2718.15f),
                    new[] { "SRU_Krug5.1.1", "SRU_KrugMini5.1.2"/*, "MiniKrugA", "MiniKrugB", 
                                          "MiniKrugA", "MiniKrugB", "MiniKrugA", "MiniKrugB", "MiniKrugA", "MiniKrugB"*/ },
                    GameMapId.SummonersRift,
                    GameObjectTeam.Order),
                new JungleCamp(
                    180000,
                    new Vector3(4285.04f, -67.6f, 9597.52f),
                    new[] { "SRU_Crab16.1.1" },
                    GameMapId.SummonersRift,
                    GameObjectTeam.Neutral),
                new JungleCamp(
                    150000,
                    new Vector3(6476.17f, 56.48f, 12142.51f),
                    new[] { "SRU_Krug11.1.1", "SRU_KrugMini11.1.2"/*, "MiniKrugA", "MiniKrugB", "MiniKrugA",
                                          "MiniKrugB", "MiniKrugA", "MiniKrugB", "MiniKrugA", "MiniKrugB"*/ },
                    GameMapId.SummonersRift,
                    GameObjectTeam.Chaos),
                new JungleCamp(
                    75000,
                    new Vector3(11025.95f, -107.19f, 5805.61f),
                    new[] { "TT_NWraith4.1.1", "TT_NWraith24.1.2", "TT_NWraith24.1.3" },
                    GameMapId.TwistedTreeline,
                    GameObjectTeam.Chaos),
                new JungleCamp(
                    150000,
                    new Vector3(10983.83f, 62.22f, 8328.73f),
                    new[] { "SRU_Murkwolf8.1.1", "SRU_MurkwolfMini8.1.2", "SRU_MurkwolfMini8.1.3" },
                    GameMapId.SummonersRift,
                    GameObjectTeam.Chaos),
                new JungleCamp(
                    150000,
                    new Vector3(12671.83f, 51.71f, 6306.6f),
                    new[] { "SRU_Gromp14.1.1" },
                    GameMapId.SummonersRift,
                    GameObjectTeam.Chaos),
                new JungleCamp(
                    360000,
                    new Vector3(7738.3f, -61.6f, 10079.78f),
                    new[] { "TT_Spiderboss8.1.1" },
                    GameMapId.SummonersRift,
                    GameObjectTeam.Neutral),
                new JungleCamp(
                    300000,
                    new Vector3(3800.99f, 52.18f, 7883.53f),
                    new[] { "SRU_Blue1.1.1"/*, "SRU_BlueMini1.1.2", "SRU_BlueMini21.1.3" */},
                    GameMapId.SummonersRift,
                    GameObjectTeam.Order),
                new JungleCamp(
                    75000,
                    new Vector3(4373.14f, -107.14f, 5842.84f),
                    new[] { "TT_NWraith1.1.1", "TT_NWraith21.1.2", "TT_NWraith21.1.3" },
                    GameMapId.TwistedTreeline,
                    GameObjectTeam.Order),
                new JungleCamp(
                    300000,
                    new Vector3(4993.14f, -71.24f, 10491.92f),
                    new[] { "SRU_RiftHerald" },
                    GameMapId.SummonersRift,
                    GameObjectTeam.Neutral),
                new JungleCamp(
                    75000,
                    new Vector3(5106.94f, -108.38f, 7985.9f),
                    new[] { "TT_NGolem2.1.1", "TT_NGolem22.1.2" },
                    GameMapId.TwistedTreeline,
                    GameObjectTeam.Order),
                new JungleCamp(
                    150000,
                    new Vector3(7852.38f, 52.3f, 9562.62f),
                    new[] { "SRU_Razorbeak9.1.1", "SRU_RazorbeakMini9.1.2", "SRU_RazorbeakMini9.1.3",
                        "SRU_RazorbeakMini9.1.4", "SRU_RazorbeakMini9.1.5", "SRU_RazorbeakMini9.1.6" },
                    GameMapId.SummonersRift,
                    GameObjectTeam.Chaos),
                new JungleCamp(
                    300000,
                    new Vector3(10984.11f, 51.72f, 6960.31f),
                    new[] { "SRU_Blue7.1.1"/*, "SRU_BlueMini7.1.2", "SRU_BlueMini27.1.3" */},
                    GameMapId.SummonersRift,
                    GameObjectTeam.Chaos),
                new JungleCamp(
                    180000,
                    new Vector3(10647.7f, -62.81f, 5144.68f),
                    new[] { "SRU_Crab15.1.1" },
                    GameMapId.SummonersRift,
                    GameObjectTeam.Neutral),
                new JungleCamp(
                    75000,
                    new Vector3(9294.02f, -96.7f, 6085.41f),
                    new[] { "TT_NWolf6.1.1", "TT_NWolf26.1.2", "TT_NWolf26.1.3" },
                    GameMapId.TwistedTreeline,
                    GameObjectTeam.Chaos),
                new JungleCamp(
                    420000,
                    new Vector3(4993.14f, -71.24f, 10491.92f),
                    new[] { "SRU_Baron12.1.1" },
                    GameMapId.SummonersRift,
                    GameObjectTeam.Neutral),
                new JungleCamp(
                    150000,
                    new Vector3(3849.95f, 52.46f, 6504.36f),
                    new[] { "SRU_Murkwolf2.1.1", "SRU_MurkwolfMini2.1.2", "SRU_MurkwolfMini2.1.3" },
                    GameMapId.SummonersRift,
                    GameObjectTeam.Order),
                new JungleCamp(
                    300000,
                    new Vector3(7813.07f, 53.81f, 4051.33f),
                    new[] { "SRU_Red4.1.1"/*, "SRU_RedMini4.1.2", "SRU_RedMini4.1.3" */},
                    GameMapId.SummonersRift,
                    GameObjectTeam.Order),
                new JungleCamp(
                    360000,
                    new Vector3(9813.83f, -71.24f, 4360.19f),
                    new[] { "SRU_Dragon_Air6.1.1" },
                    GameMapId.SummonersRift,
                    GameObjectTeam.Neutral),
                new JungleCamp(
                    360000,
                    new Vector3(9813.83f, -71.24f, 4360.19f),
                    new[] { "SRU_Dragon_Earth6.4.1" },
                    GameMapId.SummonersRift,
                    GameObjectTeam.Neutral),
                new JungleCamp(
                    360000,
                    new Vector3(9813.83f, -71.24f, 4360.19f),
                    new[] { "SRU_Dragon_Fire6.2.1" },
                    GameMapId.SummonersRift,
                    GameObjectTeam.Neutral),
                new JungleCamp(
                    360000,
                    new Vector3(9813.83f, -71.24f, 4360.19f),
                    new[] { "SRU_Dragon_Water6.3.1" },
                    GameMapId.SummonersRift,
                    GameObjectTeam.Neutral),
                new JungleCamp(
                    360000,
                    new Vector3(9813.83f, -71.24f, 4360.19f),
                    new[] { "SRU_Dragon_Elder6.5.1" },
                    GameMapId.SummonersRift,
                    GameObjectTeam.Neutral),
                new JungleCamp(
                    300000,
                    new Vector3(7139.29f, 56.38f, 10779.34f),
                    new[] { "SRU_Red10.1.1"/*, "SRU_RedMini10.1.2", "SRU_RedMini10.1.3" */},
                    GameMapId.SummonersRift,
                    GameObjectTeam.Chaos),
                new JungleCamp(
                    75000,
                    new Vector3(10276.81f, -108.92f, 8037.54f),
                    new[] { "TT_NGolem5.1.1", "TT_NGolem25.1.2" },
                    GameMapId.TwistedTreeline,
                    GameObjectTeam.Chaos)
            };

            #endregion

            JungleCamps = JungleCamps.Where(x => x.MapID == Game.MapId).ToList();
        }
    }
}
