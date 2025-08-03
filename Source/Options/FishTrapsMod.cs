using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace FishTraps.Options
{
  public class FishTrapsMod : Mod
  {
    private const string SPAWN_INTERVAL = "WFFT_SpawnInterval";
    private const string SPAWN_TOOLTIP = "WFFT_SpawnIntervalToolTip";
    private const string DMG_INTERVAL = "WFFT_DmgInterval";
    private const string DMG_TOOLTIP = "WFFT_DmgIntervalToolTip";
    private const string DEFAULT = "WFFT_Default";
    private const string BUILD_RADIUS = "WFFT_BuildRadius";
    private const string FISH_TRAP = "WFFT_FishTrap";
    private const string AUTO_REBUILD = "WFFT_AutoRebuild";
    private const string AUTO_REBUILD_TOOLTIP = "WFFT_AutoRebuildDesc";
    private const string FISH_NET = "WFFT_FishNet";
    private const string MOD_NAME = "WFFT_ModName";
    private const string AUTO_DMG = "WFFT_DisableTrapDamageOverTime";
    private const string AUTO_DMG_TOOLTIP = "WFFT_DisableTrapDamageOverTimeDesc";
    private const string NOTIFY_WATER_BODIES = "WFFT_NotifyWaterBodies";
    private const string NOTIFY_WATER_BODIES_DESC = "WFFT_NotifyWaterBodiesDesc";
    private const string WfftFishnetmultiply = "WFFT_FishNetMultiply";
    private const string WfftFishnetmultiplydesc = "WFFT_FishNetMultiplyDesc";
    private const string WfftFishtrapmultiply = "WFFT_FishTrapMultiply";
    private const string WfftFishtrapmultiplydesc = "WFFT_FishTrapMultiplyDesc";

    public FishTrapsMod(ModContentPack content) : base(content)
    {
      this.GetSettings<FishTrapsModSettings>();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
      Listing_Standard listingStandard = new Listing_Standard();


      listingStandard.Begin(inRect);
      listingStandard.Gap(12f);

      listingStandard.CheckboxLabeled(AUTO_DMG.Translate(), ref FishTrapsModSettings.dmgOvertime,
        AUTO_DMG_TOOLTIP.Translate());
      listingStandard.Gap(12f);

      if (ModsConfig.OdysseyActive)
      {
        listingStandard.CheckboxLabeled(NOTIFY_WATER_BODIES.Translate(),
          ref FishTrapsModSettings.odysseyNotifyWaterBodies, NOTIFY_WATER_BODIES_DESC.Translate());
        listingStandard.Gap(12f);
      }

      listingStandard.Label(FISH_TRAP.Translate());
      listingStandard.Gap(12f);
      listingStandard.Label(
        SPAWN_INTERVAL.Translate(FishTrapsModSettings.trapSpawnInterval, FishTrapsModSettings.TrapSpawnIntervalInTicks),
        tooltip: SPAWN_TOOLTIP.Translate());
      FishTrapsModSettings.trapSpawnInterval =
        (int)listingStandard.Slider(FishTrapsModSettings.trapSpawnInterval, 1, 24 * 5);
      listingStandard.Label(
        DMG_INTERVAL.Translate(FishTrapsModSettings.trapDmgInterval, FishTrapsModSettings.TrapDmgIntervalInTicks),
        tooltip: DMG_TOOLTIP.Translate());
      FishTrapsModSettings.trapDmgInterval = (int)listingStandard.Slider(FishTrapsModSettings.trapDmgInterval, 1, 24);
      if (ModsConfig.OdysseyActive)
      {
        listingStandard.Label(WfftFishtrapmultiply.Translate(FishTrapsModSettings.OdysseyTrapCatchMultiplier.ToString("F1")),
          tooltip: WfftFishtrapmultiplydesc.Translate());
        FishTrapsModSettings.odysseyTrapCatchMultuplyValue =
          (int)listingStandard.Slider(FishTrapsModSettings.odysseyTrapCatchMultuplyValue, 1, 100);
      }

      if (listingStandard.ButtonText(DEFAULT.Translate()))
      {
        FishTrapsModSettings.trapSpawnInterval = 36;
        FishTrapsModSettings.trapDmgInterval = 12;
        FishTrapsModSettings.odysseyTrapCatchMultuplyValue = 6;
      }

      listingStandard.Gap(12f);

      listingStandard.Label(FISH_NET.Translate());
      listingStandard.Gap(12f);
      listingStandard.Label(
        SPAWN_INTERVAL.Translate(FishTrapsModSettings.netSpawnInterval, FishTrapsModSettings.NetSpawnIntervalInTicks),
        tooltip: SPAWN_TOOLTIP.Translate());
      FishTrapsModSettings.netSpawnInterval =
        (int)listingStandard.Slider(FishTrapsModSettings.netSpawnInterval, 1f, 24 * 5);
      listingStandard.Label(
        DMG_INTERVAL.Translate(FishTrapsModSettings.netDmgInterval, FishTrapsModSettings.NetDmgIntervalInTicks),
        tooltip: DMG_TOOLTIP.Translate());
      FishTrapsModSettings.netDmgInterval = (int)listingStandard.Slider(FishTrapsModSettings.netDmgInterval, 1, 24);
      if (ModsConfig.OdysseyActive)
      {
        listingStandard.Label(WfftFishnetmultiply.Translate( FishTrapsModSettings.OdysseyNetCatchMultiplier.ToString("F1")),
          tooltip: WfftFishnetmultiplydesc.Translate());
        FishTrapsModSettings.odysseyNetCatchMultuplyValue =
          (int)listingStandard.Slider(FishTrapsModSettings.odysseyNetCatchMultuplyValue, 1, 100);
      }

      if (listingStandard.ButtonText(DEFAULT.Translate()))
      {
        FishTrapsModSettings.netSpawnInterval = 48;
        FishTrapsModSettings.netDmgInterval = 12;
        FishTrapsModSettings.odysseyNetCatchMultuplyValue = 10;
      }

      listingStandard.Label(BUILD_RADIUS.Translate(FishTrapsModSettings.buildRadius));
      FishTrapsModSettings.buildRadius = (int)listingStandard.Slider(FishTrapsModSettings.buildRadius, 0, 30);
      if (listingStandard.ButtonText(DEFAULT.Translate()))
      {
        FishTrapsModSettings.buildRadius = 10;
      }

      listingStandard.CheckboxLabeled(AUTO_REBUILD.Translate(), ref FishTrapsModSettings.autoReplaceAfterDestroyed,
        AUTO_REBUILD_TOOLTIP.Translate());

      listingStandard.End();
    }

    public override string SettingsCategory()
    {
      return MOD_NAME.Translate();
    }
  }
}