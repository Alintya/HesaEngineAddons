using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;

namespace AwarenessEngine
{
    interface IPlugin
    {
        string Name { get; }

        bool Enabled { get; set; }

        Menu Menu { get; set; }

        void InitializePlugin();
    }
}
