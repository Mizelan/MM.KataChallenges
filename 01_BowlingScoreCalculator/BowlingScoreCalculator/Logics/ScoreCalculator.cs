using System;
using System.Linq;

namespace BowlingScoreCalculator.Logics
{
    public class ScoreCalculator
    {
        public int FrameSeq => Frames == null ? 0 : Frames.Count();
        public int Score => Frames == null ? 0 : Frames.Where(x => x.Score.HasValue).Sum(x => x.Score.Value);
        public Frame[] Frames { get; private set; }
        readonly GameConfig gameConfig;
        readonly RollRecorder rollRecorder;
        readonly FrameMaker frameMaker;

        public ScoreCalculator(GameConfig gameConfig = null)
        {
            this.gameConfig = gameConfig ?? new GameConfig();
            this.rollRecorder = new RollRecorder();
            this.frameMaker = new FrameMaker(rollRecorder, this.gameConfig);
        }

        public void StartGame()
        {
            rollRecorder.Reset();
            Frames = null;
        }

        public void PushRoll(int pinCount)
        {
            if (IsGameOver())
            {
                throw new InvalidOperationException("이미 게임이 종료되었습니다.");
            }

            rollRecorder.Record(pinCount);
            Frames = frameMaker.GetFrames().ToArray();
            FillFramesScore(); 
        }

        void FillFramesScore()
        {
            var framesEnumerator = Frames.GetEnumerator();
            while (framesEnumerator.MoveNext())
            {
                var currentFrame = framesEnumerator.Current as Frame;
                int bonusNextRollCount = 0;
                if (currentFrame.FrameResult == FrameResultType.Spare)
                {
                    bonusNextRollCount = 1;
                }
                else if (currentFrame.FrameResult == FrameResultType.Strike)
                {
                    bonusNextRollCount = 2;
                }
                else if (!currentFrame.IsLastFrame &&
                        currentFrame.FrameResult == FrameResultType.InProgress)
                {
                    continue;
                }

                var baseScore = currentFrame.Rolls.Sum(x => x.DownedPinCount);
                var nextRolls = currentFrame.Rolls.Last().GetNextRolls(bonusNextRollCount);
                if (!IsGameOver() && nextRolls.Count() != bonusNextRollCount)
                {
                    // 보너스에 충분한 볼이 모이지 않았음
                    currentFrame.Score = null;
                }
                else
                {
                    var bonusScore = nextRolls.Sum(x => x.DownedPinCount);
                    currentFrame.Score = baseScore + bonusScore;
                }
            }
        }

        public bool IsGameOver()
        {
            if (Frames == null)
            {
                return false;
            }

            if (Frames.Count() < gameConfig.MaxFrame)
            {
                return false;
            }

            if (Frames.Last().Rolls.Count() >= 3)
            {
                return true;
            }

            if (Frames.Last().FrameResults.Contains(FrameResultType.Open))
            {
                return true;
            }

            return false;
        }
    }
    
}
