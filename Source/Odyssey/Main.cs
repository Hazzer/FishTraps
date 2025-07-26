using System;
using System.Reflection;
using FishTraps.Spawner;
using HarmonyLib;
using Verse;

namespace Odyssey
{
  [StaticConstructorOnStartup]
  public static class Main
  {
    static Main()
    {

      var harmony = new Harmony("Harmony_FishTraps_Odyssey");
      try
      {
        harmony.PatchAll(Assembly.GetExecutingAssembly());
      }
      catch (Exception e)
      {
        Log.Error($"Exception during init: {e}");
      }
    }
  }
  
  [HarmonyPatch(typeof(SpawnerSelector), nameof(SpawnerSelector.GetAdapter))]
  class InsertAdapterPatch
  {
    static void Postfix(ref SpawnerAdapter __result)
    {
      __result = new OdysseySpawner();
    }
  }
}