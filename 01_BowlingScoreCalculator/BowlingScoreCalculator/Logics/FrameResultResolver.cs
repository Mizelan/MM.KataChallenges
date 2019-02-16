using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingScoreCalculator.Logics
{
    class FrameResultResolver
    {
        public static FrameResultType ResolveRegularFrameResult(IEnumerable<Roll> rolls, int maxPin)
        {
            if (rolls.Count() == 1)
            {
                if (rolls.First().DownedPinCount >= maxPin)
                {
                    return FrameResultType.Strike;
                }
                else
                {
                    return FrameResultType.InProgress;
                }
            }
            else if (rolls.Count() == 2)
            {
                if (rolls.Sum(x => x.DownedPinCount) >= maxPin)
                {
                    return FrameResultType.Spare;
                }
                else
                {
                    return FrameResultType.Open;
                }
            }

            throw new InvalidOperationException("FrameResult를 구할 수 없는 상태입니다.");
        }

        public static IEnumerable<FrameResultType> ResolveLastFrameResult(IEnumerable<Roll> rolls, int maxPin)
        {
            if (rolls.Count() == 0)
            {
                throw new InvalidOperationException("FrameResult를 구할 수 없는 상태입니다.");
            }

            if (rolls.Count() > 3)
            {
                throw new InvalidOperationException("FrameResult를 구할 수 없는 상태입니다.");
            }

            var results = new List<FrameResultType>();
            var rollEnumerator = rolls.GetEnumerator();
            var prevFrameResult = FrameResultType.None;
            Roll prevRoll = null;
            while (rollEnumerator.MoveNext())
            {
                var currentRoll = rollEnumerator.Current;
                var currentFrameResult = FrameResultType.None;
                if (currentRoll.DownedPinCount >= maxPin)
                {
                    currentFrameResult = FrameResultType.Strike;
                }
                else if (prevFrameResult == FrameResultType.InProgress)
                {
                    if (prevRoll.DownedPinCount + currentRoll.DownedPinCount >= maxPin)
                    {
                        currentFrameResult = FrameResultType.Spare;
                    }
                    else
                    {
                        currentFrameResult = FrameResultType.Open;
                    }
                }
                else if (prevFrameResult == FrameResultType.Open)
                {
                    throw new InvalidOperationException("FrameResult를 구할 수 없는 상태입니다.");
                }
                else
                {
                    currentFrameResult = FrameResultType.InProgress;
                }

                results.Add(currentFrameResult);
                prevFrameResult = currentFrameResult;
                prevRoll = currentRoll;
            }

            return results;
        }
    }
}
