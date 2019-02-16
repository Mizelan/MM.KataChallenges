using System;
using System.Linq;
using BowlingScoreCalculator.Logics;
using FluentAssertions;
using Xunit;

namespace BowlingScoreCalculator.Unittest
{
    public class FrameResultResolverTests
    {
        static readonly int maxPinCount = 10;

        static Roll[] CreateRolls(params int[] downedPinList)
        {
            return downedPinList.Select(x => new Roll { DownedPinCount = x }).ToArray();
        }

        public class RegularFrameCases
        {
            [Fact]
            public void ResolveRegularFrameResult_Strike()
            {
                var resultType = FrameResultResolver.ResolveRegularFrameResult(CreateRolls(maxPinCount), maxPinCount);
                resultType.Should().Be(FrameResultType.Strike);
            }

            [Fact]
            public void ResolveRegularFrameResult_Spare()
            {
                var resultType = FrameResultResolver.ResolveRegularFrameResult(CreateRolls(maxPinCount - 1, 1), maxPinCount);
                resultType.Should().Be(FrameResultType.Spare);
            }

            [Fact]
            public void ResolveRegularFrameResult_InProgress()
            {
                var resultType = FrameResultResolver.ResolveRegularFrameResult(CreateRolls(1), maxPinCount);
                resultType.Should().Be(FrameResultType.InProgress);
            }

            [Fact]
            public void ResolveRegularFrameResult_Open()
            {
                var resultType = FrameResultResolver.ResolveRegularFrameResult(CreateRolls(1, 2), maxPinCount);
                resultType.Should().Be(FrameResultType.Open);
            }

            [Fact]
            public void ResolveRegularFrameResult_TooMuchRolls()
            {
                Action action = () => FrameResultResolver.ResolveRegularFrameResult(CreateRolls(1, 2, 3), maxPinCount);
                action.Should().Throw<Exception>().WithMessage("FrameResult를 구할 수 없는 상태입니다.");
            }

            [Fact]
            public void ResolveRegularFrameResult_EmptyRolls()
            {
                Action action = () => FrameResultResolver.ResolveRegularFrameResult(new Roll[0], maxPinCount);
                action.Should().Throw<Exception>().WithMessage("FrameResult를 구할 수 없는 상태입니다.");
            }
        }

        public class LastFrameCases
        {
            [Fact]
            public void ResolveLastFrameResult_CheckRollsCountRange()
            {
                Action action = () => FrameResultResolver.ResolveLastFrameResult(new Roll[0], maxPinCount);
                action.Should().Throw<Exception>().WithMessage("FrameResult를 구할 수 없는 상태입니다.");

                action = () => FrameResultResolver.ResolveLastFrameResult(CreateRolls(1,2,3,4), maxPinCount);
                action.Should().Throw<Exception>().WithMessage("FrameResult를 구할 수 없는 상태입니다.");
            }

            [Fact]
            public void ResolveLastFrameResult_Stirkes()
            {
                var resultType = FrameResultResolver.ResolveLastFrameResult(CreateRolls(maxPinCount), maxPinCount);
                resultType.Should().BeEquivalentTo(new FrameResultType[] { FrameResultType.Strike });

                resultType = FrameResultResolver.ResolveLastFrameResult(CreateRolls(maxPinCount, maxPinCount), maxPinCount);
                resultType.Should().BeEquivalentTo(new FrameResultType[] { FrameResultType.Strike, FrameResultType.Strike });

                resultType = FrameResultResolver.ResolveLastFrameResult(CreateRolls(maxPinCount, maxPinCount, maxPinCount), maxPinCount);
                resultType.Should().BeEquivalentTo(new FrameResultType[] { FrameResultType.Strike, FrameResultType.Strike, FrameResultType.Strike });
            }

            [Fact]
            public void ResolveLastFrameResult_Open()
            {
                var resultType = FrameResultResolver.ResolveLastFrameResult(CreateRolls(1), maxPinCount);
                resultType.Should().BeEquivalentTo(new FrameResultType[] { FrameResultType.InProgress });

                resultType = FrameResultResolver.ResolveLastFrameResult(CreateRolls(1, 2), maxPinCount);
                resultType.Should().BeEquivalentTo(new FrameResultType[] { FrameResultType.InProgress, FrameResultType.Open });

                Action action = () => FrameResultResolver.ResolveLastFrameResult(CreateRolls(1,2,3), maxPinCount);
                action.Should().Throw<Exception>().WithMessage("FrameResult를 구할 수 없는 상태입니다.");
            }

            [Fact]
            public void ResolveLastFrameResult_Spare()
            {
                var resultType = FrameResultResolver.ResolveLastFrameResult(CreateRolls(1, maxPinCount - 1), maxPinCount);
                resultType.Should().BeEquivalentTo(new FrameResultType[] { FrameResultType.InProgress, FrameResultType.Spare });

                resultType = FrameResultResolver.ResolveLastFrameResult(CreateRolls(1, maxPinCount - 1, maxPinCount), maxPinCount);
                resultType.Should().BeEquivalentTo(new FrameResultType[] { FrameResultType.InProgress, FrameResultType.Spare, FrameResultType.Strike });
            }

            [Fact]
            public void ResolveLastFrameResult_MixedCase()
            {
                var resultType = FrameResultResolver.ResolveLastFrameResult(CreateRolls(maxPinCount, 1, maxPinCount - 1), maxPinCount);
                resultType.Should().BeEquivalentTo(new FrameResultType[] { FrameResultType.Strike, FrameResultType.InProgress, FrameResultType.Spare });
            }
        }
    }
}
