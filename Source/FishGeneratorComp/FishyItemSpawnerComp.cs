using FishTraps.Options;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using FishTraps.Spawner;
using VCE_Fishing;
using VCE_Fishing.Options;
using Verse;

namespace FishTraps
{
    class FishyItemSpawnerComp : ThingComp
    {
        private int ticksPassed;
        private bool disabled;
        private OverlayHandle? overlayHandle;

        private FishyCompProperties Props => (FishyCompProperties)props;

        private static SpawnerAdapter Spawner => SpawnerSelector.GetAdapter();

        private int SpawnInterval
        {
            get
            {
                switch (Props.buildingType)
                {
                    case FishyBuildings.Trap:
                        return FishTrapsModSettings.TrapSpawnIntervalInTicks;
                    case FishyBuildings.Net:
                        return FishTrapsModSettings.NetSpawnIntervalInTicks;
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
                        return FishTrapsModSettings.TrapDmgIntervalInTicks;
                    case FishyBuildings.Net:
                        return FishTrapsModSettings.NetDmgIntervalInTicks;
                    default:
                        return Props.dmgInterval;
                }
            }
        }

        private bool CanRebuild =>
            FishTrapsModSettings.autoReplaceAfterDestroyed &&
            parent.Faction == Faction.OfPlayer && parent.def.blueprintDef != null && parent.def.IsResearchFinished;

        private OverlayTypes MyOverlayType
        {
            get
            {

                if (parent.def.size.x > 1 || parent.def.size.z > 1)
                {
                    return OverlayTypes.ForbiddenBig;
                }
                return OverlayTypes.Forbidden;
            }
        }

        private void UpdateOverlayHandle()
        {
            if (parent.Spawned)
            {
                parent.Map.overlayDrawer.Disable(parent, ref overlayHandle);
                if (parent.Spawned && disabled)
                {
                    overlayHandle = parent.Map.overlayDrawer.Enable(parent, MyOverlayType);
                }
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            Spawner.PostSpawnSetup(parent, Props);
            UpdateOverlayHandle();
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            Command_Toggle forbidToggle = new Command_Toggle
            {
                defaultLabel = "WFFT_AllowSpawn".Translate(),
                defaultDesc = "WFFT_AllowSpawnDesc".Translate(),
                icon = TexCommand.ForbidOff,
                isActive = () => !disabled,
                toggleAction = delegate
                {
                    disabled = !disabled;
                    UpdateOverlayHandle();
                    if (disabled)
                    {
                        ticksPassed = 0;
                    }
                }
            };
            yield return forbidToggle;
            if (Prefs.DevMode)
            {
                Command_Action command_Action = new Command_Action
                {
                    defaultLabel = $"DEV: Spawn items ({Spawner.GetType().Name})",
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
                        DoDamage(Props.intervalDmgTreshhold);
                    }
                };
                yield return damageTrap;

            }
        }

        public override void CompTickRare()
        {
            if (disabled)
            {
                return;
            }
            ticksPassed += 250;
            if (ticksPassed >= SpawnInterval)
            {
                SpawnItems();
                ticksPassed -= SpawnInterval;
                DoDamage(Props.fishedDmgThreshhold);

            }
            else if (Rand.MTBEventOccurs(DmgInterval, 60000, 30000))
            {
                DoDamage(Props.intervalDmgTreshhold);
            }
        }

        private void DoDamage(float threshhold)
        {
            if (FishTrapsModSettings.dmgOvertime)
            {
                float trapDamage = Rand.Range(0f, threshhold);
                float dmgDone = Math.Max(1f, parent.HitPoints * trapDamage);
                parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, dmgDone));
            }
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            if (mode == DestroyMode.KillFinalize)
            {
                if (!this.CanRebuild || previousMap == null || !GenConstruct.CanPlaceBlueprintAt(parent.def, parent.Position, parent.Rotation, previousMap, false, null, null, parent.Stuff).Accepted)
                    return;
                GenConstruct.PlaceBlueprintForBuild(parent.def, parent.Position, previousMap, parent.Rotation, Faction.OfPlayer, parent.Stuff);
            }
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look<int>(ref ticksPassed, "ticksPassed", 0);
            Scribe_Values.Look<bool>(ref disabled, "trapDisabled", false);
        }

        private void SpawnItems()
        {
            Spawner.SpawnFish(parent);
        }

        public override string CompInspectStringExtra()
        {
            return disabled ?
                "WFFT_Disabled".Translate() :
                ("NextSpawnedResourceIn".Translate() + ": " + (SpawnInterval - ticksPassed).ToStringTicksToPeriod());

        }

        public override void Notify_SignalReceived(Signal signal)
        {
            if (signal.tag != "WFFT_TerrainChange")
            {
                return;
            }

            if (parent?.Map != null
                && signal.args.TryGetArg("grid", out TerrainGrid grid) && grid == parent.Map.terrainGrid && signal.args.TryGetArg("pos", out IntVec3 pos))
            {
                IEnumerable<IntVec3> cells = GenAdj.CellsOccupiedBy(parent.Position, parent.Rotation, parent.def.Size);
                if (cells.Any(cell => cell == pos && !parent.Map.terrainGrid.TerrainAt(pos).IsWater))
                {
                    Messages.Message("WFFT_Deconstructed".Translate(parent.def.label.CapitalizeFirst()), MessageTypeDefOf.SilentInput);
                    parent.Destroy(DestroyMode.Deconstruct);
                }
            }
        }
    }
}
