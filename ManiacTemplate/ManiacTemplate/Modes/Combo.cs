using HesaEngine.SDK;
using static ManiacTemplate.SpellManager;
using static ManiacTemplate.MenuManager;

namespace ManiacTemplate.Modes
{
    public static class Combo
    {
        public static void DoCombo()
        {
            var q = comboMenu.GetCheckbox("useQ") && Q.IsReady();
            var w = comboMenu.GetCheckbox("useW") && W.IsReady();
            var e = comboMenu.GetCheckbox("useE") && E.IsReady();
            var r = comboMenu.GetCheckbox("useR") && R.IsReady();
            var target = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);
            
            if (target != null)
            {
                if (w)
                {
                    W.CastIfHitchanceEquals(target, HitChance.Medium);
                }
            }
        }
    }
}
