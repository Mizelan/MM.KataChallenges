using BowlingScoreCalculator.Logics;
using FluentAssertions;
using Xunit;

namespace BowlingScoreCalculator.Unittest
{
    public class ScoreCalculateTests
    {
        ScoreCalculator calculator;

        public ScoreCalculateTests()
        {
            calculator = new ScoreCalculator();
        }

        [Fact]
        public void InitializeCheck()
        {
            calculator.StartGame();
            calculator.Score.Should().Be(0);
        }

        [Fact]
        public void FrameInProgressCase()
        {
            calculator.StartGame();
            calculator.KnockDownPin(1);
            calculator.Score.Should().Be(0);
        }

        [Fact]
        public void OpenCase()
        {
            calculator.StartGame();
            calculator.KnockDownPin(1);
            calculator.KnockDownPin(2);
            calculator.Score.Should().Be(3);
        }

        [Fact]
        public void SpareCase()
        {
            calculator.StartGame();
            calculator.KnockDownPin(1);
            calculator.MakeSpare();
            calculator.Score.Should().Be(0);

            calculator.KnockDownPin(1);
            calculator.KnockDownPin(1);
            calculator.Frames[0].Score.Should().Be(11);
            calculator.Frames[1].Score.Should().Be(2);
            calculator.Score.Should().Be(13);
        }

        [Fact]
        public void StrikeCase()
        {
            calculator.StartGame();
            calculator.MakeStrike();
            calculator.Score.Should().Be(0);

            calculator.KnockDownPin(1);
            calculator.KnockDownPin(1);
            calculator.Frames[0].Score.Should().Be(12);
            calculator.Frames[1].Score.Should().Be(2);
            calculator.Score.Should().Be(14);
        }

        [Fact]
        public void DoubleStrikeCase()
        {
            calculator.StartGame();
            calculator.MakeStrike();
            calculator.Score.Should().Be(0);

            calculator.MakeStrike();
            calculator.Score.Should().Be(0);

            calculator.KnockDownPin(1);
            calculator.KnockDownPin(1);
            calculator.Frames[0].Score.Should().Be(21);
            calculator.Frames[1].Score.Should().Be(12);
            calculator.Frames[2].Score.Should().Be(2);
            calculator.Score.Should().Be(35);
        }

        [Fact]
        public void LastFrame_PrevStrike_CurrentOpenCase()
        {
            var gameConfig = new GameConfig { MaxFrame = 2 };
            calculator = new ScoreCalculator(gameConfig);
            calculator.StartGame();
            calculator.MakeStrike();
            calculator.Score.Should().Be(0);

            calculator.KnockDownPin(1);
            calculator.KnockDownPin(1);
            calculator.Frames[0].Score.Should().Be(12);
            calculator.Frames[1].Score.Should().Be(2);
            calculator.Score.Should().Be(14);
            calculator.IsGameEnded().Should().BeTrue();
        }

        [Fact]
        public void LastFrame_PrevTwoStrike_CurrentOpenCase()
        {
            var gameConfig = new GameConfig { MaxFrame = 3 };
            calculator = new ScoreCalculator(gameConfig);
            calculator.StartGame();
            calculator.MakeStrike();
            calculator.MakeStrike();
            calculator.Score.Should().Be(0);

            calculator.KnockDownPin(1);
            calculator.KnockDownPin(1);
            calculator.Frames[0].Score.Should().Be(21);
            calculator.Frames[1].Score.Should().Be(12);
            calculator.Frames[2].Score.Should().Be(2);
            calculator.Score.Should().Be(35);
            calculator.IsGameEnded().Should().BeTrue();
        }

        [Fact]
        public void LastFrame_PrevStrike_CurrentDoubleStrikeCase()
        {
            var gameConfig = new GameConfig { MaxFrame = 3 };
            calculator = new ScoreCalculator(gameConfig);
            calculator.StartGame();
            calculator.MakeStrike();
            calculator.MakeStrike();
            calculator.Score.Should().Be(0);

            calculator.MakeStrike();
            calculator.MakeStrike();
            calculator.MakeStrike();
            calculator.Frames[0].Score.Should().Be(30);
            calculator.Frames[1].Score.Should().Be(30);
            calculator.Frames[2].Score.Should().Be(30);
            calculator.Score.Should().Be(90);
            calculator.IsGameEnded().Should().BeTrue();
        }
    }
}