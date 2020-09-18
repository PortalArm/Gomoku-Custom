using Gomoku_Custom.Game.FieldControllers;
using Gomoku_Custom.Shared;
using System.Collections.Generic;
using System.Text;

namespace Gomoku_Custom.Game.Formatters
{
    public class ConsoleGomokuFormatter
    {
        public static readonly Dictionary<Team, char> DefaultChars = new Dictionary<Team, char>()
            { {Team.Red, 'o' }, { Team.Blue, 'x' }, { Team.None, '.' } };
        //private readonly GomokuGame _game;
        private StringBuilder _sb;
        private readonly Dictionary<Team, char> _teamChars;
        public ConsoleGomokuFormatter(Dictionary<Team, char> correspondances = null)
        {
            _teamChars = correspondances ?? DefaultChars;
        }
        private void Init(int fieldSize)
        {
            int side = fieldSize + 2;
            _sb?.Clear();
            _sb ??= new StringBuilder(side * (side + 1));
            for (int i = 0; i < side; ++i)
            {
                _sb.Append(' ', side);
                _sb.Append('\n');
            }
        }
        
        // Should be removed, used only in debugging
        public string RenderAsString(IFieldController fieldController) {
            int side = fieldController.FieldSize + 2;
            if (_sb == null || _sb.Length < side * (side + 1))
                Init(side - 2);

            for (int i = 0; i < side - 2; ++i)
                for (int j = 0; j < side - 2; ++j)
                    _sb[(i + 1) * (side + 1) + j + 1] = _teamChars[fieldController.GetPos(j,i)];
            return _sb.ToString();
        }
        public char GetSymbolOf(Team team) => _teamChars[team];

        public string RenderAsString(GameData gd)
        {
            int side = gd.Field.GetLength(0) + 2;
            if (_sb == null || _sb.Length < side * (side + 1))
                Init(side - 2);

            for (int i = 0; i < side - 2; ++i)
                for (int j = 0; j < side - 2; ++j)
                    _sb[(i + 1) * (side + 1) + j + 1] = _teamChars[gd.Field[i][j]];
            return _sb.ToString();
        }
        //public void Print()
        //{
        //    int side = _game.Field.GetLength(0) + 2;

        //    for (int i = 0; i < side - 2; ++i)
        //        for (int j = 0; j < side - 2; ++j)
        //            _sb[(i + 1) * (side + 1) + j + 1] = _teamChars(_game.Field[i, j]);
        //    Console.WriteLine(_sb.ToString());
        //}
    }
}
