using System;

namespace Microsoft.AppCenter.Unity.Analytics
{
    /// <summary>
    /// Persistence and latency flags for telemetry.
    /// </summary>
    [Flags]
    public enum Flags
    {
        /// <summary>
        /// An event can be lost due to low bandwidth or disk space constraints.
        /// </summary>
        PersistenceNormal = 0x01,

        /// <summary>
        /// Used for events that should be prioritized over non-critical events.
        /// </summary>
        PersistenceCritical = 0x02
    }
}
