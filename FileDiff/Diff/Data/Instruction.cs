namespace FileDiff.Diff.Data;

/// <summary>
/// Represents an instruction
/// </summary>
public abstract class Instruction : IPrintable
{
    public enum Type
    {
        None, Insert, Remove
    }

    protected Instruction(int startingLine, int linesCount, string content)
    {
        StartingLine = startingLine;
        LinesCount = linesCount;
        Content = content;
    }

    public Type InstructionType { get; protected init; }
    public int StartingLine { get; protected set; }
    public int LinesCount { get; protected set; }
    public string Content { get; protected set; }
    
    public abstract void Print();
}
