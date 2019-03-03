using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGame.Logics
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

        public int GetNeighborAliveCellCount(Cell cell)
        {
            int count = 0;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (!CheckValidCellPosition(cell.X + x, cell.Y + y))
                        continue;

                    var neighberCell = FindCell(cell.X + x, cell.Y + y);
                    if (cell == neighberCell)
                        continue;

                    if (!neighberCell.IsAlive)
                        continue;

                    count++;
                }
            }
            return count;
        }

        public void MarkAliveCell(int x, int y) => SetLiveStatus(x, y, true);
        public void MarkDeadCell(int x, int y) => SetLiveStatus(x, y, false);
        public Cell this[int x, int y] => FindCell(x, y);

        bool CheckValidCellPosition(int x, int y)
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

        void SetLiveStatus(int x, int y, bool isAlive)
        {
            var cell = FindCell(x, y);
            cell.IsAlive = isAlive;
        }
    }
}
