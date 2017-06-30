using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;
using HesaEngine.SDK.GameObjects;

namespace AwarenessEngine.Plugins
{
    class WardTracker : IPlugin
    {
        public string Name => "Ward Tracker";

        public bool Enabled { get; set; }

        public Menu Menu { get; set; }

        public static Dictionary<Ward.Type, MenuCheckBox> EnabledWards { get; private set; }
        private static readonly List<Obj_AI_Base> WardsPlaced = new List<Obj_AI_Base>();
        public static Ward.PinkColors PinkColor { get; private set; }

        public void InitializePlugin()
        {
            if (!Enabled)
                return;
            // Init
        }
    }

}
