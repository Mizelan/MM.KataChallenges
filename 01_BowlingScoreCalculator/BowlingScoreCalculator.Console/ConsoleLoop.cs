using System;
using System.Linq;
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

            scoreCalculator = new ScoreCalculator(new GameConfig { MaxFrame = 10 });
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
                    if (scoreCalculator.IsGameOver())
                    {
                        Console.WriteLine("이미 게임이 종료되었습니다. (restart 명령어로 다시 시작할 수 있습니다.)");
                    }
                    else if (int.TryParse(line, out var number))
                    {
                        try
                        {
                            scoreCalculator.PushRoll(number);
                            var frame = scoreCalculator.Frames.Last();
                            if (frame.IsLastFrame ||
                                frame.FrameResult != FrameResultType.InProgress)
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
            if (scoreCalculator.IsGameOver())
            {
                Console.Write($"게임 완료. Score:{scoreCalculator.Score}> ");
                return;
            }

            var frameRollCount = 0;
            
            if (scoreCalculator.Frames != null)
            {
                var currentFrame = scoreCalculator.Frames.Last();
                if (currentFrame != null &&
                    currentFrame.Rolls != null)
                {

                    frameRollCount = currentFrame.Rolls.Count();
                }
            }

            var frameSeqText = "";
            if(scoreCalculator.Frames != null &&
               scoreCalculator.Frames.Count() > 0 &&
               scoreCalculator.Frames.Last().IsLastFrame)
            {
                frameSeqText = "LAST";
            }
            else
            {
                frameSeqText = (scoreCalculator.FrameSeq + 1).ToString();
            }
            

            Console.Write($"Frame:{frameSeqText}, Ball:{frameRollCount + 1}, Score:{scoreCalculator.Score}> ");
        }

        void WriteProgress()
        {
            Console.WriteLine("\t[점수 기록]");
            int count = 1;
            foreach (var frame in scoreCalculator.Frames)
            {
                var pinCountTexts = frame.Rolls.Select(x => x.DownedPinCount.ToString()).ToArray();

                if (frame.IsLastFrame)
                {
                    var frameResults = frame.FrameResults.ToArray();
                    for (int i=0; i< frameResults.Length; i++)
                    {
                        if (frameResults[i] == FrameResultType.Spare)
                        {
                            pinCountTexts[i] = "/";
                        }
                        else if (frameResults[i] == FrameResultType.Strike)
                        {
                            pinCountTexts[i] = "X";
                        }
                    }
                }
                else
                {
                    if (frame.FrameResult == FrameResultType.Spare)
                    {
                        pinCountTexts[pinCountTexts.Count() - 1] = "/";
                    }
                    else if(frame.FrameResult == FrameResultType.Strike)
                    {
                        pinCountTexts[pinCountTexts.Count() - 1] = "X";
                    }
                }

                var scoreText = frame.Score.HasValue ? frame.Score.Value.ToString() : "?";
                Console.WriteLine($"\t#{count++, -2} Pin:{{ {string.Join(",", pinCountTexts), -5} }} Score:{scoreText}");
            }
            Console.WriteLine();
        }
    }
}
