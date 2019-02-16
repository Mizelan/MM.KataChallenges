using System.Collections.Generic;

namespace BowlingScoreCalculator.Logics
{
    static class RollExtension
    {
        public static Roll[] GetNextRolls(this Roll roll, int count)
        {
            var rv = new List<Roll>();
            for (int i = 0; i < count; i++)
            {
                roll = roll.NextRoll;

                if (roll == null)
                {
                    break;
                }
                rv.Add(roll);
            }
            return rv.ToArray();
        }
    }
}
