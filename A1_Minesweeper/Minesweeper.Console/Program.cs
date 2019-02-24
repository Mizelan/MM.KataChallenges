using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minesweeper.Console;

namespace Minesweeper.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataLoader = new DataProcessor();
            dataLoader.ProcessFromDataFolder(@".\data", @".\output");
        }
    }
}
