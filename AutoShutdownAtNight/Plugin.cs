using BepInEx;
using HarmonyLib;
using System;
using System.Reflection;
using BepInEx.Configuration;

namespace AutoShutdownAtNight
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        #region[Declarations]
        private const string
            MODNAME = "AutoShutdownAtNight",
            AUTHOR = "Zemogiter",
            GUID = "zemogiter.subnautica.AutoShutdownAtNight",
            VERSION = "1.0.0.0";
        #endregion

        public static ConfigEntry<bool> ConfigDisableScannerRoom;
        public static ConfigEntry<bool> ConfigDisableWaterFiltration;

        public void Awake()
        {
            Console.WriteLine("AutoShutdownAtNight - Started patching v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(3));
            LoadConfig();
            var harmony = new Harmony(GUID);
            Patch_Map(harmony);
            Patch_FiltrationMachine(harmony);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Console.WriteLine("AutoShutdownAtNight - Finished patching");
        }
        private void LoadConfig()
        {
            ConfigDisableScannerRoom = base.Config.Bind(new ConfigDefinition("General", "Disable Scanner Room?"), defaultValue: false, new ConfigDescription("Set to Enabled to make scanner room turn off the scanning at night.", null));
            ConfigDisableWaterFiltration = base.Config.Bind(new ConfigDefinition("General", "Disable Water Filtration?"), defaultValue: false, new ConfigDescription("Set to Enabled to turn off the water filtration machine at night.", null));
        }
        private static void Patch_Map(Harmony harmony)
        {
            harmony.Patch(AccessTools.Method(typeof(MapRoomFunctionality), "Update"), new HarmonyMethod(typeof(MapRoomFunctionality_Patch).GetMethod("Update")));
        }
        private static void Patch_FiltrationMachine(Harmony harmony)
        {
            harmony.Patch(typeof(BaseFiltrationMachineGeometry).GetMethod("OnUpdate"), new HarmonyMethod(typeof(MapRoomFunctionality_Patch).GetMethod("OnUpdate")));
        }
    }
}
