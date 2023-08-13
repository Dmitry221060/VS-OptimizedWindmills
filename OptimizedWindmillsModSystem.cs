using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.GameContent.Mechanics;

namespace OptimizedWindmills {
    [HarmonyPatch]
    public class OptimizedWindmillsModSystem : ModSystem {
        Harmony harmony;

        public override void Start(ICoreAPI api) {
            harmony = new Harmony("optimizedwindmills");
            harmony.PatchAll();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(BEBehaviorWindmillRotor), "CheckWindSpeed")]
        public static bool CheckWindSpeed(ref double ___windSpeed, float dt) {
            ___windSpeed = 0.4;
            return false;
        }

        public override void Dispose() {
            harmony.UnpatchAll("optimizedwindmills");
        }
    }
}
