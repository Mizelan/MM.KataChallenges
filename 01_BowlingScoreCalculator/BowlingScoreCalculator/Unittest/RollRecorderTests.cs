using System;
using System.Linq;
using BowlingScoreCalculator.Logics;
using FluentAssertions;
using Xunit;

namespace BowlingScoreCalculator.Unittest
{
    public class RollRecorderTests
    {
        readonly RollRecorder rollRecorder;

        public RollRecorderTests()
        {
            rollRecorder = new RollRecorder();
        }

        void PushRolls(params int[] downedPinList)
        {
            foreach (var p in downedPinList)
            {
                rollRecorder.Record(p);
            }
        }

        [Fact]
        public void NextRoll()
        {
            PushRolls(1);
            var nextRolls = rollRecorder.Rolls.First().GetNextRolls(10);
            nextRolls.Length.Should().Be(0);
            rollRecorder.Reset();

            PushRolls(1, 2);
            nextRolls = rollRecorder.Rolls.First().GetNextRolls(10);
            nextRolls.Length.Should().Be(1);
            nextRolls.Select(x => x.DownedPinCount).Should().BeEquivalentTo(new int[] { 2 });
            rollRecorder.Reset();

            PushRolls(1, 2, 3);
            nextRolls = rollRecorder.Rolls.First().GetNextRolls(10);
            nextRolls.Length.Should().Be(2);
            nextRolls.Select(x => x.DownedPinCount).Should().BeEquivalentTo(new int[] { 2, 3 });
            rollRecorder.Reset();
        }
    }
}
