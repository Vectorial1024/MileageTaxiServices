using HarmonyLib;
using System;

namespace MileageTaxiServices
{
    [HarmonyPatch(typeof(TaxiAI))]
    [HarmonyPatch("GetPassengerInstance", MethodType.Normal)]
    public class ReversePatch_TaxiAi_GetPassengerInstance
    {
        [HarmonyReversePatch]
        public static bool TaxiAi_GetPassengerInstance(object __instance, ushort vehicleID, ref Vehicle data)
        {
            throw new NotImplementedException("This is a stub that is not available at this moment.");
        }
    }
}
