using System;

namespace Querite
{
    [Serializable]
    public class LastQueryStats
    {
        public int? Count { get; set; }
        public DateTime? LastRun { get; set; }
    }
}