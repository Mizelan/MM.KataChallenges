using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGame.Logics
{
    public class Cell
    {
        public bool IsAlive = false;
        public readonly int X;
        public readonly int Y;

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            IsAlive = false;
        }
    }
}
