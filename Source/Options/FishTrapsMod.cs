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

        public FishTrapsMod(ModContentPack content) : base(content)
        {
            this.GetSettings<FishTrapsModSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Gap(12f);

            listingStandard.Label("WFFT_FishTrap".Translate());
            listingStandard.Gap(12f);
            listingStandard.Label(SPAWN_INTERVAL.Translate() + FishTrapsModSettings.trapSpawnInterval, tooltip: SPAWN_TOOLTIP.Translate());
            FishTrapsModSettings.trapSpawnInterval = (int)listingStandard.Slider(FishTrapsModSettings.trapSpawnInterval, 10000f, 60000 * 5f);
            listingStandard.Label(DMG_INTERVAL.Translate() + FishTrapsModSettings.trapDmgInterval, tooltip: DMG_TOOLTIP.Translate() );
            FishTrapsModSettings.trapDmgInterval = (int)listingStandard.Slider(FishTrapsModSettings.trapDmgInterval, 1000f, 60000f);

            if (listingStandard.ButtonText(DEFAULT.Translate()))
            {
                FishTrapsModSettings.trapSpawnInterval = 90000;
                FishTrapsModSettings.trapDmgInterval = 30000;
            };
            listingStandard.Gap(12f);

            listingStandard.Label("WFFT_FishNet".Translate());
            listingStandard.Gap(12f);
            listingStandard.Label(SPAWN_INTERVAL.Translate() + FishTrapsModSettings.netSpawnInterval, tooltip: SPAWN_TOOLTIP.Translate());
            FishTrapsModSettings.netSpawnInterval = (int)listingStandard.Slider(FishTrapsModSettings.netSpawnInterval, 10000f, 60000 * 5f);
            listingStandard.Label(DMG_INTERVAL.Translate() + FishTrapsModSettings.netDmgInterval, tooltip: DMG_TOOLTIP.Translate());
            FishTrapsModSettings.netDmgInterval = (int)listingStandard.Slider(FishTrapsModSettings.netDmgInterval, 1000f, 60000f);

            if (listingStandard.ButtonText(DEFAULT.Translate()))
            {
                FishTrapsModSettings.netSpawnInterval = 120000;
                FishTrapsModSettings.netDmgInterval = 30000;
            };
            


            listingStandard.End();
        }

        public override string SettingsCategory()
        {
            return "WFFT_ModName".Translate();
        }
    }
}
