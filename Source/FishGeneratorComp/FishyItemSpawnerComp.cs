using FishTraps.Options;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using VCE_Fishing;
using VCE_Fishing.Options;
using Verse;

namespace FishTraps
{
    class FishyItemSpawnerComp : ThingComp
    {
        private int ticksPassed;

        private FishyCompProperties Props => (FishyCompProperties)base.props;

        private List<FishDef> fishInThisZone = new List<FishDef>();

        private bool IsOcean
        {
            get
            {
                foreach (IntVec3 item in GenAdj.CellsOccupiedBy(parent.Position, parent.Rotation, parent.def.Size))
                {
                    TerrainDef terrainDef = parent.Map.terrainGrid.TerrainAt(item);
                    if (terrainDef.defName == "WaterOceanDeep" || terrainDef.defName == "WaterOceanShallow")
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private int SpawnInterval
        {
            get
            {
                switch (Props.buildingType)
                {
                    case FishyBuildings.Trap:
                        return FishTrapsModSettings.trapSpawnInterval;
                    case FishyBuildings.Net:
                        return FishTrapsModSettings.netSpawnInterval;
                    default:
                        return Props.spawnInterval;
                }
            }
        }

        private int DmgInterval
        {
            get
            {
                switch (Props.buildingType)
                {
                    case FishyBuildings.Trap:
                        return FishTrapsModSettings.trapDmgInterval;
                    case FishyBuildings.Net:
                        return FishTrapsModSettings.netDmgInterval;
                    default:
                        return Props.dmgInterval;
                }
            }
        }
        public FishyItemSpawnerComp() : base()
        {

        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            BiomeDef biome = parent.Map.Biome;
            foreach (FishDef item in from element in DefDatabase<FishDef>.AllDefs
                                     where FishyChecker.IsGoodSizeOfFish(Props.fishSizeCategory, element.fishSizeCategory)
                                     select element)
            {
                foreach (String biomeName in item.allowedBiomes)
                {
                    if (BiomeRepo.CheckBiome(biome, biomeName))
                    {
                        if (IsOcean && item.canBeSaltwater)
                        {
                            fishInThisZone.Add(item);
                        }
                        if (!IsOcean && item.canBeFreshwater)
                        {
                            fishInThisZone.Add(item);
                        }
                    }
                }
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (Prefs.DevMode)
            {
                Command_Action command_Action = new Command_Action
                {
                    defaultLabel = "DEV: Spawn items",
                    action = delegate
                    {
                        SpawnItems();
                    }
                };
                yield return command_Action;

                Command_Action damageTrap = new Command_Action
                {
                    defaultLabel = "DEV: damage trap",
                    action = delegate
                    {
                        float trapDamage = Rand.Range(0f, Props.intervalDmgTreshhold);
                        parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, parent.HitPoints * trapDamage));
                    }
                };
                yield return damageTrap;

            }
        }

        public override void CompTickRare()
        {

            ticksPassed += 250;
            if (ticksPassed >= SpawnInterval)
            {
                SpawnItems();
                ticksPassed -= SpawnInterval;
                float trapDamage = Rand.Range(0f, Props.fishedDmgThreshhold);
                parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, parent.HitPoints * trapDamage));

            }
            else if (Rand.MTBEventOccurs(DmgInterval, 60000, 30000))
            {
                float trapDamage = Rand.Range(0f, Props.intervalDmgTreshhold);
                parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, parent.HitPoints * trapDamage));
            }
        }
        public override void PostExposeData()
        {
            Scribe_Values.Look(ref ticksPassed, "ticksPassed", 0);
        }

        private void SpawnItems()
        {
            float specialChance = 1f - (VCE_Fishing_Settings.VCEF_chanceForSpecials / 100.0f);
            if (Rand.Value <= specialChance)
            {
                if (fishInThisZone.TryRandomElement(out FishDef result))
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
                    .Where<FishDef>(element => element.fishSizeCategory == FishSizeCategory.Special)
                    .ToList()
                    .RandomElementByWeight<FishDef>(s => s.commonality);
                Thing thing = ThingMaker.MakeThing(thingDef.thingDef);
                thing.stackCount = thingDef.baseFishingYield;
                GenPlace.TryPlaceThing(thing, parent.Position, parent.Map, ThingPlaceMode.Near);
            }
        }

        public override string CompInspectStringExtra()
        {
            return "NextSpawnedResourceIn".Translate() + ": " + (SpawnInterval - ticksPassed).ToStringTicksToPeriod();

        }
    }
}
