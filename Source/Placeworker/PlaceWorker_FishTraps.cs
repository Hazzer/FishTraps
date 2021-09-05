using System;
using Verse;
using UnityEngine;

namespace FishTraps
{
    class PlaceWorker_FishTraps : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            Color color = new Color(0f, 0.6f, 0f);
            GenDraw.DrawRadiusRing(center, 10f, color, (Func<IntVec3, bool>)null);
        }

        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
			foreach (IntVec3 item in GenAdj.CellsOccupiedBy(loc, rot, checkingDef.Size))
			{
				if (!map.terrainGrid.TerrainAt(item).IsWater)
				{
					return new AcceptanceReport(Translator.Translate("WFFT_NeedsWater"));
				}
			}
			foreach (Thing item2 in GenRadial.RadialDistinctThingsAround(loc, map, 10f, true))
			{
				Building val = item2 as Building;
				if (val != null && IsFishTraps(val.def))
				{
					return new AcceptanceReport(Translator.Translate("WFFT_NeedsDistance"));
				}
				Thing val2 = item2;
				if (val2 != null && (val2.def.IsBlueprint || val2.def.IsFrame) && IsFishTraps(val2.def.entityDefToBuild))
				{
					return new AcceptanceReport(Translator.Translate("WFFT_NeedsDistance"));
				}
			}
            return true;
		}

		private bool IsFishTraps (Def def)
		{
			return def.defName == "WFFT_FishTrap" || def.defName == "WFFT_FishNet";
		}
    }
}
