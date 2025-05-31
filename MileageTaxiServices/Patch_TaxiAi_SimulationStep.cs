using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ColossalFramework;
using UnityEngine;

namespace MileageTaxiServices
{
    // somehow cannot patch anything
    [HarmonyPatch]
    public class Patch_TaxiAi_SimulationStep
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method("TaxiAI:SimulationStep", new Type[]
            {
                typeof(ushort),
                typeof(Vehicle).MakeByRefType(),
                typeof(Vehicle.Frame).MakeByRefType(),
                typeof(ushort),
                typeof(Vehicle).MakeByRefType(),
                typeof(int)
            });
        }

        public static bool Prepare()
        {
            // print to a certain place
            return true;
        }

        [HarmonyPostfix]
        public static void HandleTaxiGeneratePassiveIncome(TaxiAI __instance, ushort vehicleID, ref Vehicle vehicleData)
        {
            if (ReversePatch_TaxiAi_GetPassengerInstance.TaxiAi_GetPassengerInstance(null, vehicleID, ref vehicleData) != 0)
            {
                // has passenger
                // then sinple: generate some money for it
                Debug.Log($"Taxi vehicle ID: {vehicleID}; generating an income.");
                Singleton<EconomyManager>.instance.AddResource(EconomyManager.Resource.PublicIncome, 25, __instance.m_info.m_class);
                EconomyManager.instance.FetchResource(EconomyManager.Resource.FeePayment, 1, ItemClass.Service.PublicTransport, ItemClass.SubService.PublicTransportTaxi, ItemClass.Level.None);
            }
        }
    }
}
