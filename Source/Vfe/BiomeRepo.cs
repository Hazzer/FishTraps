using System;
using System.Collections.Generic;
using RimWorld;
using VCE_Fishing;
using Verse;

namespace Vfe
{
    class BiomeRepo
    {
        private static readonly Dictionary<String, List<String>> biomesMap = new Dictionary<String, List<String>>();
        public static void Init()
        {
            foreach (BiomeTempDef tempBiome in DefDatabase<BiomeTempDef>.AllDefs)
            {
                biomesMap.Add(tempBiome.biomeTempLabel, tempBiome.biomes);
            }
            Log.Message($"Biomes Map after Init: {biomesMap.Count}");

        }

        public static bool CheckBiome(BiomeDef mapBiome, string fishBiome)
        {
            return biomesMap.TryGetValue(fishBiome, out List<string> biomes) && biomes.Contains(mapBiome.defName);
        }
    }
}
