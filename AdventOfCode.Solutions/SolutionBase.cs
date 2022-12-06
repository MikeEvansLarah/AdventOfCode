global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using AdventOfCode.Solutions.Utils;

using System.Diagnostics;
using System.IO;
using System.Net;
using AdventOfCode.Services;

namespace AdventOfCode.Solutions;

public abstract class SolutionBase
{
    public int Day { get; }
    public int Year { get; }
    public string Title { get; }
    public bool Debug { get; set; }
    public string? Input { get; private set; }
    public string? DebugInput { get; private set; }

    private protected SolutionBase(int day, int year, string title, bool useDebugInput = false)
    {
        Day = day;
        Year = year;
        Title = title;
        Debug = useDebugInput;
    }

    public SolutionResult LoadAndSolve()
    {
        var sw = Stopwatch.StartNew();
        Input = LoadInput(Debug);
        DebugInput = LoadInput(true);
        sw.Stop();

        SolutionResult result = new(Day, Year, Title, Debug, sw.Elapsed, SolvePart(1), SolvePart(2));

        return result;
    }

    SolutionPartResult SolvePart(int part = 1)
    {
        if (part == 1) return SolvePart(part, SolvePartOne);
        if (part == 2) return SolvePart(part, SolvePartTwo);

        throw new InvalidOperationException("Invalid part param supplied.");
    }

    SolutionPartResult SolvePart(int part, Func<string> SolverFunction)
    {
        if (Debug)
        {
            if (string.IsNullOrEmpty(DebugInput))
            {
                throw new Exception("DebugInput is null or empty");
            }
        }
        else if (string.IsNullOrEmpty(Input))
        {
            throw new Exception("Input is null or empty");
        }

        try
        {
            var sw = Stopwatch.StartNew();
            var result = SolverFunction();
            sw.Stop();
            return string.IsNullOrEmpty(result)
                ? SolutionPartResult.Empty
                : new SolutionPartResult(part, result, sw.Elapsed);
        }
        catch (Exception)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
                return SolutionPartResult.Empty;
            }
            else
            {
                throw;
            }
        }
    }

    string LoadInput(bool debug = false)
    {
        var inputFilepath =
            $"./AdventOfCode.Solutions/Year{Year}/Day{Day:D2}/{(debug ? "debug" : "input")}";

        if (File.Exists(inputFilepath) && new FileInfo(inputFilepath).Length > 0)
        {
            return File.ReadAllText(inputFilepath);
        }

        if (debug) return "";

        try
        {
            var input = AdventOfCodeService.FetchInput(Year, Day).Result;
            Directory.CreateDirectory(Path.GetDirectoryName(inputFilepath)!);
            File.WriteAllText(inputFilepath, input);
            return input;
        }
        catch (HttpRequestException e)
        {
            var code = e.StatusCode;
            var colour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            if (code == HttpStatusCode.BadRequest)
            {
                Console.WriteLine($"Day {Day}: Received 400 when attempting to retrieve puzzle input. Your session cookie is probably not recognized.");

            }
            else if (code == HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Day {Day}: Received 404 when attempting to retrieve puzzle input. The puzzle is probably not available yet.");
            }
            else
            {
                Console.ForegroundColor = colour;
                Console.WriteLine(e.ToString());
            }
            Console.ForegroundColor = colour;
        }
        catch (InvalidOperationException)
        {
            var colour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Day {Day}: Cannot fetch puzzle input before given date (Eastern Standard Time).");
            Console.ForegroundColor = colour;
        }

        return "";
    }

    protected abstract string SolvePartOne();
    protected abstract string SolvePartTwo();
}

public readonly struct SolutionPartResult
{
    public SolutionPartResult(int part, string answer, TimeSpan time)
    {
        Part = part;
        Answer = answer;
        Time = time;
    }

    public int Part { get; }
    public string Answer { get; }
    public TimeSpan Time { get; }

    public static SolutionPartResult Empty => new();

    public override string ToString() =>
        $"  - Part{Part} => " + (string.IsNullOrEmpty(Answer)
            ? "Unsolved"
            : $"{Answer} ({Time.TotalMilliseconds}ms)");
}

public readonly struct SolutionResult
{
    public SolutionResult(int day, int year, string title, bool debug, TimeSpan inputLoadingTime, SolutionPartResult partOneResult, SolutionPartResult partTwoResult)
    {
        Day = day;
        Year = year;
        Title = title;
        Debug = debug;
        InputLoadingTime = inputLoadingTime;
        PartOneResult = partOneResult;
        PartTwoResult = partTwoResult;
    }

    public int Day { get; }
    public int Year { get; }
    public string Title { get; }
    public bool Debug { get; }
    public TimeSpan InputLoadingTime { get; }
    public SolutionPartResult PartOneResult { get; }
    public SolutionPartResult PartTwoResult { get; }

    public override string ToString() =>
        $"\n--- Day {Day}: {Title} --- {(Debug ? "!! Debug mode active, using DebugInput !!" : "")}\n"
        + $"  - Input loading... ({InputLoadingTime.TotalMilliseconds}ms)\n"
        + $"{PartOneResult}\n"
        + $"{PartTwoResult}";
}
