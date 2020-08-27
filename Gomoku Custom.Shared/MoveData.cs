using System;

namespace Gomoku_Custom.Shared
{
    [Serializable]
    public struct MoveData
    {
        public Point Proposed { get; set; }
        public Guid ClientId { get; set; }
    }
}
