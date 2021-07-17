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
    [StaticConstructorOnStartup]
    public class Comp_HeatRelease : Comp_ShieldNet
    {
        private bool isUnderShield;

        private CompHeatPusher heatPusher => parent.TryGetComp<CompHeatPusher>();
        private CompPowerTrader powerTrader => parent.TryGetComp<CompPowerTrader>();
        private CompExplosive explosive => parent.TryGetComp<CompExplosive>();

        public float GetTemp => parent.AmbientTemperature;

        public bool Active => (powerTrader == null || powerTrader.PowerOn) && !IsUnderShield;

        public bool IsUnderShield
        {
            get
            {
                if (Props.canBeUnderShield)
                {
                    return false;
                }
                else
                {
                    return isUnderShield;
                }
            }
            set
            {
                isUnderShield = value;
            }
        }

        public float SetHeat
        {
            get 
            { 
                return heatPusher.Props.heatPerSecond;
            }
            set
            {
                heatPusher.Props.heatPerSecond = value;
                UpdatePowerUsage();
            }
        }

        public void UpdatePowerUsage()
        {
            powerTrader.Props.basePowerConsumption = GetTemp * Props.powerPerDegree;
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

        }
    }
}
