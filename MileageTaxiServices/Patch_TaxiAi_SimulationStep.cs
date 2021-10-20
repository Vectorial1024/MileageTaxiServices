using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace MileageTaxiServices
{
    // somehow cannot patch anything
    [HarmonyPatch(typeof(TaxiAI))]
    [HarmonyPatch("SimulationStep", MethodType.Normal)]
    [HarmonyPatch(new Type[] { typeof(ushort), typeof(Vehicle), typeof(Vector3) })]
    public class Patch_TaxiAi_SimulationStep
    {
        public static bool Prepare()
        {
            // print to a certain place
            return true;
        }

        [HarmonyPostfix]
        public static void HandleTaxiGeneratePassiveIncome(ushort vehicleID, ref Vehicle data)
        {
            if (ReversePatch_TaxiAi_GetPassengerInstance.TaxiAi_GetPassengerInstance(null, vehicleID, ref data))
            {
                // has passenger
                // then sinple: generate some money for it
                Debug.Log($"Taxi vehicle ID: {vehicleID}; generating an income.");
                EconomyManager.instance.FetchResource(EconomyManager.Resource.FeePayment, 1, ItemClass.Service.PublicTransport, ItemClass.SubService.PublicTransportTaxi, ItemClass.Level.None);
            }
        }
    }
}
