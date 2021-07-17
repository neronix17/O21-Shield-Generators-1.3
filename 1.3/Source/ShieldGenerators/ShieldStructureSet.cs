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
    public class ShieldStructureSet
    {
        public Comp_ShieldNet parent;
        public List<Comp_ShieldConduit> conduits = new List<Comp_ShieldConduit>();
        public List<Comp_ShieldBuilding> shields = new List<Comp_ShieldBuilding>();
        public List<Comp_HeatRelease> vents = new List<Comp_HeatRelease>();
        public List<Comp_ShieldEnergyGen> generators = new List<Comp_ShieldEnergyGen>();

        public List<Comp_ShieldNet> fullList = new List<Comp_ShieldNet>();

        public ShieldStructureSet()
        {

        }

        public ShieldStructureSet(Comp_ShieldNet parent)
        {
            this.parent = parent;
        }

        public void AddNewStructure(Comp_ShieldNet netBuilding)
        {
            if (!fullList.Contains(netBuilding))
            {
                if(netBuilding is Comp_ShieldConduit conduit)
                {
                    conduits.Add(conduit);
                }
                if(netBuilding is Comp_ShieldBuilding shield)
                {
                    shields.Add(shield);
                }
                if(netBuilding is Comp_HeatRelease vent)
                {
                    vents.Add(vent);
                }
                if(netBuilding is Comp_ShieldEnergyGen gen)
                {
                    generators.Add(gen);
                }
                fullList.Add(netBuilding);
            }
        }

        public void AddNewStructure(Comp_ShieldNet netBuilding, IntVec3 cell)
        {
            if(!fullList.Contains(netBuilding) && netBuilding != null)
            {
                parent?.StructureSetOnAdd(netBuilding, cell);
                AddNewStructure(netBuilding);
                netBuilding.shieldStructureSet.AddNewStructure(parent, cell + parent?.parent.Position.PositionOffset(cell) ?? IntVec3.Invalid);
            }
        }

        public void RemoveStructure(Comp_ShieldNet netBuilding)
        {
            if (fullList.Contains(netBuilding))
            {
                if (netBuilding is Comp_ShieldConduit conduit)
                {
                    conduits.Remove(conduit);
                }
                if (netBuilding is Comp_ShieldBuilding shield)
                {
                    shields.Remove(shield);
                }
                if (netBuilding is Comp_HeatRelease vent)
                {
                    vents.Remove(vent);
                }
                if (netBuilding is Comp_ShieldEnergyGen gen)
                {
                    generators.Remove(gen);
                }
                fullList.Remove(netBuilding);
            }
        }

        public void ParentDestroyed()
        {
            foreach(Comp_ShieldNet net in fullList)
            {
                net.shieldStructureSet.RemoveStructure(parent);
            }
        }

        public bool Empty => fullList.NullOrEmpty();
    }
}
