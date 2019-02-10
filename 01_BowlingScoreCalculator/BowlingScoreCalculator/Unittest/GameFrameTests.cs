using BowlingScoreCalculator.Logics;
using FluentAssertions;
using Xunit;

namespace BowlingScoreCalculator.Unittest
{
    public class GameFrameTests
    {
        readonly int maxPinCount = 10;
        GameFrame gameFrame;

        public GameFrameTests()
        {
            gameFrame = new GameFrame(maxPinCount);
        }

        [Fact]
        public void IsStrike()
        {
            gameFrame.IsStrike().Should().BeFalse();

            gameFrame.PinCounts.Add(maxPinCount);
            gameFrame.IsStrike().Should().BeTrue();
            gameFrame.GetFrameResultType().Should().Be(GameFrameResultType.Strike);
        }

        [Fact]
        public void IsSpare()
        {
            gameFrame.IsSpare().Should().BeFalse();

            gameFrame.PinCounts.Add(1);
            gameFrame.PinCounts.Add(maxPinCount - 1);
            gameFrame.IsSpare().Should().BeTrue();
            gameFrame.GetFrameResultType().Should().Be(GameFrameResultType.Spare);
        }

        [Fact]
        public void CalcNextFrameStatusTest()
        {
            gameFrame.GetFrameResultType().Should().Be(GameFrameResultType.InProgress);

            gameFrame.PinCounts.Add(1);
            gameFrame.PinCounts.Add(1);
            gameFrame.GetFrameResultType().Should().Be(GameFrameResultType.Open);
        }
    }
}
