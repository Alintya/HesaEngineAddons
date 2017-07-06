using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;
using HesaEngine.SDK.Enums;
using HesaEngine.SDK.GameObjects;
using SharpDX;

namespace AwarenessEngine.Plugins
{
    class CloneRevealer : IPlugin
    {
        public string Name => "Clone Revealer";
        public bool Initialized { get; set; }
        public Menu Menu { get; set; }

        private static readonly Champion[] CloneChamps = { Champion.Leblanc, Champion.Shaco, Champion.MonkeyKing };
        private List<AIHeroClient> CloneChampsIngame { get; set; } = new List<AIHeroClient>();
        private List<Obj_AI_Base> ActiveClones { get; set; } = new List<Obj_AI_Base>();

        private readonly Vector2 _offset = new Vector2(40, 40);

        public void InitializePlugin()
        {
            if (Initialized)
                return;

            // Init

            foreach (var cloneChamp in ObjectManager.Heroes.Enemies.Where(x => CloneChamps.Contains(x.Hero)))
                CloneChampsIngame.Add(cloneChamp);

            if (CloneChampsIngame.Count < 1)
            {
                Utils.PrintChat("No clone champs ingame");
                this.UnloadPlugin();
            }

            Menu = AwarenessEngine.RootMenu.AddSubMenu(Name);
            Menu.AddSeparator("Coming soon(tm)");

            // Event subscriptions
            Game.OnUpdate += Game_OnUpdate;
            GameObject.OnCreate += GameObject_OnCreate;
            Drawing.OnDraw += Drawing_OnDraw;

            Initialized = true;
        }

        public void UnloadPlugin()
        {
            if (!Initialized)
                return;

            Menu?.Remove();

            Game.OnUpdate -= Game_OnUpdate;
            GameObject.OnCreate -= GameObject_OnCreate;
            Drawing.OnDraw -= Drawing_OnDraw;

            Initialized = false;
        }


        private void Game_OnUpdate()
        {
            if (ActiveClones.Count > 0)
                ActiveClones.RemoveAll(x => !x.IsValid() || x.IsDead);
        }

        private void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            // Redundant
            if (CloneChampsIngame.Count <= 0)
                return;
            if (!sender.IsEnemy)
                return;

            var obj = sender as Obj_AI_Base;
            if (obj == null)
                return;
            
            if (CloneChampsIngame.Any(cloneChamp => obj.Name == cloneChamp.Name))
                ActiveClones.Add(obj);
        }

        private void Drawing_OnDraw(EventArgs args)
        {
            foreach (var clone in ActiveClones.Where(x => x.IsVisible && x.IsHPBarRendered))
            {
                var c = Drawing.WorldToScreen(clone.Position);
                Drawing.DrawLine(c - _offset, c + _offset, Color.Red, 3f);
                Drawing.DrawLine((c - _offset).RotateAroundPoint(c, 90), (c + _offset).RotateAroundPoint(c, 90), Color.Red, 3f);

            }
        }
    }
}
