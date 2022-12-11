namespace AdventOfCode.Solutions.Year2022.Day10;

class Solution : SolutionBase
{
    public Solution() : base(10, 2022, "Cathode-Ray Tube") { }


    public enum InstructionType
    {
        Noop,
        Addx
    }

    public interface IInstruction
    {
        InstructionType Type { get; set; }
    }

    public class NoopInstruction : IInstruction
    {
        public NoopInstruction()
        {
            Type = InstructionType.Noop;
        }

        public InstructionType Type { get; set; }
    }

    public class AddxInstruction : IInstruction
    {
        public AddxInstruction(int valueV)
        {
            Type = InstructionType.Addx;
            ValueV = valueV;
        }

        public InstructionType Type { get; set; }

        public int ValueV { get; private set; }
    }

    public class Cpu
    {
        public long RegisterX = 1;

        public long Cycle = 1;

        public long SignalStrength => RegisterX * Cycle;
    }

    public class Processor : IObservable<Cpu>
    {
        private readonly Cpu cpu = new();
        private readonly List<IObserver<Cpu>> observers = new();

        public IDisposable Subscribe(IObserver<Cpu> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
            return new Unsubscriber(observers, observer);
        }

        public void ProcessInstruction(IInstruction instruction)
        {
            if (instruction is NoopInstruction)
            {
                UpdateObservers();
                this.cpu.Cycle++;
            }
            else if (instruction is AddxInstruction addx)
            {
                UpdateObservers();
                this.cpu.Cycle++;
                UpdateObservers();
                this.cpu.Cycle++;
                this.cpu.RegisterX += addx.ValueV;
            }
        }

        private void UpdateObservers()
        {
            foreach (var observer in observers) observer.OnNext(cpu);
        }

        public class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<Cpu>> observers;
            private readonly IObserver<Cpu> observer;

            public Unsubscriber(List<IObserver<Cpu>> observers, IObserver<Cpu> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose()
            {
                if (!(observer == null)) observers.Remove(observer);
                GC.SuppressFinalize(this);
            }
        }
    }

    public class SignalStrengthReporter : IObserver<Cpu>
    {
        public List<long> InterestingSignalStrengths = new();

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        void IObserver<Cpu>.OnNext(Cpu cpu)
        {
            if ((cpu.Cycle - 20) % 40 == 0 && cpu.Cycle <= 220)
            {
                InterestingSignalStrengths.Add(cpu.SignalStrength);
            }
        }
    }

    public class CRT : IObserver<Cpu>
    {
        private int position = 0;

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        void IObserver<Cpu>.OnNext(Cpu cpu)
        {
            if (cpu.RegisterX - 1 <= position && position <= cpu.RegisterX + 1)
            {
                Console.Write("#");
            }
            else
            {
                Console.Write(".");
            }

            if (cpu.Cycle % 40 == 0)
            {
                position = 0;
                Console.Write(Environment.NewLine);
            }
            else
            {
                position++;
            }
        }
    }

    public IEnumerable<IInstruction> Parse()
    {
        var lines = this.Input!.SplitByNewline();
        foreach (var line in lines)
        {
            string[] splits = line.Split(" ");
            var instructionType = Enum.Parse<InstructionType>(splits[0], true);

            if (instructionType == InstructionType.Noop)
            {
                yield return new NoopInstruction();
            }
            else
            {
                yield return new AddxInstruction(int.Parse(splits[1]));
            }
        }
    }

    protected override string SolvePartOne()
    {
        var instructions = this.Parse();
        var processor = new Processor();
        var reporter = new SignalStrengthReporter();

        processor.Subscribe(reporter);

        foreach (var instruction in instructions)
        {
            processor.ProcessInstruction(instruction);
        }

        return reporter.InterestingSignalStrengths.Sum().ToString();
    }

    protected override string SolvePartTwo()
    {
        Console.WriteLine();

        var instructions = this.Parse();
        var processor = new Processor();
        var crt = new CRT();

        processor.Subscribe(crt);

        foreach (var instruction in instructions)
        {
            processor.ProcessInstruction(instruction);
        }

        Console.WriteLine();

        return "Check above!";
    }
}
