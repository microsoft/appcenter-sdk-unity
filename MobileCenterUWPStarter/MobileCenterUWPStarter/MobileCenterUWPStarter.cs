using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileCenterUWPStarter
{
    public sealed class MobileCenterUWPStarter
    {
        private static readonly object MobileCenterUWPStarterLock = new object();
        private static string _launchArgs = null;

        public static void SetLaunchArgs(string args)
        {
            lock (MobileCenterUWPStarterLock)
            {
                _launchArgs = args;
            }
        }
        public static string GetLaunchArgs()
        {
            lock (MobileCenterUWPStarterLock)
            {
                var launchArgsCopy = _launchArgs;
                _launchArgs = null;
                return launchArgsCopy;
            }
        }
    }
}
