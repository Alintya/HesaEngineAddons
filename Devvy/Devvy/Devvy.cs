using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;
using HesaEngine.SDK.Args;
using HesaEngine.SDK.Enums;
using HesaEngine.SDK.GameObjects;
using HesaEngine.SDK.Notifications;
using SharpDX;

namespace Devvy
{
    public class Devvy : IScript
    {
        public void OnInitialize()
        {
            Game.OnGameLoaded += Game_OnGameLoaded;
        }

        public string Name => "Devvy";
        public string Version => Utils.GetVersion();
        public string Author => "Mystery";

        private static Menu RootMenu { get; set; }

        public bool ShowMouse => RootMenu.SubMenu("Misc")["showMouseInfo"].GetValue<bool>();
        public bool ShowMouseLines => RootMenu.SubMenu("Misc")["showMouseLines"].GetValue<bool>();
        public bool ShowBuffs => RootMenu.SubMenu("Misc")["showBuffs"].GetValue<bool>();

        public bool NearMouseInfo => RootMenu.SubMenu("Near Mouse")["nearMouseEnabled"].GetValue<bool>();
        public int NearMouseRange => RootMenu.SubMenu("Near Mouse")["nearMouseRange"].GetValue<int>();
        public bool ChampsOnlyNearMouse => RootMenu.SubMenu("Near Mouse")["nearMouseChampsOnly"].GetValue<bool>();

        public bool SpellInfoEnabled => RootMenu.SubMenu("Spell Info")["enabled"].GetValue<bool>();
        public bool ChampsOnlySpells => RootMenu.SubMenu("Spell Info")["champsOnly"].GetValue<bool>();
        public bool OnlyMySpells => RootMenu.SubMenu("Spell info")["meOnly"].GetValue<bool>();

        private bool tog = true;
        private string line = "";

        private void Game_OnGameLoaded()
        {
            Logger.Log("Loading Devvy");

            LoadMenu();
            // Init


            // Event subscriptions
            Drawing.OnDraw += OnDraw;
            //SpellBook.OnCastSpell += OnCastSpell;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;

            /*
            GameObject.OnCreate += (x, y) =>
            {
                //Logger.Log("OnCreate called");
                if(x.Name != "missile")
                    Logger.Log(x.Name);      
            };
            */
            
            Utils.PrintChat($"{this.Name} v{this.Version} by {this.Author} loaded!");
        }

