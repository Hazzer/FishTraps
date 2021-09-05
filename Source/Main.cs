using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FishTraps
{
    [StaticConstructorOnStartup]
    class Main
    {
        static Main()
        {
            try
            {
                BiomeRepo.Init();

            } catch (Exception e)
            {
                Log.Error($"Exception during init: {e}");
            }
        }
    }
}
