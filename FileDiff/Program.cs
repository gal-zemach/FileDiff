using FileDiff.Services;
using FileDiff.Services.Data;

namespace FileDiff;

/// <summary>
/// Main class for a program that prints the line diff between 2 files
/// </summary>
internal static class Program
{
    private const int EXPECTED_ARGS_COUNT = 2;
    
    /// <summary>
    /// Prints the line diff between two files
    /// </summary>
    /// <param name="args">Paths for both files</param>
    private static void Main(string[] args)
    {
        if (args.Length < EXPECTED_ARGS_COUNT)
        {
            Console.WriteLine("Usage: FileDiff <file1> <file2>");
        }
        
        List<string> lines1 = FileService.ReadAllLines(args[0]).ToList();
        List<string> lines2 = FileService.ReadAllLines(args[1]).ToList();
        
        List<Instruction> diff = FileComparer.Compare(lines1, lines2);

        PrintInstructions(diff);
    }

    private static void PrintInstructions(List<Instruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            instruction.Print();
        }
    }
}
