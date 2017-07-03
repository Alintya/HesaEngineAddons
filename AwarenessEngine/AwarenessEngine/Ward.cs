using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;
using HesaEngine.SDK.Enums;
using HesaEngine.SDK.GameObjects;
using SharpDX;
using SharpDX.Direct3D9;

namespace AwarenessEngine
{
    public enum Type
    {
        SightWard,
        JammerDevice,
        YellowTrinket,
        BlueTrinket
    }

    public enum PinkColors
    {
        Pink,
        Red
    }

    public abstract class Ward
    {
        string FriendlyName { get; }
        string BaseSkinName { get; }
        string DetectingBuffName { get; }
        string DetectingSpellCastName { get; }
        string DetectingObjectName { get; }

        public Type Type { get; }
        public const int Radius = 65;

        private static readonly Vector2 HealthBarSize = new Vector2(64, 6);

        private static readonly RectangleF HealthBar = new RectangleF(-HealthBarSize.X / 2, -HealthBarSize.Y / 2, HealthBarSize.X, HealthBarSize.Y);

        private const int HealthBarBorderWidth = 1;
        private const int HealthBarPadding = 2;
        private static readonly Color HealthBarBackgroundColor = Color.Black;

        private static readonly Dictionary<GameObjectTeam, Tuple<Color, Color>> HealthBarColors =
            new Dictionary<GameObjectTeam, Tuple<Color, Color>>
            {
                {GameObjectTeam.Order, new Tuple<Color, Color>(Color.GreenYellow, Color.AntiqueWhite)},
                {GameObjectTeam.Chaos, new Tuple<Color, Color>(Color.OrangeRed, Color.AntiqueWhite)}
            };


        #region Constructor Properties

        public AIHeroClient Caster { get; private set; }
        public Obj_AI_Base Handle { get; set; }
        public Vector3 FakePosition { get; private set; }

        public int Duration { get; private set; }
        public int CreationTime { get; private set; }

        #endregion

        #region Wrapped Properties

        public Vector3 Position
        {
            get { return Handle != null ? Handle.Position : FakePosition; }
        }

        public Vector2 ScreenPosition
        {
            get { return Handle != null ? Handle.Position.WorldToScreen() : FakePosition.WorldToScreen(); }
        }

        public Vector2 MinimapPosition
        {
            get { return Handle != null ? Handle.Position.WorldToMinimap() : FakePosition.WorldToMinimap(); }
        }

        public bool IsVisible
        {
            get { return Handle != null && Handle.IsHPBarRendered; }
        }

        private GameObjectTeam _team;

        public GameObjectTeam Team
        {
            get => Handle?.Team ?? _team;
            set => _team = value;
        }

        public float MaxHealth
        {
            get
            {
                switch (Type)
                {
                    case Type.BlueTrinket:
                        return 1;
                    case Type.JammerDevice:
                        return 4;
                    default:
                        return 3;
                }
            }
        }

        #endregion

        public bool IsFakeWard => Handle == null;

        private Drawing.Text TextHandle { get; set; }

    }
}