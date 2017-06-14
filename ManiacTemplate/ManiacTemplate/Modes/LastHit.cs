using System.Linq;
using HesaEngine.SDK;
using static ManiacTemplate.MenuManager;
using static ManiacTemplate.SpellManager;

namespace ManiacTemplate.Modes
{
    public static class LastHit
    {
        public static void DoLastHit()
        {
            var q = lasthitMenu.GetCheckbox("useQ") && Q.IsReady();
            var w = lasthitMenu.GetCheckbox("useW") && W.IsReady();
            var e = lasthitMenu.GetCheckbox("useE") && E.IsReady();
            var r = lasthitMenu.GetCheckbox("useR") && R.IsReady();
            var minion = ObjectManager.MinionsAndMonsters.Enemy.Where(x => x.IsValidTarget(W.Range));

            foreach (var m in minion)
            {
                if (w && W.GetDamage(m) >= m.Health)
                {
                    W.CastIfHitchanceEquals(m, HitChance.Medium);
                }
            }
        }
    }
}
