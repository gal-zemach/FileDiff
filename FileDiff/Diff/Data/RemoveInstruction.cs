namespace FileDiff.Diff.Data;

/// <summary>
/// Represents an instruction for removing lines 
/// </summary>
public class RemoveInstruction : Instruction
{
    public RemoveInstruction(int startingLine, int linesCount, string content) : base(
        startingLine, linesCount, content)
    {
        InstructionType = Type.Remove;
    }
    
    public override void Print()
    {
        Console.WriteLine($"Remove {LinesCount} lines from line {StartingLine}");
    }
}