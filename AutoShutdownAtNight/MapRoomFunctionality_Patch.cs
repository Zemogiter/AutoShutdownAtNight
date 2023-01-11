using HarmonyLib;

namespace AutoShutdownAtNight
{
    internal class MapRoomFunctionality_Patch
    {
        [HarmonyPrefix]
        public static void Update()
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
