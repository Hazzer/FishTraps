using RimWorld;
using System;
using System.Collections.Generic;
using VCE_Fishing;
using Verse;

namespace FishTraps
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

        public static bool CheckBiome(BiomeDef mapBiome, String fishBiome)
        {
            if (biomesMap.TryGetValue(fishBiome, out List<String> biomes))
            {
                return biomes.Contains(mapBiome.defName);
            }
            return false;

        }
    }
}
