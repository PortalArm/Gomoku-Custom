using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Gomoku_Custom.Shared
{
    [Serializable]
    public class GameData
    {
        public Team[][] Field { get; set; }
        public Point Updated { get; set; }
        public ResponseCode Code { get; set; }
        public Team NextPlayer { get; set; }
    }

    
}
