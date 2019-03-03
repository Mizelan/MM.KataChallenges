using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeGame.Logics;

namespace LifeGame.Console
{
    class DataProcessor
    {
        public void ProcessFromDataFolder(string inputFolderPath, string outputFolderPath)
        {
            var files = Directory.GetFiles(inputFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var generation = ReadGenerationData(file);
                generation.GoNextGeneration();
                WriteGenerationData(generation, file, outputFolderPath);
            }
        }

        Generation ReadGenerationData(string filename)
        {
            var fileInfo = new FileInfo(filename);
            System.Console.WriteLine($"[{fileInfo.Name}]");
            var lines = File.ReadAllLines(filename);
            if (!int.TryParse(lines[0].Replace("Generation", string.Empty).Trim(':', ' '), out var generationSeqId))
            {
                throw new Exception("세대 정보 파싱 실패 : " + lines[0]);
            }
            var size = lines[1].Split(' ').Select(x => int.Parse(x)).ToArray();
            var width = size[1];
            var height = size[0];
            var map = new Map(width, height);
            int y = 0;
            foreach (var line in lines.Skip(2))
            {
                System.Console.WriteLine(line);
                for (int x = 0; x < width; x++)
                {
                    if (line[x] == '*')
                        map.MarkAliveCell(x, y);
                }
                y++;
            }

            return new Generation(generationSeqId, map);
        }

        void WriteGenerationData(Generation generation, string filename, string outputFolderPath)
        {
            var map = generation.Map;
            var outputLines = new List<string>();
            outputLines.Add($"Generation {generation.SequenceId}:");
            outputLines.Add($"{map.Height} {map.Width}");
            for (int y = 0; y < map.Height; y++)
            {
                var line = string.Empty;
                for (int x = 0; x < map.Width; x++)
                {
                    var cell = map[x, y];
                    if (cell.IsAlive)
                        line += "*";
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
