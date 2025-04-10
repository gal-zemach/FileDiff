namespace FileDiff.Diff.Data;

/// <summary>
/// Represents an instruction for removing lines 
/// </summary>
public class RemoveInstruction : Instruction
{
    public RemoveInstruction(int startingLineNumber, int numberOfLinesToChange)
    {
        StartingLineNumber = startingLineNumber;
        NumberOfLinesToChange = numberOfLinesToChange;
    }
    
    public override void Print()
    {
        Console.WriteLine($"Remove {NumberOfLinesToChange} lines from line {StartingLineNumber}");
    }
}