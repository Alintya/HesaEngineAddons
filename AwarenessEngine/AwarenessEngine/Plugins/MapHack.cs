using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;
using HesaEngine.SDK.Args;
using HesaEngine.SDK.Enums;
using HesaEngine.SDK.GameObjects;
using SharpDX;

namespace AwarenessEngine.Plugins
{
    public class LastSeenInfo
    {
        public int LastSeenTime;
        public float LastSeenRange;
        public Vector3 LastSeenPosition;
    }

    class MapHack : IPlugin
    {
        public string Name => "Map Hack";
        public bool Initialized { get; set; }
        public Menu Menu { get; set; }

        private Dictionary<int, LastSeenInfo> FogOfWarChamps { get; set; } = new Dictionary<int, LastSeenInfo>();
        private List<int> DeadChamps { get; set; } = new List<int>();
        private Vector3 EnemySpawn { get; set; }

        private int LastUpdate { get; set; }

        private int DisableRange => Menu.GetSlider("Disable movement circle range");

        public void InitializePlugin()
        {
            if (Initialized)
                return;

            // Init
            Menu = AwarenessEngine.RootMenu.AddSubMenu(Name);
            Menu.AddSeparator("Coming soon(tm)");
            Menu.AddCheckbox("Draw recall circle");
            Menu.AddCheckbox("Draw movement circle");
            Menu.AddSlider("Disable movement circle range", 2000, 200, 800);

            var objSpawnPoint = ObjectManager.Get<Obj_SpawnPoint>().FirstOrDefault(x => x.IsEnemy);
            if (objSpawnPoint != null)
                EnemySpawn = objSpawnPoint.Position;

            // Event subscriptions
            Game.OnTick += Game_OnTick;
            Obj_AI_Base.OnTeleport += Base_OnTeleport;
            //GameObject.OnCreate += GameObject_OnCreate;
            Drawing.OnEndScene += Drawing_OnEndScene;

            Initialized = true;
        }

        public void UnloadPlugin()
        {
            if (!Initialized)
                return;

            Menu?.Remove();

            Game.OnTick -= Game_OnTick;
            Obj_AI_Base.OnTeleport -= Base_OnTeleport;
            GameObject.OnCreate -= GameObject_OnCreate;
            Drawing.OnDraw -= Drawing_OnEndScene;

            Initialized = false;
        }

        private void Game_OnTick()
        {
            var elapsedTime = Game.GameTimeTickCount - LastUpdate;
            LastUpdate = Game.GameTimeTickCount;

            foreach (var enemy in ObjectManager.Heroes.Enemies)
            {
                // Died
                if (enemy.IsDead)
                {
                    if (!DeadChamps.Contains(enemy.NetworkId))
                    {
                        Utils.DebugLog(enemy.ChampionName + " died " + enemy.NetworkId);
                        DeadChamps.Add(enemy.NetworkId);
                        FogOfWarChamps.Remove(enemy.NetworkId);
                    }
                    continue;
                }

                // Visible
                if (enemy.IsVisible)
                {
                    if (FogOfWarChamps.ContainsKey(enemy.NetworkId))
                    {
                        Utils.DebugLog(enemy.ChampionName + " reappeared " + enemy.NetworkId);
                        FogOfWarChamps.Remove(enemy.NetworkId);
                    }
                    continue;
                }

                // Respawned
                if (DeadChamps.Contains(enemy.NetworkId))
                {
                    Utils.DebugLog(enemy.ChampionName + " respawned " + enemy.NetworkId);
                    DeadChamps.Remove(enemy.NetworkId);

                    FogOfWarChamps[enemy.NetworkId] = new LastSeenInfo
                    {
                        LastSeenTime = Game.GameTimeTickCount,
                        LastSeenPosition = EnemySpawn,
                        LastSeenRange = 0
                    };
                }

                // Disappeared
                if (!FogOfWarChamps.ContainsKey(enemy.NetworkId))
                {
                    Utils.DebugLog(enemy.ChampionName + " disappeared " + enemy.NetworkId);
                    FogOfWarChamps[enemy.NetworkId] = new LastSeenInfo
                    {
                        LastSeenPosition = enemy.ServerPosition,
                        LastSeenTime = Game.GameTimeTickCount,
                        LastSeenRange = 0
                    };
                }


                if (elapsedTime > 0 && FogOfWarChamps.ContainsKey(enemy.NetworkId)/* && !RecallingHeroes.ContainsKey(enemy.NetworkId)*/)
                    FogOfWarChamps[enemy.NetworkId].LastSeenRange += (enemy.MovementSpeed >= 1 ? enemy.MovementSpeed : 540) * elapsedTime / 1000f;

                Utils.DebugLog($"RangeUpdate for {enemy.ChampionName}: {FogOfWarChamps[enemy.NetworkId].LastSeenRange}\t @{enemy.MovementSpeed}");
            }
            
        }