        private void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!SpellInfoEnabled)
                return;
            if (ChampsOnlySpells && sender.ObjectType != GameObjectType.AIHeroClient)
                return;
            if (OnlyMySpells && !sender.IsMe)
                return;

            
            Logger.Log($"{sender.Name}, {args.SData.SpellDataInfos}:\n--------------- start");

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(args.SData.SpellDataInfos))
            {
                string combo = $"{descriptor.Name,-20} = {descriptor.GetValue(args.SData.SpellDataInfos)}";


                if (tog)
                {
                    //line = $"{descriptor.Name,-10}: {descriptor.GetValue(args.SData.SpellDataInfos)}\t\t\t";
                    line = $"{combo,-40}|";
                }
                else
                {
                    line += $"| {combo}"; 
                    Console.WriteLine(line);
                }
                tog = !tog;
            }
            Console.WriteLine("--------------- end");
        }

        private void OnCastSpell(SpellBook sender, SpellbookCastSpellEventArgs args)
        {
            Logger.Log($"{sender.Owner.Name}, {args.Target.Name}");
        }

        private void OnDraw(EventArgs args)
        {
            //Logger.Log("OnDraw called");

            if (ShowMouseLines)
            {
                Drawing.DrawLine(new Vector2(Game.MousePos.X, 0), new Vector2(Game.MousePos.X, Drawing.Height),
                    Color.Orange);
                Drawing.DrawLine(new Vector2(0, Game.MousePos.Y), new Vector2(Drawing.Width, Game.MousePos.Y),
                    Color.Orange);
            }

            if (ShowMouse)
            {
                Drawing.DrawText(Game.MousePos + new Vector2(40, 0), Color.GhostWhite,
                    $"Screen Position: X:{Game.MousePos.X} Y:{Game.MousePos.Y}");

                Drawing.DrawText(Game.MousePos + new Vector2(40, 20), Color.Orange,
                    $"Game Position: X:{Math.Round(Game.CursorPosition.X)} Y:{Math.Round(Game.CursorPosition.Y)} Z:{Math.Round(Game.CursorPosition.Z)}");

                var navMeshCell = Game.CursorPosition.ToNavMeshCell();
                Drawing.DrawText(Game.MousePos + new Vector2(40, 40), Color.NavajoWhite,
                    $"Collision flags: {navMeshCell.Flags}");
            }

            if (NearMouseInfo)
            {
                Drawing.DrawCircle(Game.CursorPosition, NearMouseRange, Color.Red);
            }


            foreach (var obj in (ChampsOnlyNearMouse ? ObjectManager.Get<GameObject>()
                .Where(x => x.IsVisibleOnScreen && x.GetType() == typeof(AIHeroClient)) : 
                ObjectManager.Get<GameObject>())
                .Where(o => o.IsVisibleOnScreen))
            {
                var row = 0;
                const int height = 20;

                var baseObject = obj as Obj_AI_Base;


                if (NearMouseInfo && obj.IsInRange(Game.CursorPosition, NearMouseRange))
                {
                    Drawing.DrawText(obj.Position.WorldToScreen() + new Vector2(0, row), Color.Orange, "General info");
                    row += height;

                    var data = new Dictionary<string, object>
                    {
                        { "System.Type", obj.GetType().Name },
                        { "GameObjectType", obj.GetType().ToString() },
                        { "Name", obj.Name },
                        { "Position", obj.Position }
                    };
                    foreach (var dataEntry in data)
                    {
                        Drawing.DrawText(obj.Position.WorldToScreen() + new Vector2(0, row), Color.NavajoWhite, $"{dataEntry.Key}: {dataEntry.Value}");
                        row += height;
                    }
                    Drawing.DrawCircle(obj.Position, obj.BoundingRadius, Color.DarkRed);
                }

                if (ShowBuffs && baseObject != null && baseObject.ObjectType != GameObjectType.obj_AI_Minion && baseObject.ObjectType != GameObjectType.obj_AI_Turret)
                {
                    Drawing.DrawText(baseObject.Position.WorldToScreen() + new Vector2(0, row), Color.Orange, "Buffs");
                    row += height;
                    //Drawing.DrawText(baseObject.Position.WorldToScreen() + new Vector2(0, row), Color.NavajoWhite, "None");
                    foreach (var buff in baseObject.Buffs.Where(o => o.IsValid()))
                    {
                        var endTime = Math.Max(0, buff.EndTime - Game.Time);
                        Drawing.DrawText(baseObject.Position.WorldToScreen() + new Vector2(0, row), Color.NavajoWhite,
                            string.Format("DisplayName: {0} | Name: {1} | Caster: {2} | Count: {3} | RemainingTime: {4}", buff.DisplayName, buff.Name, buff.Caster.Name, buff.Count,
                                endTime > 1000 ? "Infinite" : Convert.ToString(endTime, CultureInfo.InvariantCulture)));

                        row += height;
                    }
                }
            }
        }

        private static void LoadMenu()
        {
            RootMenu = Menu.AddMenu("Devvy");

            RootMenu.AddSubMenu("Misc");
            RootMenu.SubMenu("Misc").Add(new MenuCheckbox("showMouseInfo", "Show mouse info"));
            RootMenu.SubMenu("Misc").Add(new MenuCheckbox("showMouseLines", "Show mouse lines"));
            RootMenu.SubMenu("Misc").Add(new MenuCheckbox("showBuffs", "Show buffs"));

            RootMenu.AddSubMenu("Spell Info");
            RootMenu.SubMenu("Spell Info").Add(new MenuCheckbox("enabled", "Enabled")).OnValueChanged += OnSpellInfoEnabledChange;
            RootMenu.SubMenu("Spell Info")["enabled"].SetTooltip("Outputs to console.");
            RootMenu.SubMenu("Spell Info").Add(new MenuCheckbox("champsOnly", "Only Champions", true));
            RootMenu.SubMenu("Spell Info").Add(new MenuCheckbox("meOnly", "Only Me", true));
            RootMenu.SubMenu("Spell Info").AddSubMenu("Spell Data");
            RootMenu.SubMenu("Spell Info").SubMenu("Spell Data").Add(new MenuCheckbox("prop", "Prop"));

            RootMenu.AddSubMenu("Near Mouse");
            RootMenu.SubMenu("Near Mouse").Add(new MenuCheckbox("nearMouseEnabled", "Near mouse object info"));
            RootMenu.SubMenu("Near Mouse").Add(new MenuSlider("nearMouseRange", "Near mouse range", 0, 1000, 500));
            RootMenu.SubMenu("Near Mouse").Add(new MenuCheckbox("nearMouseChampsOnly", "Only Champions"));
        
        }

        private static void OnSpellInfoEnabledChange(MenuCheckbox menuCheckbox, bool b)
        {
            if (b)
            {
                Chat.Print("Spell data is being printed to console.");
                Notifications.AddNotification("Check console for spell data", 1000);     
            }

        }
    }
}
