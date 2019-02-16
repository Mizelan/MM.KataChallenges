using System.Collections.Generic;
using System.Linq;

namespace BowlingScoreCalculator.Logics
{
    public enum FrameResultType
    {
        None,
        InProgress,
        Open,
        Spare,
        Strike,
    }

    public class Frame
    {
        public FrameResultType FrameResult => FrameResults.First();
        public IEnumerable<FrameResultType> FrameResults;
        public readonly bool IsLastFrame;
        public readonly Roll[] Rolls;
        public int? Score = null;
        

        public Frame(IEnumerable<FrameResultType> frameResults, IEnumerable<Roll> rolls, bool isLastFrame)
        {
            FrameResults = frameResults;
            Rolls = rolls.ToArray();
            IsLastFrame = isLastFrame;
        }
    }
}
