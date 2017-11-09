#if UNITY_WSA_10_0 && ENABLE_IL2CPP && !UNITY_EDITOR
using Microsoft.AppCenter.Channel;

namespace Microsoft.AppCenter.Unity.Internal.Utils
{
    public class UnityChannelGroupFactory : IChannelGroupFactory
    {
        public IChannelGroup CreateChannelGroup(string appSecret)
        {
            return new ChannelGroup(appSecret, new UnityHttpNetworkAdapter());
        }
    }
}
#endif