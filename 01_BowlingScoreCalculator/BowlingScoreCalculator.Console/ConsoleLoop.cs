using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowlingScoreCalculator.Logics;

namespace BowlingScoreCalculator
{
    class ConsoleLoop
    {
        ScoreCalculator scoreCalculator;

        public void Run()
        {
            Console.WriteLine("# 볼링 점수 계산기 (재시작: 'restart', 종료: 'quit')");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();

            scoreCalculator = new ScoreCalculator();
            scoreCalculator.StartGame();

            bool willExit = false;
            while (!willExit)
            {
                WriteSessionStatus();
                var line = Console.ReadLine();
                switch (line.ToLower())
                {
                    case "q":
                    case "quit":
                    willExit = true;
                    break;

                    case "r":
                    case "restart":
                    scoreCalculator.StartGame();
                    break;

                    default:
                    if (scoreCalculator.IsGameEnded())
                    {
                        Console.WriteLine("이미 게임이 종료되었습니다. (restart 명령어로 다시 시작할 수 있습니다.)");
                    }
                    else if (int.TryParse(line, out var number))
                    {
                        try
                        {
                            var prevSeq = scoreCalculator.CurrentFrameSeq;
                            scoreCalculator.KnockDownPin(number);
                            if (prevSeq != scoreCalculator.CurrentFrameSeq)
                            {
                                WriteProgress();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"에러: {e.Message}");   
                        }
                    }
                    else
                    {
                        Console.WriteLine($"알 수 없는 명령어 '{line}'입니다.");
                    }
                    break;
                }
            }
            
        }

        void WriteSessionStatus()
        {
            if (scoreCalculator.IsGameEnded())
            {
                Console.Write($"게임 완료. Score:{scoreCalculator.Score}> ");
                return;
            }

            var ballText = "";
            switch (scoreCalculator.FrameStatus)
            {
                case GameFrameStatus.FirstBall:
                ballText = "첫번째";
                break;
                case GameFrameStatus.SecondBall:
                ballText = "두번째";
                break;
                case GameFrameStatus.ThirdBall:
                ballText = "세번째";
                break;
                default:
                ballText = $"({scoreCalculator.FrameStatus.ToString()})";
                break;
            }
            
            Console.Write($"Frame:{scoreCalculator.CurrentFrameSeq}, Ball:{ballText}, Score:{scoreCalculator.Score}> ");
        }

        void WriteProgress()
        {
            Console.WriteLine("\t[점수 기록]");
            int count = 1;
            foreach (var frame in scoreCalculator.Frames)
            {
                var pinCounts = frame.PinCounts.Select(x => x.ToString());
                Console.WriteLine($"\t#{count++} {frame.Score}점 다운핀수={string.Join(",", pinCounts)})");
            }
            Console.WriteLine();
        }
    }
}
