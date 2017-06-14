using System.Linq;
using HesaEngine.SDK;
using SharpDX;
using static ManiacTemplate.MenuManager;
using static ManiacTemplate.SpellManager;

namespace ManiacTemplate.Modes
{
    public static class LaneClear
    {
        public static void DoLaneClear()
        {
            var q = laneclearMenu.GetCheckbox("useQ") && Q.IsReady();
            var w = laneclearMenu.GetCheckbox("useW") && W.IsReady();
            var e = laneclearMenu.GetCheckbox("useE") && E.IsReady();
            var r = laneclearMenu.GetCheckbox("useR") && R.IsReady();

            var minion = ObjectManager.MinionsAndMonsters.Enemy.Where(x => x.IsValidTarget(W.Range));

            foreach (var m in minion)
            {
                if (w)
                {
                    W.CastIfHitchanceEquals(m, HitChance.Medium);
                }
            }
        }
    }
}
