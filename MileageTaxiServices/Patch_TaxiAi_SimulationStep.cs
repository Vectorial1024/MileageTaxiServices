using HarmonyLib;
using System;
using System.Reflection;
using ColossalFramework;
using JetBrains.Annotations;
using UnityEngine;

namespace MileageTaxiServices
{
    // somehow cannot patch anything
    [HarmonyPatch]
    [UsedImplicitly]
    public class Patch_TaxiAi_SimulationStep
    {
        private const double TaxiFareRate = 1.0 / 2000.0;

        [UsedImplicitly]
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

        [HarmonyPostfix]
        [UsedImplicitly]
        public static void HandleTaxiGenerateMileageIncome(TaxiAI __instance, ushort vehicleID, ref Vehicle vehicleData)
        {
            if (ReversePatch_TaxiAi_GetPassengerInstance.TaxiAi_GetPassengerInstance(null, vehicleID, ref vehicleData) == 0)
            {
                // no passenger; skip
                return;
            }
            // has passenger
            // then simple: generate some money for it
            Debug.Log($"Taxi vehicle ID: {vehicleID}; generating an income.");

            // calculate the instantaneous fare based on the distance travelled
            // if distance travelled is not enough, causing the fare to round to 0, then consider as idle fare
            // this allows maximum compatibility with other fare-scaling mods
            var instantFare = (int) Math.Round(__instance.m_transportInfo.m_ticketPrice * DetermineDelta(ref vehicleData) * TaxiFareRate);
            Singleton<EconomyManager>.instance.AddResource(EconomyManager.Resource.PublicIncome, instantFare < 0 ? 1 : instantFare, __instance.m_info.m_class);
        }

        private static double DetermineDelta(ref Vehicle vehicleData)
        {
            /*
             * important note:
             * taxi must have the last 4 frames available (i.e. data for past real-life 1 second)
             * because it must have driven for some distance before picking up passengers
             */

            var lastFrameRadix = vehicleData.m_lastFrame;
            var secondLastFrameRadix = lastFrameRadix == 0 ? 3 : lastFrameRadix - 1;
            var lastFramePosition = vehicleData.GetLastFramePosition();
            Vehicle.Frame secondLastFrame;
            switch (secondLastFrameRadix)
            {
                case 0:
                    secondLastFrame = vehicleData.m_frame0;
                    break;
                case 1:
                    secondLastFrame = vehicleData.m_frame1;
                    break;
                case 2:
                    secondLastFrame = vehicleData.m_frame2;
                    break;
                case 3:
                    secondLastFrame = vehicleData.m_frame3;
                    break;
                default:
                    secondLastFrame = vehicleData.m_frame0;
                    break;
            }
            var secondLastFramePosition = secondLastFrame.m_position;

            return Vector3.Distance(secondLastFramePosition, lastFramePosition);
        }
    }
}
