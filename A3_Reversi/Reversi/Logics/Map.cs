using System.Diagnostics;
using Reversi.Logics.Definition;

namespace Reversi.Logics
{
    public class Map
    {
        readonly Cell[,] cells;
        readonly public int Width;
        readonly public int Height;

        public Map(int width, int height)
        {
            this.Width = width;
            this.Height = height;

            cells = new Cell[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    cells[x,y] = new Cell(x, y);
                }
            }
        }

        public void MarkBlackCell(int x, int y) => SetOccupiedColor(x, y, PlayerColor.Black);
        public void MarkWhiteCell(int x, int y) => SetOccupiedColor(x, y, PlayerColor.White);
        public Cell this[int x, int y] => FindCell(x, y);

        public bool CheckValidCellPosition(int x, int y)
        {
            if (x < 0 || y < 0)
                return false;

            if (x >= Width || y >= Height)
                return false;

            return true;
        }

        Cell FindCell(int x, int y)
        {
            Debug.Assert(CheckValidCellPosition(x, y));
            return cells[x, y];
        }

        void SetOccupiedColor(int x, int y, PlayerColor playerColor)
        {
            var cell = FindCell(x, y);
            cell.OccupiedColor = playerColor;
        }
    }
}
