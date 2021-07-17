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
    public class OverlayDrawHandler_ShieldNet
    {
        private static int lastShieldGridDrawFrame;

        public static void DrawShieldGridOverlayThisFrame()
        {
            lastShieldGridDrawFrame = Time.frameCount;
        }
        public static bool ShouldDrawShieldGrid => lastShieldGridDrawFrame + 1 >= Time.frameCount;
    }
}
