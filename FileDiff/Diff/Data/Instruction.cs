namespace FileDiff.Diff.Data;

/// <summary>
/// Represents an instruction
/// </summary>
public abstract class Instruction : IPrintable
{
    public int StartingLineNumber { get; protected set; }
    public int NumberOfLinesToChange { get; protected set; }
    
    public abstract void Print();
}
