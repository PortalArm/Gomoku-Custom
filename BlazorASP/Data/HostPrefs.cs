using Gomoku_Custom.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gomoku_Custom.Blazor.Data
{
    public class HostPrefs
    {
        public Team Team { get; set; }
        public Type Opponent { get; set; }
        public bool IsAuto { get; set; }
        public string Id { get; set; }
        public int FieldSize { get; set; }
        public int WinLength { get; set; }
    }
}
