using LudeonTK;
using Verse;

namespace Odyssey
{
  public class DebugActions
  {
    [DebugAction("Fish Traps", "Reinitialize WaterBody",  actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
    public static void ReinitWaterType()
    {
      var cell =UI.MouseCell();
      var map = Find.CurrentMap;
      if (map.waterBodyTracker.TryGetWaterBodyAt(cell, out WaterBody waterBody))
      {
        waterBody.Initialize();
      }
      
    }
  }
}