using HesaEngine.SDK;
using static ManiacTemplate.MenuManager;
using static ManiacTemplate.SpellManager;

namespace ManiacTemplate.Modes
{
    public static class Harass
    {
        public static void DoHarass()
        {
            var q = harassMenu.GetCheckbox("useQ") && Q.IsReady();
            var w = harassMenu.GetCheckbox("useW") && W.IsReady();
            var e = harassMenu.GetCheckbox("useE") && E.IsReady();
            var r = harassMenu.GetCheckbox("useR") && R.IsReady();

            var Target = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);

            if (w)
                W.CastIfHitchanceEquals(Target, HitChance.Medium);
        }
    }
}
