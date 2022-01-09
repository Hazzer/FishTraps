using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FishTraps.Options
{
    public class FishTrapsModSettings : ModSettings
    {
        public static readonly int ONE_HOUR = 2500;

        public static int trapSpawnInterval = 36;
        public static int TrapSpawnIntervalInTicks
        {
            get
            {
                return trapSpawnInterval * ONE_HOUR;
            }
        }

        public static int trapDmgInterval = 12;
        public static int TrapDmgIntervalInTicks
        {
            get
            {
                return trapDmgInterval * ONE_HOUR;
            }
        }

        public static int netSpawnInterval = 48;
        public static int NetSpawnIntervalInTicks
        {
            get
            {
                return netSpawnInterval * ONE_HOUR;
            }
        }

        public static int netDmgInterval = 12;
        public static int NetDmgIntervalInTicks
        {
            get
            {
                return netDmgInterval * ONE_HOUR;
            }
        }
        public static int buildRadius = 10;

        public static bool autoReplaceAfterDestroyed = false;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref trapSpawnInterval, "WFFT_trapSpawnInterval", 36, true);
            Scribe_Values.Look<int>(ref trapDmgInterval, "WFFT_trapDmgInterval", 12, true);

            Scribe_Values.Look<int>(ref netSpawnInterval, "WFFT_netSpawnInterval", 48, true);
            Scribe_Values.Look<int>(ref netDmgInterval, "WFFT_netDmgInterval", 12, true);

            Scribe_Values.Look<int>(ref buildRadius, "WFFT_buildRadius", 10, true);

            Scribe_Values.Look<bool>(ref autoReplaceAfterDestroyed, "WFFT_AutoDestroyReplace", true, true);

            if(Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                if (trapSpawnInterval > 5 * 24)
                {
                    trapSpawnInterval = 36;
                    Log.Warning("Trap spawn interval adjused - please check fish trap mod options");
                }

                if (trapDmgInterval > 24)
                {
                    trapDmgInterval = 12;
                    Log.Warning("Trap dmg interval adjused - please check fish trap mod options");
                }

                if (netSpawnInterval > 5 * 24)
                {
                    netSpawnInterval = 48;
                    Log.Warning("Net spawn interval adjused - please check fish trap mod options");
                }

                if (netDmgInterval > 24)
                {
                    netDmgInterval = 12;
                    Log.Warning("Net dmg interval adjused - please check fish trap mod options");
                }
            }
        }
    }
}
