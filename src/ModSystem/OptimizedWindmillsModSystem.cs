using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.GameContent.Mechanics;
using System.Reflection;

namespace OptimizedWindmills {
    public class OptimizedWindmillsModSystem : ModSystem {
        const string modID = "optimizedwindmills";
        Harmony harmony = new Harmony(modID);
        NetworkSystem networkSystem = new NetworkSystem();

        public override void StartServerSide(ICoreServerAPI api) {
            LoadConfig(api);
            networkSystem.StartServerSide(api);
        }

        public override void StartClientSide(ICoreClientAPI api) {
            networkSystem.StartClientSide(api);
        }

        public override void Start(ICoreAPI api) {
            var OriginalCheckWindSpeed = typeof(BEBehaviorWindmillRotor).GetMethod(nameof(WindmillRotorPatch.CheckWindSpeed), BindingFlags.NonPublic | BindingFlags.Instance);
            var PatchedCheckWindSpeed = typeof(WindmillRotorPatch).GetMethod(nameof(WindmillRotorPatch.CheckWindSpeed));
            harmony.Patch(OriginalCheckWindSpeed, prefix: new HarmonyMethod(PatchedCheckWindSpeed));
        }

        public override void Dispose() {
            harmony.UnpatchAll(harmony.Id);
            ModConfig.Loaded = new ModConfig();
        }

        private void LoadConfig(ICoreServerAPI api) {
            try {
                ModConfig config = api.LoadModConfig<ModConfig>($"{modID}.json");
                ModConfig.Loaded = config ?? ModConfig.Loaded;
                if (config == null) api.StoreModConfig(ModConfig.Loaded, $"{modID}.json");
            } catch {
                api.StoreModConfig(ModConfig.Loaded, $"{modID}.json");
            }
        }
    }
}
