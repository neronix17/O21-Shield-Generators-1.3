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
    public class MapComp_ShieldNetManager : MapComponent
    {
        public List<ShieldNetwork> networks = new List<ShieldNetwork>();
        public Dictionary<ShieldNetwork, List<IntVec3>> networkCells = new Dictionary<ShieldNetwork, List<IntVec3>>();
        public ShieldStructureSet mainStructureSet = new ShieldStructureSet();
        public int masterID = -1;

        public bool[] shieldGrid;

        public static bool showNetworks = true;

        public MapComp_ShieldNetManager(Map map) : base(map)
        {
            shieldGrid = new bool[map.cellIndices.NumGridCells];
        }

        [TweakValue("MapComp_ShieldNetManager", 0f, 100f)]
        public static bool drawBool = false;

        public override void MapComponentUpdate()
        {
            base.MapComponentUpdate();
        }

        public override void MapComponentTick()
        {
            base.MapComponentTick();
            foreach(ShieldNetwork network in networks)
            {
                network.Tick();
            }
        }

        public ShieldNetwork MakeNewNetwork(Comp_ShieldNet root, ShieldNetwork forNetwork = null)
        {
            ShieldNetwork newNet = forNetwork ?? new ShieldNetwork(this);
            HashSet<Comp_ShieldNet> closedSet = new HashSet<Comp_ShieldNet>();
            HashSet<Comp_ShieldNet> openSet = new HashSet<Comp_ShieldNet>() { root };
            HashSet<Comp_ShieldNet> currentSet = new HashSet<Comp_ShieldNet>();
            while(openSet.Count > 0)
            {
                foreach(Comp_ShieldNet item in openSet)
                {
                    item.Network = newNet;
                    newNet.AddStructure(item);
                    closedSet.Add(item);
                }
                HashSet<Comp_ShieldNet> hashSet = currentSet;
                currentSet = openSet;
                openSet = hashSet;
                openSet.Clear();
                foreach(Comp_ShieldNet shieldNet in currentSet)
                {
                    foreach(IntVec3 c in shieldNet.CardinalConnectionCells)
                    {
                        List<Thing> thingList = c.GetThingList(shieldNet.parent.Map);
                        for (int i = 0; i < thingList.Count(); i++)
                        {
                            var newShieldNet = thingList[i].TryGetComp<Comp_ShieldNet>();
                            if(newShieldNet != null && !closedSet.Contains(newShieldNet) && newShieldNet.ConnectsTo(shieldNet))
                            {
                                map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Buildings);
                                map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Things);
                                shieldNet.shieldStructureSet.AddNewStructure(newShieldNet, c);
                                newShieldNet.shieldStructureSet.AddNewStructure(shieldNet, c + IntVec3.North);
                                openSet.Add(newShieldNet);
                                break;
                            }
                        }
                    }
                }
            }
            return newNet;
        }

        public bool ConnectionAt(IntVec3 c)
        {
            return shieldGrid[map.cellIndices.CellToIndex(c)];
        }

        public void RegisterNetwork(ShieldNetwork network)
        {
            network.networkID = masterID += 1;
            networks.Add(network);
            networkCells.Add(network, network.NetworkCells());
            for (int i = 0; i < networkCells[network].Count; i++)
            {
                shieldGrid[map.cellIndices.CellToIndex(networkCells[network][i])] = true;
            }
        }

        public void DeregisterNetwork(ShieldNetwork network)
        {
            if (networks.Contains(network))
            {
                for (int i = 0; i < networkCells[network].Count; i++)
                {
                    shieldGrid[map.cellIndices.CellToIndex(networkCells[network][i])] = false;
                }
            }
            networks.Remove(network);
            networkCells.Remove(network);
        }
    }
}
