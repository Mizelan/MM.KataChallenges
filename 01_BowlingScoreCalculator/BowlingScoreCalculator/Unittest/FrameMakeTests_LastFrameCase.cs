using System;
using System.Linq;
using BowlingScoreCalculator.Logics;
using FluentAssertions;
using Xunit;

namespace BowlingScoreCalculator.Unittest
{
    public class FrameMakeTests_LastFrameCase
    {
        readonly int maxFrameCount = 1;
        readonly RollRecorder rollRecorder;
        readonly FrameMaker frameMaker;

        public FrameMakeTests_LastFrameCase()
        {
            rollRecorder = new RollRecorder();
            frameMaker = new FrameMaker(rollRecorder, new GameConfig { MaxFrame = maxFrameCount });
        }

        void PushRolls(params int[] downedPinList)
        {
            foreach (var p in downedPinList)
            {
                rollRecorder.Record(p);
            }
        }

        [Fact]
        public void GetFrames_InProgress()
        {
            PushRolls(1);
            var frames = frameMaker.GetFrames();
            frames.Count().Should().Be(1);
            var frame = frames.First();
            frame.FrameResult.Should().Be(FrameResultType.InProgress);
            frame.Rolls.Select(x => x.DownedPinCount).Should().BeEquivalentTo(new int[] { 1 });
        }

        [Fact]
        public void GetSingleFrame_Open()
        {
            PushRolls(1, 2);
            var frames = frameMaker.GetFrames();
            frames.Count().Should().Be(1);
            var frame = frames.First();
            frame.FrameResults.Should().BeEquivalentTo(new FrameResultType[] { FrameResultType.InProgress, FrameResultType.Open });
            frame.Rolls.Select(x => x.DownedPinCount).Should().BeEquivalentTo(new int[] { 1, 2});
        }

        [Fact]
        public void GetMultiFrames_Open()
        {
            PushRolls(1, 2, 3, 4);
            Action action = () => frameMaker.GetFrames();
            action.Should().Throw<InvalidOperationException>().WithMessage("최대 프레임에 필요한 투구 횟수보다 많습니다.");
        }

        [Fact]
        public void GetMultiFrames_Strike()
        {
            PushRolls(10, 10, 10);
            var frames = frameMaker.GetFrames();
            frames.Count().Should().Be(1);
            var frame = frames.First();
            frame.FrameResults.Should().BeEquivalentTo(new FrameResultType[] { FrameResultType.Strike, FrameResultType.Strike, FrameResultType.Strike });
            frame.Rolls.Select(x => x.DownedPinCount).Should().BeEquivalentTo(new int[] { 10, 10, 10 });
        }

        [Fact]
        public void GetMultiFrames_Spare()
        {
            PushRolls(1, 9, 2);
            var frames = frameMaker.GetFrames();
            frames.Count().Should().Be(1);
            var frame = frames.First();
            frame.FrameResults.Should().BeEquivalentTo(new FrameResultType[] { FrameResultType.InProgress, FrameResultType.Spare, FrameResultType.InProgress });
            frame.Rolls.Select(x => x.DownedPinCount).Should().BeEquivalentTo(new int[] { 1, 9, 2 });
        }
    }
}
