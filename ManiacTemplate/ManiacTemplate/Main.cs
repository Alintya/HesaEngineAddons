using HesaEngine.SDK;
using HesaEngine.SDK.Enums;
using HesaEngine.SDK.GameObjects;
using ManiacTemplate.Modes;
using static ManiacTemplate.SpellManager;
using static ManiacTemplate.MenuManager;
using static ManiacTemplate.DrawingManager;

namespace ManiacTemplate
{
    public class Main : IScript
    {
        //Don't forget to change the champion first!
        private readonly Champion champion = Champion.Ashe;

        public string Name => "Maniac" + champion;

        public string Version => "1.0.0";

        public string Author => "BoliBerrys";

        public static PredictionInput QPred, WPred, EPred, RPred;

        public static Orbwalker.OrbwalkerInstance orb;

        public void OnInitialize()
        {
            Game.OnGameLoaded += Game_OnGameLoaded;
        }

        private void Game_OnGameLoaded()
        {
            if (ObjectManager.Me.Hero != champion)
                return;

            LoadMenu();

            LoadSpells();

            HesaEngine.SDK.AntiGapcloser.OnEnemyGapcloser += Modes.AntiGapcloser.DoAntigapclose;

            LoadDrawings();

            Game.OnTick += Game_OnTick;
            Chat.Print(Name + " Loaded Successfully");
        }

        

        private void Game_OnTick()
        {
            var mana = ObjectManager.Me.ManaPercent;

            if (killstealMenu.GetCheckbox("enable"))
                Killsteal.DoKS();

            if (orb.ActiveMode == Orbwalker.OrbwalkingMode.Combo && mana >= comboMenu.GetSlider("mana"))
                Combo.DoCombo();

            if (orb.ActiveMode == Orbwalker.OrbwalkingMode.Harass && mana >= harassMenu.GetSlider("mana"))
            {
                //Not Implemented Yet
                Harass.DoHarass();
            }

            if (orb.ActiveMode == Orbwalker.OrbwalkingMode.LaneClear && mana >= laneclearMenu.GetSlider("mana"))
                LaneClear.DoLaneClear();

            if (orb.ActiveMode == Orbwalker.OrbwalkingMode.LastHit && mana >= lasthitMenu.GetSlider("mana"))
                LaneClear.DoLaneClear();

            if (orb.ActiveMode == Orbwalker.OrbwalkingMode.Flee && mana >= fleeMenu.GetSlider("mana"))
            {
                //Not Implemented Yet
                Flee.DoFlee();
            }
        }
    }
}
