using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FishTraps.HarmonyPatches
{
    [HarmonyPatch(typeof(TerrainGrid), nameof(TerrainGrid.SetTerrain))]
    class TerrainChangePatch
    {
        static void Postfix(TerrainGrid __instance, IntVec3 c, TerrainDef newTerr)
        {
            if (Current.ProgramState == ProgramState.Playing)
            {
                Find.SignalManager.SendSignal(new RimWorld.Signal("WFFT_TerrainChange", c.Named("pos"), __instance.Named("grid")));
            }
        }
    }
}
