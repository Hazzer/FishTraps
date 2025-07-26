using Verse;

namespace FishTraps.Spawner
{
  public interface SpawnerAdapter
  {
    void PostSpawnSetup(Thing parent, FishyCompProperties fishyCompProps);
    void SpawnFish(Thing parent);
  }
}