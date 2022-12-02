namespace AdventOfCode.Solutions.Year2022.Day01;

class Solution : SolutionBase
{
    public Solution() : base(01, 2022, "Calorie Counting") { }

    public class Food
    {
        public Food(int calories) 
        {
            this.Calories = calories;
        }

        public int Calories { get; }
    }

    public class Elf
    {
        public Elf(IList<Food> foods) 
        {
            Foods = foods;
        }

        public IList<Food> Foods { get; }

        public int TotalCalories => this.Foods.Sum(f => f.Calories);
    }

    protected override string SolvePartOne()
    {
        IEnumerable<Elf> elves = ParseElves();

        Elf elfCarryingMostCalories = elves.MaxBy(e => e.TotalCalories);

        return elfCarryingMostCalories.TotalCalories.ToString();
    }

    protected override string SolvePartTwo()
    {
        IEnumerable<Elf> elves = ParseElves();

        var totalCaloriesOfTop3Elves = elves.OrderByDescending(e => e.TotalCalories).Take(3).Sum(e => e.TotalCalories);

        return totalCaloriesOfTop3Elves.ToString();
    }

    private IEnumerable<Elf> ParseElves()
    {
        return this.Input
            .SplitByParagraph()
            .Select(p =>
            {
                var foods = p.SplitByNewline().Select(x => new Food(int.Parse(x))).ToList();
                return new Elf(foods);
            });
    }
}
