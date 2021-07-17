using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using RimWorld;
using Verse;

namespace ShieldGenerators
{
    public class Building_ShieldNetConduit : Building
    {
        public override Graphic Graphic => base.Graphic;

        public Comp_ShieldConduit ConduitComp => this.TryGetComp<Comp_ShieldConduit>();

        public override void Draw()
        {
            base.Draw();
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
        }
    }
}
