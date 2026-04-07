using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using ColossalFramework;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace MileageTaxiServices
{
    [HarmonyPatch]
    [UsedImplicitly]
    public static class Patch_TaxiAi_UnloadPassengers
    {
        [UsedImplicitly]
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method("TaxiAI:UnloadPassengers");
        }

        [HarmonyTranspiler]
        [UsedImplicitly]
        public static IEnumerable<CodeInstruction> HandleTaxiPaysNoFareWhenArrive(IEnumerable<CodeInstruction> instructions)
        {
            /*
             * Replace the callvirt AddResource with a dummy method so it effectively produces no-op while consuming the leftover symbols on the stack
             * Patch with CodeMatcher
             */
            var signature = new List<Type>
            {
                typeof(EconomyManager.Resource),
                typeof(int),
                typeof(ItemClass)
            };
            return new CodeMatcher(instructions)
                .MatchStartForward(
                    new CodeMatch(OpCodes.Callvirt,
                        AccessTools.Method(typeof(EconomyManager), nameof(EconomyManager.AddResource),
                            signature.ToArray()))
                ) // find the only occurence of .AddResource()
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, typeof(Patch_TaxiAi_UnloadPassengers).GetMethod(nameof(HandleTaxiBaseMileage)))
                ) // insert replacement to call the dummy method; this clears the stack symbols for future calculation
                .Set(OpCodes.Nop, null) // and ignore the original instruction
                .InstructionEnumeration();
        }

        public static int HandleTaxiBaseMileage(this EconomyManager manager, EconomyManager.Resource resource, int amount, ItemClass itemClass, TaxiAI taxiInstance)
        {
            // we can "abuse" this method to allow for "taxi base mileage fare".
            var baseMileage = MileageTaxiServices.GetTaxiBaseMileage();
            var baseFare = Mathf.RoundToInt(taxiInstance.m_transportInfo.m_ticketPrice * baseMileage * MileageTaxiServices.VanillaTaxiFareRate);
            manager.AddResource(resource, baseFare, itemClass);
            return 0;
        }
    }
}