        private void Base_OnTeleport(Obj_AI_Base sender, GameObjectTeleportEventArgs args)
        {
            Utils.DebugLog("On Teleport: " + sender.BaseSkinName);
            if (sender.ObjectType != GameObjectType.AIHeroClient/* || sender.IsAlly*/)
                return;

            Utils.DebugLog($"{sender.BaseSkinName} doing {args.SpellName}");
            Utils.DebugLog($"{args.EndTime}");
            /*
            if (sender.ObjectType == GameObjectType.AIHeroClient && sender.IsEnemy && args.GetType() == args..Recall)
            {
                
                switch (args.Status)
                {
                    case TeleportStatus.Start:
                        RecallingHeroes[sender.NetworkId] = new Tuple<int, int>(Core.GameTickCount, args.Duration);
                        break;

                    case TeleportStatus.Abort:
                        RecallingHeroes.Remove(sender.NetworkId);
                        break;

                    case TeleportStatus.Finish:
                        FogOfWarChamps[sender.NetworkId] = new LastSeenInfo
                        {
                            LastSeenTime = Game.GameTimeTickCount,
                            LastSeenPosition = EnemySpawn,
                            LastSeenRange = 0
                        };
                        RecallingHeroes.Remove(sender.NetworkId);
                        break;
                }
                
            }
            */
        }

        private void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (sender.ObjectType != GameObjectType.Missile)
                return;

            var missile = (MissileClient)sender;

            if (missile.SpellCaster == null || missile.SpellCaster.ObjectType != GameObjectType.AIHeroClient ||
                !missile.SpellCaster.IsEnemy || missile.StartPosition.IsZero)
                    return;

            FogOfWarChamps[sender.NetworkId] = new LastSeenInfo
            {
                LastSeenTime = Game.GameTimeTickCount,
                LastSeenPosition = missile.StartPosition,
                LastSeenRange = 0
            };
        }


        private void Drawing_OnEndScene(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Heroes.Enemies.Where(x => !x.IsDead || x.IsInRange(EnemySpawn, 250)))
            {
                if (!FogOfWarChamps.ContainsKey(enemy.NetworkId))
                    continue;

                var pos = FogOfWarChamps[enemy.NetworkId].LastSeenPosition.WorldToMinimap();


                //var invisibleTime = (Game.GameTimeTickCount - FogOfWarChamps[enemy.NetworkId].LastSeenTime) / 1000f;

                if (Menu.GetCheckbox("Draw movement circle"))
                {

                    var radius = FogOfWarChamps[enemy.NetworkId].LastSeenRange;
                    
                    if (radius < DisableRange)
                    {
                        Drawing.DrawCircle(pos - new Vector2(radius/2), (int)radius);
                    }
                    
                }

                // Draw the minimap icon
                //ChampionSprites[enemy.Hero].Draw(pos + MinimapIconOffset);
                /*
                    // Draw the time being invisible
                    if (DrawInvisibleTime.CurrentValue && invisibleTime >= DelayInvisibleTime.CurrentValue)
                    {
                        var text = Math.Floor(invisibleTime).ToString(CultureInfo.InvariantCulture);
                        var bounding = TimerText.MeasureBounding(text);
                        TimerText.Draw(text, TimerText.Color, pos - (new Vector2(bounding.Width, bounding.Height) / 2) + 1);
                    }
                    */
                /*
                // Draw recall circle
                if (DrawRecallCircle.CurrentValue && RecallingHeroes.ContainsKey(enemy.NetworkId))
                {
                    var startTime = RecallingHeroes[enemy.NetworkId].Item1;
                    var duration = RecallingHeroes[enemy.NetworkId].Item2;

                    Utilities.DrawArc(pos, (MinimapIconSize + 4) / 2f, Color.Aqua, 3.1415f, Utilities.PI2 * ((Core.GameTickCount - startTime) / (float)duration), 2f, 100);
                }
                */
            }
        }
    }
}
