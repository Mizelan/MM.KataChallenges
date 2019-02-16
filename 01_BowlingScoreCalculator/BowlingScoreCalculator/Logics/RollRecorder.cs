using System.Collections.Generic;

namespace BowlingScoreCalculator.Logics
{
    class RollRecorder
    {
        public IEnumerable<Roll> Rolls => rolls;
        List<Roll> rolls = new List<Roll>();
        Roll prevRoll = null;

        public void Record(int downedPinCount)
        {
            var roll = new Roll { DownedPinCount = downedPinCount };
            rolls.Add(roll);

            if (prevRoll != null)
            {
                prevRoll.NextRoll = roll;
            }

            prevRoll = roll;
        }

        public void Reset()
        {
            rolls.Clear();
        }
    }
}
