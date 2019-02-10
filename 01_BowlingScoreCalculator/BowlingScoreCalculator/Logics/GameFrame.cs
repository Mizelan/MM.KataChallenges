using System.Collections.Generic;
using System.Linq;

namespace BowlingScoreCalculator.Logics
{
    public class GameFrame
    {
        public GameFrameResultType ResultType = GameFrameResultType.InProgress;
        public List<int> PinCounts = new List<int>();
        public int Score = 0;
        readonly int maxPinCount;

        public GameFrame(int maxPinCount)
        {
            this.maxPinCount = maxPinCount;
        }

        public bool IsStrike()
        {
            if (PinCounts.Count == 0)
            {
                return false;
            }
            return PinCounts.Last() == maxPinCount;
        }

        public bool IsSpare()
        {
            return PinCounts.Count == 2 && PinCounts.Sum() == maxPinCount;
        }

        public GameFrameResultType GetFrameResultType()
        {
            if (IsStrike())
            {
                return GameFrameResultType.Strike;
            }

            if (IsSpare())
            {
                return GameFrameResultType.Spare;
            }

            if (PinCounts.Count == 2)
            {
                return GameFrameResultType.Open;
            }

            return GameFrameResultType.InProgress;
        }
    }
}
