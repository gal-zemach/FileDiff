namespace FileDiff.Services.Data;

/// <summary>
/// Represents an instruction
/// </summary>
public abstract class Instruction : IPrintable
{
    protected int StartingLineNumber;
    protected int NumberOfLinesToChange;
    
    public abstract void Print();
}
