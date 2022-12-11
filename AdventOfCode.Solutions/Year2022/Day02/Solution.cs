namespace AdventOfCode.Solutions.Year2022.Day02;

class Solution : SolutionBase
{
    public Solution() : base(02, 2022, "Rock Paper Scissors") { }

    public enum HandShape
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3,
    }

    public enum Outcome
    {
        Win = 6,
        Draw = 3,
        Lose = 0,
    }

    public class Round
    {
        public Round(HandShape opponentHandShape, HandShape playerHandShape)
        {
            OpponentHandShape = opponentHandShape;
            PlayerHandShape = playerHandShape;

            Outcome = this switch
            {
                { PlayerHandShape: HandShape.Rock, OpponentHandShape: HandShape.Rock } => Outcome.Draw,
                { PlayerHandShape: HandShape.Rock, OpponentHandShape: HandShape.Paper } => Outcome.Lose,
                { PlayerHandShape: HandShape.Rock, OpponentHandShape: HandShape.Scissors } => Outcome.Win,
                { PlayerHandShape: HandShape.Paper, OpponentHandShape: HandShape.Rock } => Outcome.Win,
                { PlayerHandShape: HandShape.Paper, OpponentHandShape: HandShape.Paper } => Outcome.Draw,
                { PlayerHandShape: HandShape.Paper, OpponentHandShape: HandShape.Scissors } => Outcome.Lose,
                { PlayerHandShape: HandShape.Scissors, OpponentHandShape: HandShape.Rock } => Outcome.Lose,
                { PlayerHandShape: HandShape.Scissors, OpponentHandShape: HandShape.Paper } => Outcome.Win,
                { PlayerHandShape: HandShape.Scissors, OpponentHandShape: HandShape.Scissors } => Outcome.Draw,
                _ => throw new NotImplementedException(),
            };
        }

        public Round(HandShape opponentHandShape, Outcome outcome)
        {
            OpponentHandShape = opponentHandShape;
            Outcome = outcome;

            PlayerHandShape = this switch
            {
                { Outcome: Outcome.Draw, OpponentHandShape: HandShape.Rock } => HandShape.Rock,
                { Outcome: Outcome.Draw, OpponentHandShape: HandShape.Paper } => HandShape.Paper,
                { Outcome: Outcome.Draw, OpponentHandShape: HandShape.Scissors } => HandShape.Scissors,
                { Outcome: Outcome.Win, OpponentHandShape: HandShape.Rock } => HandShape.Paper,
                { Outcome: Outcome.Win, OpponentHandShape: HandShape.Paper } => HandShape.Scissors,
                { Outcome: Outcome.Win, OpponentHandShape: HandShape.Scissors } => HandShape.Rock,
                { Outcome: Outcome.Lose, OpponentHandShape: HandShape.Rock } => HandShape.Scissors,
                { Outcome: Outcome.Lose, OpponentHandShape: HandShape.Paper } => HandShape.Rock,
                { Outcome: Outcome.Lose, OpponentHandShape: HandShape.Scissors } => HandShape.Paper,
                _ => throw new NotImplementedException(),
            };
        }

        public HandShape OpponentHandShape { get; }

        public HandShape PlayerHandShape { get; }

        public Outcome Outcome { get; }

        public int Score => (int)PlayerHandShape + (int)Outcome;
    }

    private IEnumerable<Round> Parse1()
    {
        var rounds = this.Input!
            .SplitByNewline()
            .Select(
                x =>
                {
                    HandShape opponentHandShape = x.Split(' ')[0] switch
                    {
                        "A" => HandShape.Rock,
                        "B" => HandShape.Paper,
                        "C" => HandShape.Scissors,
                        _ => throw new Exception()
                    };

                    HandShape myHandShape = x.Split(' ')[1] switch
                    {
                        "X" => HandShape.Rock,
                        "Y" => HandShape.Paper,
                        "Z" => HandShape.Scissors,
                        _ => throw new Exception()
                    };

                    return new Round(opponentHandShape, myHandShape);
                });

        return rounds;
    }

    private IEnumerable<Round> Parse2()
    {
        var rounds = this.Input!
            .SplitByNewline()
            .Select(
                x =>
                {
                    HandShape opponentHandShape = x.Split(' ')[0] switch
                    {
                        "A" => HandShape.Rock,
                        "B" => HandShape.Paper,
                        "C" => HandShape.Scissors,
                        _ => throw new Exception()
                    };

                    Outcome outcome = x.Split(' ')[1] switch
                    {
                        "X" => Outcome.Lose,
                        "Y" => Outcome.Draw,
                        "Z" => Outcome.Win,
                        _ => throw new Exception()
                    };

                    return new Round(opponentHandShape, outcome);
                });

        return rounds;
    }

    protected override string SolvePartOne()
    {
        var rounds = this.Parse1();
        var totalScore = rounds.Sum(r => r.Score);
        return totalScore.ToString();
    }

    protected override string SolvePartTwo()
    {
        var rounds = this.Parse2();
        var totalScore = rounds.Sum(r => r.Score);
        return totalScore.ToString();
    }
}
