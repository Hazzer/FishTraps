using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace FishTraps
{
  [StaticConstructorOnStartup]
  public static class Main
  {
    static Main()
    {
      if (LoadedModManager.RunningMods.FirstOrDefault(m => m.PackageId == "vanillaexpanded.vcef") == null &&
          !ModsConfig.OdysseyActive)
      {
        Log.Error("Mod require DLC Odyssey or Vanilla Fishing Expanded");
        return;
      }

      var harmony = new Harmony("Harmony_FishTraps");
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
}