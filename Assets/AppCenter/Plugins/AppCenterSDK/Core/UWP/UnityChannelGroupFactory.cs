#if UNITY_WSA_10_0 && ENABLE_IL2CPP && !UNITY_EDITOR
using Microsoft.AppCenter.Channel;
using Microsoft.AppCenter.Ingestion.Http;

namespace Microsoft.AppCenter.Unity.Internal.Utils
{
    public class UnityChannelGroupFactory : IChannelGroupFactory
    {
        public IChannelGroup CreateChannelGroup(string appSecret)
        {
            //TODO use argument for networkstateadapter once sdk is updated
            return new ChannelGroup(appSecret, new UnityHttpNetworkAdapter(), new NetworkStateAdapter());
        }
    }
}
#endif