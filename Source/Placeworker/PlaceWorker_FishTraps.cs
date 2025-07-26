using System;
using Verse;
using UnityEngine;
using FishTraps.Options;

namespace FishTraps
{
    class PlaceWorker_FishTraps : PlaceWorker
    {
        private const string NEED_DISTANCE = "WFFT_NeedsDistance";
        private const string NEED_WATER = "WFFT_NeedsWater";
        private const string FISH_TRAP = "WFFT_FishTrap";
        private const string FISH_NET = "WFFT_FishNet";

        private static int BuildRadius => FishTrapsModSettings.buildRadius;

        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            if (BuildRadius > 0)
            {
                Color color = new Color(0f, 0.6f, 0f);
                GenDraw.DrawRadiusRing(center, BuildRadius, color, (Func<IntVec3, bool>)null);
            }
        }

        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            foreach (IntVec3 item in GenAdj.CellsOccupiedBy(loc, rot, checkingDef.Size))
            {
                if (!map.terrainGrid.TerrainAt(item).IsWater)
                {
                    return new AcceptanceReport(Translator.Translate(NEED_WATER));
                }
            }
            foreach (Thing item2 in GenRadial.RadialDistinctThingsAround(loc, map, BuildRadius, true))
            {
                if (item2 is Building val && IsFishTraps(val.def))
                {
                    return new AcceptanceReport(Translator.Translate(NEED_DISTANCE));
                }
                Thing val2 = item2;
                if (val2 != null && (val2.def.IsBlueprint || val2.def.IsFrame) && IsFishTraps(val2.def.entityDefToBuild))
                {
                    return new AcceptanceReport(Translator.Translate(NEED_DISTANCE));
                }
            }
            return true;
        }

        private static bool IsFishTraps(Def def)
        {
            return def.defName == FISH_TRAP || def.defName == FISH_NET;
        }
    }
}
