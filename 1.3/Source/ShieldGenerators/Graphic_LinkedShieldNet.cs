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
    public class Graphic_LinkedShieldNet : Graphic_Linked
    {
        public Graphic_LinkedShieldNet() { }

        public Graphic_LinkedShieldNet(Graphic subGraphic)
        {
            this.subGraphic = subGraphic;
        }

        public override bool ShouldLinkWith(IntVec3 c, Thing parent)
        {
            return c.InBounds(parent.Map) && parent.Map.GetComponent<MapComp_ShieldNetManager>().ConnectionAt(c);
        }

        public override void Print(SectionLayer layer, Thing parent)
        {
            var comp = parent.TryGetComp<Comp_ShieldNet>();
            if (comp == null) return;
            IntVec3 parentPos = parent.Position;
            Printer_Plane.PrintPlane(layer, parentPos.ToVector3ShiftedWithAltitude(AltitudeLayer.FloorEmplacement), Vector2.one, LinkedDrawMatFrom(parent, parentPos));
            foreach (var pos in comp.conduitExtensionCells)
            {
                Printer_Plane.PrintPlane(layer, pos.ToVector3ShiftedWithAltitude(AltitudeLayer.FloorEmplacement), Vector2.one, LinkedDrawMatFrom(parent, pos));
            }
        }
    }
}
