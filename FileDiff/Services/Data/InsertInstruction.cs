namespace FileDiff.Services.Data;

/// <summary>
/// Represents an instruction for inserting new lines 
/// </summary>
public class InsertInstruction : Instruction
{
    private string _content;

    public InsertInstruction(int startingLineNumber, int numberOfLinesToChange, string content)
    {
        StartingLineNumber = startingLineNumber;
        NumberOfLinesToChange = numberOfLinesToChange;
        _content = content;
    }

    public override void Print()
    {
        Console.WriteLine($"Insert {NumberOfLinesToChange} lines at line {StartingLineNumber}\n{_content}");
    }
}
