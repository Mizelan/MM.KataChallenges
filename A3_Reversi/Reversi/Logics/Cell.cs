using Reversi.Logics.Definition;

namespace Reversi.Logics
{
    public class Cell
    {
        public PlayerColor OccupiedColor;
        public bool IsMovementSuggestion;
        public readonly int X;
        public readonly int Y;

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            OccupiedColor = PlayerColor.None;
            IsMovementSuggestion = false;
        }
    }
}
