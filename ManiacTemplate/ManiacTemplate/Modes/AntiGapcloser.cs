using HesaEngine.SDK;
using static ManiacTemplate.SpellManager;
using static ManiacTemplate.MenuManager;

namespace ManiacTemplate.Modes
{
    public static class AntiGapcloser
    {

        public static void DoAntigapclose(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsAlly || gapcloser.Sender.IsDead || gapcloser.Sender.IsMe) return;

            var q = miscMenu.GetCheckbox("agQ") && Q.IsReady();
            var w = miscMenu.GetCheckbox("agW") && W.IsReady();
            var e = miscMenu.GetCheckbox("agE") && E.IsReady();
            var r = miscMenu.GetCheckbox("agR") && R.IsReady();

            if (w)
                W.CastIfHitchanceEquals(gapcloser.Sender, HitChance.Medium);
            
        }
    }
}
