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
    public class Graphic_LinkedShieldNetOverlay : Graphic_LinkedShieldNet
    {
        public Graphic_LinkedShieldNetOverlay() { }

        public Graphic_LinkedShieldNetOverlay(Graphic subGraphic) : base(subGraphic)
        {
            this.subGraphic = subGraphic;
        }

        public override bool ShouldLinkWith(IntVec3 c, Thing parent)
        {
            return base.ShouldLinkWith(c, parent);
        }

        public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing parent, float extraRotation)
        {
            Graphics.DrawMesh(this.MeshAt(rot), loc, Quaternion.identity, LinkedDrawMatFrom(parent, loc.ToIntVec3()), 0);
            for (int i = 0; i < 4; i++)
            {
                IntVec3 cell = parent.Position + GenAdj.CardinalDirections[i];
                if (cell.InBounds(parent.Map) && ShouldLinkWith(cell, parent))
                {
                    Graphics.DrawMesh(this.MeshAt(rot), cell.ToVector3Shifted(), Quaternion.identity, LinkedDrawMatFrom(parent, cell), 0);
                }
            }
        }

        public override void Print(SectionLayer layer, Thing thing)
        {
            var comp = thing.TryGetComp<Comp_ShieldNet>();
            foreach (IntVec3 cell in comp.InnerConnectionCells)
            {
                Vector3 vector = cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
                Printer_Plane.PrintPlane(layer, vector, Vector2.one, base.LinkedDrawMatFrom(thing, cell), 0f, false, null, null, 0.01f, 0f);
            }
        }

        public Graphic_LinkedShieldNetOverlay ColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
        {
            return new Graphic_LinkedShieldNetOverlay(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo))
            {
                data = this.data
            };
        }
    }
}
