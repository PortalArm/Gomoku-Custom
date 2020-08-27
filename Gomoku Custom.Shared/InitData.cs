using System;

namespace Gomoku_Custom.Shared
{
    [Serializable]
    public struct InitData
    {
        public Guid ClientId { get; set; }
        public Team Side { get; set; }
        public ResponseCode Code { get; set; }
    }
}
