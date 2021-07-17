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
    public class ShieldNetwork
    {
        public MapComp_ShieldNetManager manager;
        public ShieldStructureSet networkSet = new ShieldStructureSet();

        public int networkID = -1;

        public ShieldNetwork(MapComp_ShieldNetManager manager)
        {
            this.manager = manager;
        }

        public ShieldNetwork(Comp_ShieldNet root, MapComp_ShieldNetManager manager)
        {
            this.manager = manager;
            manager.MakeNewNetwork(root, this);
            manager.RegisterNetwork(this);
        }

        public void Tick()
        {

        }

        private void ShieldTick()
        {

        }

        public List<IntVec3> NetworkCells()
        {
            List<IntVec3> cells = new List<IntVec3>();
            foreach (Comp_ShieldNet comp in networkSet.fullList)
            {
                cells.AddRange(comp.InnerConnectionCells);
            }
            return cells;
        }

        public void AddStructure(Comp_ShieldNet from)
        {
            from.Network = null;
            ShieldNetwork newNet = null;
            foreach(Comp_ShieldNet root in from.shieldStructureSet.fullList)
            {
                if(root.Network != newNet)
                {
                    newNet = root.Network = new ShieldNetwork(root, manager);
                }
            }
        }

        public bool ValidFor(ShieldNetMode mode, out string reason)
        {
            reason = string.Empty;
            switch (mode)
            {
                case ShieldNetMode.Shield:
                    reason = "Shield Generator has no Cooling";
                    return networkSet.fullList.Any(x => x.NetworkMode == ShieldNetMode.Power || x.NetworkMode == ShieldNetMode.Vent);
                case ShieldNetMode.Vent:
                    reason = "Cooler has nothing to cool";
                    return networkSet.fullList.Any(x => x.NetworkMode == ShieldNetMode.Shield || x.NetworkMode == ShieldNetMode.Power);
                case ShieldNetMode.Power:
                    reason = "Power Gen has nothing to power";
                    return networkSet.fullList.Any(x => x.NetworkMode == ShieldNetMode.Shield || x.NetworkMode == ShieldNetMode.Vent);
            }
            return true;
        }
    }
}
