using Gomoku_Custom.Shared;

namespace Gomoku_Custom.Game.Rules
{
    public class MockRule : IRule
    {
        public string Description => "Правило для тестирования игры";

        public bool CheckRule(int turn, Point pos) => true;
    }
}
