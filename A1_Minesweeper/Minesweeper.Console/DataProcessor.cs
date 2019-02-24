using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minesweeper.Logics;

namespace Minesweeper.Console
{
    class DataProcessor
    {
        public void ProcessFromDataFolder(string inputFolderPath, string outputFolderPath)
        {
            var files = Directory.GetFiles(inputFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                System.Console.WriteLine($"[{fileInfo.Name}]");
                var lines = File.ReadAllLines(file);
                var size = lines.First().Split(' ').Select(x => int.Parse(x)).ToArray();
                var width = size[1];
                var height = size[0];
                var map = new Map(width, height);
                int y = 0;
                foreach (var line in lines.Skip(1))
                {
                    System.Console.WriteLine(line);
                    for (int x=0; x<width; x++)
                    {
                        if (line[x] == '*')
                            map.MarkBomb(x, y);
                    }
                    y++;
                }
                map.FillCellNumbers();
                var numbers = map.DumpNumbers();

                var outputLines = new List<string>();
                for (int yy = 0; yy < height; yy++)
                {
                    var line = string.Empty;
                    for (int xx = 0; xx < width; xx++)
                    {
                        var n = numbers[xx, yy];
                        if (!n.HasValue)
                            line += "*";
                        else
                            line += n.Value.ToString();
                    }
                    outputLines.Add(line);
                }

                if (!Directory.Exists(outputFolderPath))
                    Directory.CreateDirectory(outputFolderPath);

                File.WriteAllLines(Path.Combine(outputFolderPath, $"{fileInfo.Name}"), outputLines);
            }
        }
    }
}
