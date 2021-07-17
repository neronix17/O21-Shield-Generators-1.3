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
    public class Comp_ShieldConduit : Comp_ShieldNet
    {
        public override void PostExposeData()
        {
            base.PostExposeData();
        }
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);
        }

        public override void StructureSetOnAdd(Comp_ShieldNet net, IntVec3 cell)
        {
            if (!(net is Comp_ShieldConduit))
            {
                base.StructureSetOnAdd(net, cell);
            }
        }
    }
}
