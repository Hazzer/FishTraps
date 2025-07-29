using System.Collections.Generic;
using System.Linq;
using FishTraps;
using FishTraps.Spawner;
using RimWorld;
using VCE_Fishing;
using VCE_Fishing.Options;
using Verse;

namespace Vfe
{
  public class VfeSpawner : SpawnerAdapter
  {
    private readonly List<FishDef> _fishInThisZone = new List<FishDef>();

    private static bool IsOcean(Thing parent)
    {
      return GenAdj.CellsOccupiedBy(parent.Position, parent.Rotation, parent.def.Size)
        .Select(item => parent.Map.terrainGrid.TerrainAt(item))
        .Any(terrainDef => terrainDef.defName == "WaterOceanDeep" || terrainDef.defName == "WaterOceanShallow");
    }


    public void PostSpawnSetup(Thing parent, FishyCompProperties fishyCompProps)
    {
      BiomeDef biome = parent.Map.Biome;
      var isOcean = IsOcean(parent);
      foreach(var item in DefDatabase<FishDef>.AllDefs.Where(def => IsGoodSizeOfFish(fishyCompProps.buildingType, def.fishSizeCategory)))
      {
        foreach (var _ in item.allowedBiomes.Where(biomeName => BiomeRepo.CheckBiome(biome, biomeName)))
        {
          if (isOcean && item.canBeSaltwater)
          {
            _fishInThisZone.Add(item);
          }

          if (!isOcean && item.canBeFreshwater)
          {
            _fishInThisZone.Add(item);
          }
        }
      }
    }

    public void SpawnFish(Thing parent)
    {
      float specialChance = 1f - (VCE_Fishing_Settings.VCEF_chanceForSpecials / 100.0f);
      if (Rand.Value <= specialChance)
      {
        if (_fishInThisZone.TryRandomElement(out FishDef result))
        {
          int stackCount = result.baseFishingYield;
          Thing thing = ThingMaker.MakeThing(result.thingDef);
          thing.stackCount = stackCount;
          GenPlace.TryPlaceThing(thing, parent.Position, parent.Map, ThingPlaceMode.Near);
        }
      }
      else
      {
        FishDef thingDef = DefDatabase<FishDef>.AllDefs
          .Where(element => element.fishSizeCategory == FishSizeCategory.Special)
          .AsEnumerable()
          .RandomElementByWeight(s => s.commonality);
        Thing thing = ThingMaker.MakeThing(thingDef.thingDef);
        thing.stackCount = thingDef.baseFishingYield;
        GenPlace.TryPlaceThing(thing, parent.Position, parent.Map, ThingPlaceMode.Near);
      }
    }

    private static bool IsGoodSizeOfFish(FishyBuildings reference, FishSizeCategory checkedSize)
    {
      switch (reference)
      {
        case FishyBuildings.Trap: return checkedSize == FishSizeCategory.Small;
        case FishyBuildings.Net: return checkedSize != FishSizeCategory.Special;

        default:
          return false;
      }
    }
  }
}