using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FishTraps
{
    [StaticConstructorOnStartup]
    public static class Main
    {
        static Main()
        {
            var harmony = new Harmony("Harmony_FishTraps");
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
}
