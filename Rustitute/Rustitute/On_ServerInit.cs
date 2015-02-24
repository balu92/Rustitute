using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pluton.Events;
using UnityEngine;

namespace Rustitute
{
    partial class Rustitute
    {
        public void On_ServerInit()
        {
            DestroyArena();
            SpawnArena();
        }
    }
}
