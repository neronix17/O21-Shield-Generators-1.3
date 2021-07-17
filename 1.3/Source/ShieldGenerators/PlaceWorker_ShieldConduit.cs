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
    public class PlaceWorker_ShieldConduit : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            if(loc.GetThingList(map).Any(p => p.TryGetComp<Comp_ShieldNet>() != null))
            {
                return false;
            }
            return true;
        }
    }
}
