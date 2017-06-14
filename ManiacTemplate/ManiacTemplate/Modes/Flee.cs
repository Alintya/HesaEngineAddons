using HesaEngine.SDK;
using static ManiacTemplate.SpellManager;
using static ManiacTemplate.MenuManager;

namespace ManiacTemplate.Modes
{
    public static class Flee
    {
        public static void DoFlee()
        {
            var q = fleeMenu.GetCheckbox("useQ") && Q.IsReady();
            var w = fleeMenu.GetCheckbox("useW") && W.IsReady();
            var e = fleeMenu.GetCheckbox("useE") && E.IsReady();
            var r = fleeMenu.GetCheckbox("useR") && R.IsReady();

            var Target = TargetSelector.GetTarget(W.Range);
            if(w)
                W.CastIfHitchanceEquals(Target, HitChance.Medium);
            
        }
    }
}
