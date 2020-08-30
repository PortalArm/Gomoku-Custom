using Gomoku_Custom.Shared;

namespace Gomoku_Custom.Game.Rules
{
    public interface IRule
    {
        public string Description { get; }
        public bool CheckRule(int turn, Point pos);
    }
}
