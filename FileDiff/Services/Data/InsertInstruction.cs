namespace FileDiff.Services.Data;

/// <summary>
/// Represents an instruction for inserting new lines 
/// </summary>
public class InsertInstruction : Instruction
{
    public string Content { get; private set; }

    public InsertInstruction(int startingLineNumber, int numberOfLinesToChange, string content)
    {
        StartingLineNumber = startingLineNumber;
        NumberOfLinesToChange = numberOfLinesToChange;
        Content = content;
    }

    public override void Print()
    {
        Console.WriteLine($"Insert {NumberOfLinesToChange} lines at line {StartingLineNumber}\n{Content}");
    }
}
