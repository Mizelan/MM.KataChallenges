using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoreCalculator
{
    // RollRecorder
    //  Roll
    // FrameMaker
    //  Rolls -> Frames
    // Frame
    //   Rolls
    //   FrameResult
    //   Score?
    // GameFlow
    //  FrameMaker -> GameFlow
    // GameScoreCalculator
    //  GameFlow
    //  RollRecorder
    //  FrameMaker ; GetFrames[]
    //  Score : GetFrames[].Sum()

    class Program
    {
        static void Main(string[] args)
        {
            var loop = new ConsoleLoop();
            loop.Run();
        }
    }
}
