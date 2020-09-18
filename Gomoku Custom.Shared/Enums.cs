using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku_Custom.Shared
{
    public enum ResponseCode : byte { OK, 
        PointOccupied, 
        PointOutOfRange, 
        RequestError, 
        Win, 
        Loss, 
        AccessDenied, WrongTurn, Reserved, RuleViolation, ServerClosed,
        Draw
    }
    public enum Team { None, Blue, Red, Unknown }
    public enum GameState { 
        AwaitingConnections, 
        GameStarted, 
        GameEnded, 
        WaitingForBlue, 
        WaitingForRed,
        AwaitingStart
    }
}
