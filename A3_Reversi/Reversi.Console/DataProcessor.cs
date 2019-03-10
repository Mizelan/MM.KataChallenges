using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Reversi.Logics;
using Reversi.Logics.Definition;

namespace Reversi.Console
{
    class DataProcessor
    {
        public void ProcessFromDataFolder(string inputFolderPath, string outputFolderPath)
        {
            var files = Directory.GetFiles(inputFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var game = ReadGameData(file);
                var map = game.WriteSuggestionMovePosition();
                WriteMovementSuggestionData(game.CurrentTurnPlayer, map, file, outputFolderPath);
            }
        }

        Game ReadGameData(string filename)
        {
            var fileInfo = new FileInfo(filename);
            System.Console.WriteLine($"[{fileInfo.Name}]");
            var lines = File.ReadAllLines(filename);
            var size = lines[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var width = size[1];
            var height = size[0];
            var playerCollor = (PlayerColor)Enum.Parse(typeof(PlayerColor), lines[1]);
            var map = new Map(width, height);
            int y = 0;
            foreach (var line in lines.Skip(2))
            {
                System.Console.WriteLine(line);
                for (int x = 0; x < width; x++)
                {
                    if (line[x] == 'B')
                        map.MarkBlackCell(x, y);
                    if (line[x] == 'W')
                        map.MarkWhiteCell(x, y);
                }
                y++;
            }

            return new Game(playerCollor, map);
        }

        void WriteMovementSuggestionData(PlayerColor playerColor, Map map, string filename, string outputFolderPath)
        {
            var outputLines = new List<string>();
            outputLines.Add($"{map.Height} {map.Width}");
            outputLines.Add($"{playerColor.ToString()}");
            for (int y = 0; y < map.Height; y++)
            {
                var line = string.Empty;
                for (int x = 0; x < map.Width; x++)
                {
                    var cell = map[x, y];
                    if (cell.IsMovementSuggestion)
                        line += "0";
                    else if (cell.OccupiedColor == PlayerColor.Black)
                        line += "B";
                    else if (cell.OccupiedColor == PlayerColor.White)
                        line += "W";
                    else
                        line += ".";
                }
                outputLines.Add(line);
            }

            if (!Directory.Exists(outputFolderPath))
                Directory.CreateDirectory(outputFolderPath);

            File.WriteAllLines(Path.Combine(outputFolderPath, $"{new FileInfo(filename).Name}"), outputLines);
        }
    }
}
