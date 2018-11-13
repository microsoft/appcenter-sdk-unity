using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.AppCenter.Unity.Analytics
{
    public class Flags
    {
        // An event can be lost due to low bandwidth or disk space constraints.     
        public const int PERSISTENCE_NORMAL = 0x01;

        // Used for events that should be prioritized over non-critical events.        
        public const int PERSISTENCE_CRITICAL = 0x02;
    }
}
