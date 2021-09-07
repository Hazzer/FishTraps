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
        public static int trapSpawnInterval = 90000;
        public static int trapDmgInterval = 30000;

        public static int netSpawnInterval = 120000;
        public static int netDmgInterval = 30000;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref trapSpawnInterval, "WFFT_trapSpawnInterval", 90000, true);
            Scribe_Values.Look<int>(ref trapDmgInterval, "WFFT_trapDmgInterval", 30000, true);

            Scribe_Values.Look<int>(ref netSpawnInterval, "WFFT_netSpawnInterval", 120000, true);
            Scribe_Values.Look<int>(ref netDmgInterval, "WFFT_netDmgInterval", 30000, true);
        }
    }
}
