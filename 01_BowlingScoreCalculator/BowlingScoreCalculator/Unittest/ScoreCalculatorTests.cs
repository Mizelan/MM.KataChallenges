using System;
using System.Linq;
using BowlingScoreCalculator.Logics;
using FluentAssertions;
using Xunit;

namespace BowlingScoreCalculator.Unittest
{
    public class ScoreCalculatorTests
    {
        readonly int maxFrame = 4;
        readonly ScoreCalculator scoreCalculator;

        public ScoreCalculatorTests()
        {
            scoreCalculator = new ScoreCalculator(new GameConfig { MaxFrame = maxFrame });
        }

        void PushRolls(params int[] downedPinList)
        {
            foreach (var p in downedPinList)
            {
                scoreCalculator.PushRoll(p);
            }
        }

        [Fact]
        public void CheckGameOver()
        {
            scoreCalculator.IsGameOver().Should().BeFalse();
            PushRolls(10, 10, 10, 10); // 4프레임 1볼
            scoreCalculator.FrameSeq.Should().Be(4);
            scoreCalculator.IsGameOver().Should().BeFalse();
            PushRolls(10);
            scoreCalculator.IsGameOver().Should().BeFalse();
            PushRolls(10);
            scoreCalculator.IsGameOver().Should().BeTrue();

            Action action = () => PushRolls(1);
            action.Should().Throw<Exception>().WithMessage("이미 게임이 종료되었습니다.");
        }

        [Fact]
        public void CheckScore_InProgress()
        {
            PushRolls(1);
            scoreCalculator.Frames.Count().Should().Be(1);
            scoreCalculator.Frames[0].Score.Should().BeNull();
            scoreCalculator.Score.Should().Be(0); // 계산 진행 중
        }

        [Fact]
        public void CheckScore_Open()
        {
            PushRolls(1, 2);
            scoreCalculator.Frames.Count().Should().Be(1);
            scoreCalculator.Frames[0].Score.Value.Should().Be(3);
            scoreCalculator.Score.Should().Be(3);
        }

        [Fact]
        public void CheckScore_Spare()
        {
            PushRolls(1, 9, 5, 3);
            scoreCalculator.Frames.Count().Should().Be(2);
            scoreCalculator.Frames[0].Score.Value.Should().Be(15);
            scoreCalculator.Frames[1].Score.Value.Should().Be(8);
            scoreCalculator.Score.Should().Be(15 + 8);
        }

        [Fact]
        public void CheckScore_Strike()
        {
            PushRolls(10, 10, 10, 1, 2);
            scoreCalculator.Frames.Count().Should().Be(4);
            scoreCalculator.Frames[0].Score.Value.Should().Be(30);
            scoreCalculator.Frames[1].Score.Value.Should().Be(21);
            scoreCalculator.Frames[2].Score.Value.Should().Be(13);
            scoreCalculator.Frames[3].Score.Value.Should().Be(3);
            scoreCalculator.Score.Should().Be(30 + 21 + 13 + 3);
        }
    }
}
