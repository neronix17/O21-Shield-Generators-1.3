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
    public class Comp_ShieldNet : ThingComp
    {
        public CompProperties_Shield Props => (CompProperties_Shield)props;

        public CompPowerTrader compPower;
        public CompFlickable compFlick;
        public MapComp_ShieldNetManager shieldNetManager;
        public ShieldStructureSet shieldStructureSet;

        public ShieldNetwork network;
        public List<IntVec3> conduitExtensionCells = new List<IntVec3>();
        private List<IntVec3> cardinalCells = new List<IntVec3>();

        public ShieldNetMode NetworkMode => Props.shieldNetMode;
        public bool HasConnection => shieldStructureSet.conduits.Any();

        private static bool debugConnectionCells = false;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            compPower = parent.TryGetComp<CompPowerTrader>();
            compFlick = parent.TryGetComp<CompFlickable>();

            shieldNetManager = parent.Map.GetComponent<MapComp_ShieldNetManager>();
            shieldNetManager.mainStructureSet.AddNewStructure(this);
            UpdateConnections(out ShieldNetwork n);
        }

        public void UpdateConnections(out ShieldNetwork network)
        {
            shieldStructureSet = new ShieldStructureSet(this);
            network = Network = new ShieldNetwork(this, shieldNetManager);
        }

        public ShieldNetwork Network
        {
            get => network;
            set
            {
                if(network != null && network != value)
                {
                    shieldNetManager.DeregisterNetwork(network);
                }
                network = value;
            }
        }

        public virtual IEnumerable<IntVec3> InnerConnectionCells => parent.OccupiedRect().Cells;

        public virtual IEnumerable<IntVec3> CardinalConnectionCells
        {
            get
            {
                if (cardinalCells.NullOrEmpty())
                {
                    foreach (IntVec3 c in GenAdj.CellsAdjacentCardinal(parent))
                    {
                        if (InnerConnectionCells.Any(v => v.AdjacentToCardinal(c)))
                        {
                            cardinalCells.Add(c);
                        }
                    }
                }
                return cardinalCells;
            }
        }

        public bool ConnectsTo(Comp_ShieldNet other)
        {
            return CardinalConnectionCells.Any(other.InnerConnectionCells.Contains);
        }

        public virtual void StructureSetOnAdd(Comp_ShieldNet tnw, IntVec3 cell)
        {
            conduitExtensionCells.AddRange(CardinalConnectionCells.Intersect(tnw.InnerConnectionCells));
        }

        public virtual void StructureSetOnRemove(Comp_ShieldNet tnw)
        {
            conduitExtensionCells.RemoveAll(c => tnw.InnerConnectionCells.Contains(c));
        }

        public void PrintForGrid(SectionLayer layer)
        {
            ShieldGraphics.shieldNetworkPipesOverlay.Print(layer, this.parent);
        }

        public override void PostPrintOnto(SectionLayer layer)
        {
            base.PostPrintOnto(layer);
            ShieldGraphics.shieldNetworkPipes.Print(layer, parent);
        }

        public override void PostDraw()
        {
            base.PostDraw();
            //if (!Network.ValidFor(Props.shieldNetMode, out string reason))
            //{
            //    Material mat = MaterialPool.MatFrom(ShieldGraphics.missingConnection, ShaderDatabase.MetaOverlay, Color.white);
            //    float num = (Time.realtimeSinceStartup + 397f * (float)(parent.thingIDNumber % 571)) * 4f;
            //    float num2 = ((float)Math.Sin((double)num) + 1f) * 0.5f;
            //    num2 = 0.3f + num2 * 0.7f;
            //    Material material = FadedMaterialPool.FadedVersionOf(mat, num2);
            //    var c = parent.TrueCenter();
            //    Graphics.DrawMesh(MeshPool.plane08, new Vector3(c.x, AltitudeLayer.MetaOverlays.AltitudeFor(), c.z), Quaternion.identity, material, 0);
            //}
            //if (debugConnectionCells && Find.Selector.IsSelected(parent))
            //{
            //    GenDraw.DrawFieldEdges(conduitExtensionCells, Color.cyan);
            //    GenDraw.DrawFieldEdges(InnerConnectionCells.ToList(), Color.magenta);
            //    GenDraw.DrawFieldEdges(CardinalConnectionCells.ToList(), Color.green);
            //}
        }

        //public override string CompInspectStringExtra()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (!Network.ValidFor(Props.shieldNetMode, out string reason))
        //    {
        //        sb.AppendLine("No connection to grid".Translate() + ":");
        //        if (!reason.NullOrEmpty())
        //        {
        //            sb.AppendLine("    " + reason.Translate());
        //        }
        //    }
        //    if (DebugSettings.godMode)
        //    {
        //        sb.AppendLine("NetworkID: " + Network.networkID);
        //        sb.AppendLine("Has Connection: " + HasConnection);
        //    }
        //    return sb.ToString().TrimStart().TrimEndNewlines();
        //}

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo g in base.CompGetGizmosExtra())
            {
                yield return g;
            }
        }
    }

    public enum ShieldNetMode
    {
        None,
        Shield,
        Vent,
        Power
    }
}
