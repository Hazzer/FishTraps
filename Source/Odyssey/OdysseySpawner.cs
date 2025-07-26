using FishTraps;
using FishTraps.Options;
using FishTraps.Spawner;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Odyssey
{
  public class OdysseySpawner : SpawnerAdapter
  {
    private FishyBuildings buildingType;

    public void PostSpawnSetup(Thing parent, FishyCompProperties fishyCompProps)
    {
      this.buildingType = fishyCompProps.buildingType;
      //Do nothing
    }

    public void SpawnFish(Thing parent)
    {
      var cell = parent.Position;
      var waterBody = cell.GetWaterBody(parent.Map);
      if (waterBody == null || waterBody.waterBodyType == WaterBodyType.None)
      {
        return;
      }


      if (Rand.Chance(FishingUtility.PollutionToxfishChanceCurve.Evaluate(waterBody.PollutionPct)))
      {
        CatchFish(parent, ThingDefOf.Fish_Toxfish);
        return;
      }

      if (parent.Map.Biome.fishTypes.rareCatchesSetMaker != null &&
          (DebugSettings.alwaysRareCatches || parent.Map.waterBodyTracker.lastRareCatchTick == 0 ||
           GenTicks.TicksGame - parent.Map.waterBodyTracker
             .lastRareCatchTick > 300000) && Rand.Chance(0.01f))
      {
        var catches = parent.Map.Biome.fishTypes.rareCatchesSetMaker.root.Generate();
        if (catches.Any())
        {
          foreach (var item in catches)
          {
            GenPlace.TryPlaceThing(item, parent.Position, parent.Map, ThingPlaceMode.Near);
          }

          return;
        }
      }

      if ((!Rand.Chance(0.05f) || !waterBody.UncommonFish.TryRandomElement(out var result)) &&
          !waterBody.CommonFishIncludingExtras.TryRandomElement(out result))
      {
        //Fishu got away
        return;
      }

      CatchFish(parent, result);
    }

    private void CatchFish(Thing parent, ThingDef fishType)
    {
      var cell = parent.Position;
      float x = parent.Map.waterBodyTracker.FishPopulationAt(cell);
      float num = Mathf.Min(parent.Map.waterBodyTracker.FishPopulationAt(cell),
        Mathf.Min(FishingUtility.PopulationToFishYieldCurve.Evaluate(x) * FishAmount(buildingType)));
      int stackCount = Mathf.Max(1, Mathf.RoundToInt(num));
      Thing thing = ThingMaker.MakeThing(fishType);
      thing.stackCount = stackCount;
      if (GenPlace.TryPlaceThing(thing, parent.Position, parent.Map, ThingPlaceMode.Near))
      {
        SoundDefOf.Interact_CatchFish.PlayOneShot(new TargetInfo(cell, parent.Map));
        if (FishTrapsModSettings.odysseyNotifyWaterBodies)
        {
          parent.Map.waterBodyTracker?.Notify_Fished(cell, stackCount);
        }
      }
    }

    private static float FishAmount(FishyBuildings fishyBuildings)
    {
      return fishyBuildings == FishyBuildings.Net ? 1.2f : 0.6f;
    }
  }
}