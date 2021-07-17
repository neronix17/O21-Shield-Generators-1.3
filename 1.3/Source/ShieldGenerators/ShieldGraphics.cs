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
    public class ShieldGraphics
    {
        static ShieldGraphics() { }

        public static readonly Graphic_LinkedShieldNetOverlay shieldNetworkPipesOverlay = new Graphic_LinkedShieldNetOverlay(GraphicDatabase.Get<Graphic_Single>("Things/ShieldGen/PowerConduit_OverlayAtlas", ShaderDatabase.Transparent, Vector2.one, new ColorInt(155, 255, 0).ToColor));
        public static readonly Graphic_LinkedShieldNetOverlay shieldNetworkPipesGlow = new Graphic_LinkedShieldNetOverlay(GraphicDatabase.Get<Graphic_Single>("Things/ShieldGen/PowerConduit_OverlayAtlas", ShaderDatabase.MoteGlow, Vector2.one, Color.white));
        public static readonly Graphic_LinkedShieldNet shieldNetworkPipes = new Graphic_LinkedShieldNet(GraphicDatabase.Get<Graphic_Single>("Things/ShieldGen/PowerConduit_Atlas", ShaderDatabase.Transparent, Vector2.one, Color.white));
        public static readonly Texture2D missingConnection = ContentFinder<Texture2D>.Get("UI/Icons/ShieldGen/ConnectionMissing", false);
    }
}
