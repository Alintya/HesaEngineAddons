using System;
using HesaEngine.SDK;
using HesaEngine.SDK.Enums;
using HesaEngine.SDK.GameObjects;
using static ManiacTemplate.MenuManager;

namespace ManiacTemplate
{
    public static class SpellManager
    {
        public static Spell Q, W, E, R;

        public static void LoadSpells()
        {
            Q = new Spell(SpellSlot.Q);         //Active: Annie's E    
            W = new Spell(SpellSlot.W, 1200);   //SkillShot: Ezreal's Q
            E = new Spell(SpellSlot.E, 1500);   //Charged: Xerath's Q
            R = new Spell(SpellSlot.R, 1000);   //Targeted: Veigar's R
            

            W.SetSkillshot(delay: 0.5f, width: 100, speed: 2000, collision: true, type: SkillshotType.SkillshotCone);
            E.SetCharged(spellName: "", buffName: "", minRange: 100, maxRange: 1000, deltaT: 14.2f);
            R.SetTargetted(delay: 1, speed: 2000);




            //Do the same with QPred, EPred, RPred, always depending on what they are going to collide.
            //Examples: 
            //Jhin's W. Collides with Heroes, and YasuoWall. But not with Minions.
            //Ezreal's W collides with YasuoWall
            //Ezreal's Q collides with Heroes, Minions, YasuoWall
            Main.WPred = new PredictionInput
            {
                Delay = W.Delay,
                Radius = W.Width,
                Speed = W.Speed,
                Type = W.Type,
                CollisionObjects = new[]
                {
                    CollisionableObjects.Heroes,
                    CollisionableObjects.Minions,
                    CollisionableObjects.YasuoWall
                }
            };
        }
    }
}