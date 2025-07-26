using System;
using System.Reflection;
using FishTraps.Spawner;
using HarmonyLib;
using Verse;

namespace Vfe
{
  [StaticConstructorOnStartup]
  public static class Main
  {
    static Main()
    {
      var harmony = new Harmony("Harmony_FishTraps_Vfe");
      try
      {
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        BiomeRepo.Init();
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
      __result = new VfeSpawner();
    }
  }
}