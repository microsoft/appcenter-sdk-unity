#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity.Distribute.Internal
{
    class DistributeDelegate : AndroidJavaProxy
    {
        private DistributeDelegate() : base("com.microsoft.azure.mobile.distribute.DistributeListener")
        {
        }

        public static void mobile_center_unity_distribute_set_delegate()
        {
            var distribute = new AndroidJavaClass("com.microsoft.azure.mobile.distribute.Distribute");
            distribute.CallStatic("setListener", new DistributeDelegate());
        }

        bool onReleaseAvailable(AndroidJavaObject activity, AndroidJavaObject details)
        {
            if (Distribute.ReleaseAvailable == null)
            {
                return false;
            }
            var releaseDetails = ReleaseDetailsHelper.ReleaseDetailsConvert(details);
			return Distribute.ReleaseAvailable.Invoke(releaseDetails);
		}
    }
}
#endif
