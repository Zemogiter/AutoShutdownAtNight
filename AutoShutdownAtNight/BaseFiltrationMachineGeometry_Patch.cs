using System.Reflection;
using HarmonyLib;

namespace AutoShutdownAtNight
{
    [HarmonyPatch(typeof(BaseFiltrationMachineGeometry), nameof(BaseFiltrationMachineGeometry.OnUpdate))]
    internal class BaseFiltrationMachineGeometry_Patch
    {
        [HarmonyPrefix]
        public static void OnUpdate()
        {
            var cycle = new DayNightCycle();
            bool dayTime = cycle.IsDay();
            FiltrationMachine filtrationMachine = new FiltrationMachine();
            FieldInfo WorkingField = typeof(FiltrationMachine).GetField("working", BindingFlags.Instance | BindingFlags.NonPublic);
            var machine = new BaseFiltrationMachineGeometry();
            bool isWaterFiltrationActive = machine.initialized;
            if (dayTime == false && isWaterFiltrationActive == true && Plugin.ConfigDisableWaterFiltration.Value == true)
            {
                filtrationMachine.CancelInvoke("UpdateFiltering");
                WorkingField.SetValue(filtrationMachine, false);
                filtrationMachine.storageContainer.container.onAddItem -= filtrationMachine.AddItem;
                filtrationMachine.storageContainer.container.onRemoveItem -= filtrationMachine.RemoveItem;
                //filtrationMachine.storageContainer.container.isAllowedToAdd = null;
                filtrationMachine.storageContainer.enabled = false;
                if (dayTime == true)
                {
                    filtrationMachine.Invoke("UpdateFiltering", 0f);
                    WorkingField.SetValue(filtrationMachine, true);
                    filtrationMachine.storageContainer.container.onAddItem += filtrationMachine.AddItem;
                    filtrationMachine.storageContainer.container.onRemoveItem += filtrationMachine.RemoveItem;
                    /*filtrationMachine.storageContainer.container.isAllowedToAdd = (){
                        return true;
                    };*/
                    filtrationMachine.storageContainer.enabled = true;
                }
            }
        }
    }
}
