using HarmonyLib;
using System.Reflection;

namespace MileageTaxiServices
{
    internal class PatchController
    {
        public static string HarmonyModID => "com.vectorial1024.cities.mts";

        /*
         * The "singleton" design is pretty straight-forward.
         */

        private static Harmony harmony;

        public static Harmony GetHarmonyInstance()
        {
            return harmony ?? (harmony = new Harmony(HarmonyModID));
        }

        public static void Activate()
        {
            GetHarmonyInstance().PatchAll(Assembly.GetExecutingAssembly());
        }

        public static void Deactivate()
        {
            GetHarmonyInstance().UnpatchAll(HarmonyModID);
        }
    }
}
