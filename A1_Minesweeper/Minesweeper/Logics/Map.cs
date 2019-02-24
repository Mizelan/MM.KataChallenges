using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Logics
{
    public class Map
    {
        readonly Cell[,] cells;
        readonly int width;
        readonly int height;
        List<Cell> bombCells;

        public Map(int width, int height)
        {
            this.width = width;
            this.height = height;

            cells = new Cell[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    cells[x,y] = new Cell(x, y);
                }
            }

            bombCells = new List<Cell>();
        }

        bool CheckCellPosition(int x, int y)
        {
            if (x < 0 || y < 0)
                return false;

            if (x >= width || y >= height)
                return false;

            return true;
        }

        Cell FindCell(int x, int y)
        {
            Debug.Assert(CheckCellPosition(x, y));
            return cells[x, y];
        }

        public void MarkBomb(int x, int y)
        {
            var cell = FindCell(x, y);
            Debug.Assert(!bombCells.Contains(cell));
            cell.IsBomb = true;
            bombCells.Add(cell);
        }

        public void FillCellNumbers()
        {
            foreach (var bomb in bombCells)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (!CheckCellPosition(bomb.X + x, bomb.Y + y))
                            continue;

                        var cell = FindCell(bomb.X + x, bomb.Y + y);
                        cell.Number++;
                    }
                }
            }
        }

        public int?[,] DumpNumbers()
        {
            var rv = new int?[width, height];
            for (var x=0; x<width; x++)
            {
                for (var y=0; y<height; y++)
                {
                    var cell = FindCell(x, y);
                    if (!cell.IsBomb)
                    {
                        rv[x, y] = cell.Number;
                    }
                }
            }
            return rv;
        }
    }
}
