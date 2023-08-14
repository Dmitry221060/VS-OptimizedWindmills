using Newtonsoft.Json;
using ProtoBuf;
using Vintagestory.API.Client;
using Vintagestory.API.Server;

namespace OptimizedWindmills {
    public class NetworkSystem {
        [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
        class SyncConfig {
            public string config;
        }

        #region Client
        public void StartClientSide(ICoreClientAPI api) {
            api.Network.RegisterChannel("optimizedwindmills")
            .RegisterMessageType(typeof(SyncConfig))
            .SetMessageHandler<SyncConfig>(OnServerMessage);
        }

        private void OnServerMessage(SyncConfig networkMessage) {
            ModConfig.Loaded = JsonConvert.DeserializeObject<ModConfig>(networkMessage.config);
        }
        #endregion

        #region Server
        IServerNetworkChannel serverChannel;

        public void StartServerSide(ICoreServerAPI api) {
            serverChannel =
                api.Network.RegisterChannel("optimizedwindmills")
                .RegisterMessageType(typeof(SyncConfig))
            ;

            api.Event.PlayerNowPlaying += (player) => {
                SendConfig(new IServerPlayer[] { player });
            };
        }

        private void SendConfig(IServerPlayer[] players) {
            var config = JsonConvert.SerializeObject(ModConfig.Loaded);
            var message = new SyncConfig() { config = config };

            serverChannel.SendPacket(message, players);
        }
        #endregion
    }
}
