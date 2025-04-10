namespace FileDiff.Diff.Data;

/// <summary>
/// Represents an instruction for inserting new lines 
/// </summary>
public class InsertInstruction : Instruction
{
    public InsertInstruction(int startingLine, int linesCount, string content) : base(
        startingLine, linesCount, content)
    {
        InstructionType = Type.Insert;
    }

    public override void Print()
    {
        Console.WriteLine($"Insert {LinesCount} lines at line {StartingLine}\n{Content}");
    }
}
