using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku_Custom.Shared
{
    public enum ResponseCode : byte { OK, PointOccupied, PointOutOfRange, RequestError, Win, Loss, AccessDenied, Reserved }
    public enum Team { Blue, Red, Unknown }
}
