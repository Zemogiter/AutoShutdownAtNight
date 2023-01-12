using HarmonyLib;

namespace AutoShutdownAtNight
{
    [HarmonyPatch(typeof(MapRoomFunctionality), nameof(MapRoomFunctionality.Update))]
    internal class MapRoomFunctionality_Patch
    {
        [HarmonyPrefix]
        public static void Update(MapRoomFunctionality __instance)
        {
            var cycle = new DayNightCycle();
            bool dayTime = cycle.IsDay();
            var map = new MapRoomFunctionality();
            bool isScannerRoomActive = map.scanActive;
            TechType scanObject = map.GetActiveTechType();
            if (dayTime == false && isScannerRoomActive == true && Plugin.ConfigDisableScannerRoom.Value == true)
            {
                map.scanActive = false;
                map.StartScanning(TechType.None);
                if (dayTime == true)
                {
                    map.StartScanning(scanObject);
                }
            }
        }
    }
}
