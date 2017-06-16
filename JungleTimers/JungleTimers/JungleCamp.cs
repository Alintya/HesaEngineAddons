using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;
using HesaEngine.SDK.Enums;
using SharpDX;

namespace JungleTimers
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class JungleCamp
    {
        public bool Dead { get; set; }
        public GameMapId MapID { get; set; }

        public Vector2 MinimapPosition
        {
            get
            {
                // Well, dont ask
                Vector3 tmp = new Vector3(Position.X, Position.Z, Position.Y);
                return TacticalMap.WorldToMinimap(tmp);
            }
        }

        public string[] MobNames { get; set; }
        public int NextRespawnTime { get; set; }
        public List<string> ObjectsAlive { get; set; }
        public List<string> ObjectsDead { get; set; }
        public Vector3 Position { get; set; }
        public int RespawnTime { get; set; }
        public GameObjectTeam Team { get; set; }

        public JungleCamp(int respawnTime, Vector3 position, string[] mobNames, GameMapId mapID, GameObjectTeam team)
        {
            RespawnTime = respawnTime;
            Position = position;
            MobNames = mobNames;
            MapID = mapID;
            Team = team;

            ObjectsDead = new List<string>();
            ObjectsAlive = new List<string>();

        }
    }
}
