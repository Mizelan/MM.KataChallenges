using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingScoreCalculator.Logics
{
    public class ScoreCalculator
    {
        public readonly GameConfig GameConfig;
        public GameFrameStatus FrameStatus { get; private set; }
        public int Score => Frames.Sum(x => x.Score);
        public int CurrentFrameSeq => Frames.Count + 1;
        public IReadOnlyList<GameFrame> Frames => frames;
        List<GameFrame> frames = new List<GameFrame>();
        GameFrame currentFrame;
                
        public ScoreCalculator(GameConfig gameConfig = null)
        {
            this.GameConfig = gameConfig ?? new GameConfig();
        }

        public bool IsGameEnded() => FrameStatus == GameFrameStatus.GameEnded;
        public bool IsLastFrame() => CurrentFrameSeq == GameConfig.MaxFrame;
        public int RemainCurrentFramePinCount() => GameConfig.MaxPinCount - currentFrame.PinCounts.Sum();

        public void StartGame()
        {
            frames.Clear();
            StartNewFrame();
        }

        void StartNewFrame()
        {
            currentFrame = new GameFrame(GameConfig.MaxPinCount);
            FrameStatus = GameFrameStatus.FirstBall;
        }
        
        public void KnockDownPin(int count)
        {
            if (IsGameEnded())
            {
                throw new Exception("게임이 이미 종료 되었습니다.");
            }

            if (!IsLastFrame() &&
                currentFrame.PinCounts.Sum() + count > GameConfig.MaxPinCount)
            {
                throw new Exception("넘어진 핀이 남은 핀 갯수보다 많습니다.");
            }

            currentFrame.PinCounts.Add(count);
            FrameStatus = CalcNextFrameStatus();
            RecordPrevFrameScore();

            if (FrameStatus == GameFrameStatus.FrameEnded ||
                FrameStatus == GameFrameStatus.GameEnded)
            {
                RecordCurrentFrame();
            }

            if (FrameStatus == GameFrameStatus.FrameEnded)
            {
                StartNewFrame();
            }
        }

        void RecordCurrentFrame()
        {
            currentFrame.ResultType = currentFrame.GetFrameResultType();
            if (currentFrame.ResultType == GameFrameResultType.Open)
            {
                currentFrame.Score = currentFrame.PinCounts.Sum();
            }
            frames.Add(currentFrame);
        }

        void RecordPrevFrameScore()
        {
            var lastFrame = Frames.LastOrDefault();
            var nextLastFrame = Frames.Count >= 2 ? Frames.Reverse().Skip(1).First() : null;
            if (nextLastFrame != null)
            {
                if (nextLastFrame.IsStrike())
                {
                    if (lastFrame.IsStrike())
                    {
                        nextLastFrame.Score = GameConfig.MaxPinCount * 2 + currentFrame.PinCounts.First();
                    }
                    else
                    {
                        if (currentFrame.PinCounts.Count >= 2)
                        {
                            nextLastFrame.Score = GameConfig.MaxPinCount + currentFrame.PinCounts.Take(2).Sum();
                        }
                    }
                }
            }
            
            if (lastFrame != null)
            {
                if (lastFrame.IsStrike())
                {
                    if (currentFrame.PinCounts.Count >= 2)
                    {
                        lastFrame.Score = GameConfig.MaxPinCount + currentFrame.PinCounts.Take(2).Sum();
                    }
                }
                else if (lastFrame.IsSpare())
                {
                    lastFrame.Score = GameConfig.MaxPinCount + currentFrame.PinCounts.First();
                }
            }

            if (FrameStatus == GameFrameStatus.GameEnded)
            {
                currentFrame.Score = currentFrame.PinCounts.Sum();
            }
        }
        
        GameFrameStatus CalcNextFrameStatus()
        {
            switch (FrameStatus)
            {
                case GameFrameStatus.FirstBall:
                if (currentFrame.IsStrike())
                {
                    if (IsLastFrame())
                    {
                        return GameFrameStatus.SecondBall;
                    }
                    return GameFrameStatus.FrameEnded;
                }
                return GameFrameStatus.SecondBall;

                case GameFrameStatus.SecondBall:
                if (IsLastFrame())
                {
                    if (currentFrame.IsStrike() || currentFrame.IsSpare())
                    {
                        return GameFrameStatus.ThirdBall;
                    }
                    else
                    {
                        return GameFrameStatus.GameEnded;
                    }
                }
                else
                {
                    return GameFrameStatus.FrameEnded;
                }

                case GameFrameStatus.ThirdBall:
                return GameFrameStatus.GameEnded;

                default:
                throw new Exception($"다음 프레임 상태를 어떻게 처리할지 정의되지 않았습니다: {FrameStatus}");
            }
        }
    }
}
