using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingScoreCalculator.Logics
{
    class FrameMaker
    {
        RollRecorder rollRecorder;
        GameConfig gameConfig;

        public FrameMaker(RollRecorder rollRecorder, GameConfig gameConfig)
        {
            this.rollRecorder = rollRecorder;
            this.gameConfig = gameConfig;
        }

        public IEnumerable<Frame> GetFrames()
        {
            var rollsEnumerator = rollRecorder.Rolls.GetEnumerator();
            var rollBuffer = new List<Roll>();
            var result = new List<Frame>();

            while (rollsEnumerator.MoveNext())
            {
                if (result.Count() > gameConfig.MaxFrame - 1)
                {
                    throw new InvalidOperationException("최대 프레임에 필요한 투구 횟수보다 많습니다.");
                }

                rollBuffer.Add(rollsEnumerator.Current);

                
                var isLastFrame = CheckLastFrame(result);
                if (!IsFrameMakable(isLastFrame, rollBuffer))
                {
                    continue;
                }

                var frame = MakeFrame(isLastFrame, rollBuffer);
                result.Add(frame);
                rollBuffer.Clear();
            }

            if (rollBuffer.Count() > 0)
            {
                result.Add(MakeFrame(CheckLastFrame(result), rollBuffer));
            }

            return result;
        }

        bool CheckLastFrame(List<Frame> frames)
        {
            return frames.Count() == gameConfig.MaxFrame - 1;
        }

        bool IsFrameMakable(bool isLastFrame, IEnumerable<Roll> rolls)
        {
            var frameResultTypes = GetFrameResults(isLastFrame, rolls);
            if (!isLastFrame)
            {
                return !frameResultTypes.Contains(FrameResultType.InProgress);
            }

            if (frameResultTypes.Contains(FrameResultType.Open))
            {
                return true;
            }

            return frameResultTypes.Count() >= 3;
        }

        Frame MakeFrame(bool isLastFrame, IEnumerable<Roll> rolls)
        {
            return new Frame(GetFrameResults(isLastFrame, rolls), rolls, isLastFrame);
        }

        FrameResultType[] GetFrameResults(bool isLastFrame, IEnumerable<Roll> rolls)
        {
            if (isLastFrame)
            {
                return FrameResultResolver.ResolveLastFrameResult(rolls, gameConfig.MaxPinCount).ToArray();
            }
            else
            {
                return new FrameResultType[] { FrameResultResolver.ResolveRegularFrameResult(rolls, gameConfig.MaxPinCount) };
            }
        }
    }
}
