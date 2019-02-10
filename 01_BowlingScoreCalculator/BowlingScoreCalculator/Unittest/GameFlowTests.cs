using System;
using System.Linq;
using BowlingScoreCalculator.Logics;
using FluentAssertions;
using Xunit;

namespace BowlingScoreCalculator.Unittest
{
    public class GameFlowTests
    {
        [Fact]
        public void StartGameTest()
        {
            var calculator = new ScoreCalculator();
            calculator.StartGame();
            calculator.IsGameEnded().Should().BeFalse();
            calculator.CurrentFrameSeq.Should().Be(1);
            calculator.Frames.Count.Should().Be(0);
            calculator.FrameStatus.Should().Be(GameFrameStatus.FirstBall);
        }

        [Fact]
        public void KnockDownPin_OverMaxPintCount()
        {
            var calculator = new ScoreCalculator();
            calculator.StartGame();
            Action action = () => calculator.KnockDownPin(calculator.GameConfig.MaxPinCount + 1);
            action.Should().Throw<Exception>().WithMessage("넘어진 핀이 남은 핀 갯수보다 많습니다.");
        }

        [Fact]
        public void KnockDownPin_AlreadyGameEnded()
        {
            var gameConfig = new GameConfig { MaxFrame = 1, MaxPinCount = 10 };
            var calculator = new ScoreCalculator(gameConfig);
            calculator.StartGame();
            calculator.KnockDownPin(1);
            calculator.KnockDownPin(1);
            calculator.IsGameEnded().Should().BeTrue();

            Action action = () => calculator.KnockDownPin(1);
            action.Should().Throw<Exception>().WithMessage("게임이 이미 종료 되었습니다.");
        }

        [Fact]
        public void KnockDownPin_StrikeCase()
        {
            var calculator = new ScoreCalculator();
            calculator.StartGame();
            calculator.MakeStrike();
            calculator.CurrentFrameSeq.Should().Be(2);
            calculator.Frames.First().ResultType.Should().Be(GameFrameResultType.Strike);
            calculator.FrameStatus.Should().Be(GameFrameStatus.FirstBall);
        }

        [Fact]
        public void KnockDownPin_SpareCase()
        {
            var calculator = new ScoreCalculator();
            calculator.StartGame();

            // 볼1
            calculator.KnockDownPin(1);
            calculator.CurrentFrameSeq.Should().Be(1);
            calculator.FrameStatus.Should().Be(GameFrameStatus.SecondBall);

            // 볼2
            calculator.MakeSpare();
            calculator.CurrentFrameSeq.Should().Be(2);
            calculator.Frames.First().ResultType.Should().Be(GameFrameResultType.Spare);
            calculator.FrameStatus.Should().Be(GameFrameStatus.FirstBall);
        }

        [Fact]
        public void KnockDownPin_LastFrame_OpenCase()
        {
            var gameConfig = new GameConfig { MaxFrame = 3, MaxPinCount = 10 };
            var calculator = new ScoreCalculator(gameConfig);
            calculator.StartGame();
            calculator.MakeStrike();
            calculator.MakeStrike();
            calculator.IsLastFrame().Should().BeTrue();

            calculator.KnockDownPin(1);
            calculator.CurrentFrameSeq.Should().Be(gameConfig.MaxFrame);
            calculator.FrameStatus.Should().Be(GameFrameStatus.SecondBall);

            calculator.KnockDownPin(1);
            calculator.FrameStatus.Should().Be(GameFrameStatus.GameEnded);
            calculator.IsGameEnded().Should().BeTrue();
        }

        [Fact]
        public void KnockDownPin_LastFrame_SpareCase()
        {
            var gameConfig = new GameConfig { MaxFrame = 3, MaxPinCount = 10 };
            var calculator = new ScoreCalculator(gameConfig);
            calculator.StartGame();
            calculator.MakeStrike();
            calculator.MakeStrike();
            calculator.IsLastFrame().Should().BeTrue();

            calculator.KnockDownPin(1);
            calculator.CurrentFrameSeq.Should().Be(gameConfig.MaxFrame);
            calculator.FrameStatus.Should().Be(GameFrameStatus.SecondBall);

            calculator.MakeSpare();
            calculator.FrameStatus.Should().Be(GameFrameStatus.ThirdBall);

            calculator.KnockDownPin(1);
            calculator.FrameStatus.Should().Be(GameFrameStatus.GameEnded);
            calculator.IsGameEnded().Should().BeTrue();
        }

        [Fact]
        public void KnockDownPin_LastFrame_StrikeCase()
        {
            var gameConfig = new GameConfig { MaxFrame = 3, MaxPinCount = 10 };
            var calculator = new ScoreCalculator(gameConfig);
            calculator.StartGame();
            calculator.MakeStrike();
            calculator.MakeStrike();
            calculator.IsLastFrame().Should().BeTrue();

            calculator.MakeStrike();
            calculator.CurrentFrameSeq.Should().Be(gameConfig.MaxFrame);
            calculator.FrameStatus.Should().Be(GameFrameStatus.SecondBall);

            calculator.MakeStrike();
            calculator.FrameStatus.Should().Be(GameFrameStatus.ThirdBall);

            calculator.MakeStrike();
            calculator.FrameStatus.Should().Be(GameFrameStatus.GameEnded);
            calculator.IsGameEnded().Should().BeTrue();
        }
    }
}
