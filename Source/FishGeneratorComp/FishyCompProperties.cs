using Verse;

namespace FishTraps
{
    public class FishyCompProperties : CompProperties
    {

        public int spawnInterval = 30000;
        public FishyBuildings buildingType;
        public int dmgInterval = 10;
        public float intervalDmgTreshhold = 0.5f;
        public float fishedDmgThreshhold = 0.2f;

        public FishyCompProperties()
        {
            compClass = typeof(FishyItemSpawnerComp);
        }

    }
}
