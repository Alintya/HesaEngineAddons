using System.Linq;
using HesaEngine.SDK;
using static ManiacTemplate.SpellManager;
using static ManiacTemplate.MenuManager;

namespace ManiacTemplate.Modes
{
    public static class Killsteal
    {
        public static void DoKS()
        {
            var q = Q.IsReady() && killstealMenu.GetCheckbox("useQ");
            var w = W.IsReady() && killstealMenu.GetCheckbox("useW");
            var e = E.IsReady() && killstealMenu.GetCheckbox("useE");
            var r = R.IsReady() && killstealMenu.GetCheckbox("useR");
            foreach (var enemy in ObjectManager.Heroes.Enemies.Where(x=> x.IsValidTarget(W.Range) && !x.IsDead && !x.IsZombie))
            {
                if (w && W.GetDamage(enemy) >= enemy.Health)
                    W.CastIfHitchanceEquals(enemy, HitChance.Medium);
            }
        }
    }
}
