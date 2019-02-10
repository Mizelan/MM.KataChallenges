using BowlingScoreCalculator.Logics;

namespace BowlingScoreCalculator.Unittest
{
    public static class ScoreCalculatorTestExtensions
    {
        public static void MakeStrike(this ScoreCalculator calculator)
        {
            calculator.KnockDownPin(calculator.GameConfig.MaxPinCount);
        }

        public static void MakeSpare(this ScoreCalculator calculator)
        {
            calculator.KnockDownPin(calculator.RemainCurrentFramePinCount());
        }
    }
}
