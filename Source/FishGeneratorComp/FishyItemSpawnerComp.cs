using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using VCE_Fishing;
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
					if(terrainDef.defName == "WaterOceanDeep" || terrainDef.defName == "WaterOceanShallow")
                    {
						return true;
                    }
				}
				return false;
			}
        }

		public FishyItemSpawnerComp() : base()
        {

        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
			BiomeDef biome = parent.Map.Biome;
			Log.Message("1st for");
			foreach (FishDef item in from element in DefDatabase<FishDef>.AllDefs
									 where FishyChecker.IsGoodSizeOfFish(Props.fishSizeCategory, element.fishSizeCategory)
									 select element)
			{
				Log.Message("Fishfor");
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

			Log.Message("Done");
		}

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "DEV: Spawn items";
			command_Action.action = delegate
			{
				SpawnItems();
			};
			yield return command_Action;

			Command_Action damageTrap = new Command_Action();
			damageTrap.defaultLabel = "DEV: damage trap";
			damageTrap.action = delegate
			{
				float trapDamage = Rand.Range(0f, Props.intervalDmgTreshhold);
				parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, parent.HitPoints * trapDamage));
			};
			yield return damageTrap;
		}

		public override void CompTickRare()
		{

			ticksPassed += 250;
			if (ticksPassed >= Props.spawnInterval)
			{
				SpawnItems();
				ticksPassed -= Props.spawnInterval;
				float trapDamage = Rand.Range(0f, Props.fishedDmgThreshhold);
				parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, parent.HitPoints * trapDamage));
				
			} else if(Rand.MTBEventOccurs(Props.dmgInterval, 60000, 30000))
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
			if (fishInThisZone.TryRandomElement(out FishDef result))
			{
                
				int stackCount = result.baseFishingYield;
				Thing thing = ThingMaker.MakeThing(result.thingDef);
				thing.stackCount = stackCount;
				GenPlace.TryPlaceThing(thing, parent.Position, parent.Map, ThingPlaceMode.Near);
			}
		}

		public override string CompInspectStringExtra()
		{
			return "NextSpawnedResourceIn".Translate() + ": " + (Props.spawnInterval - ticksPassed).ToStringTicksToPeriod();	

		}
	}
}
