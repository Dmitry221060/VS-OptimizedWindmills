using HarmonyLib;

namespace OptimizedWindmills {
    [HarmonyPatch]
    class WindmillRotorPatch {
        [HarmonyPrefix]
        public static bool CheckWindSpeed(ref double ___windSpeed) {
            ___windSpeed = ModConfig.Loaded.windmillEfficiency;
            return false;
        }
    }
}
