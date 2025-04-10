namespace FileDiff.Diff.Data;

/// <summary>
/// Represents an instruction
/// </summary>
public abstract class Instruction
{
    /// <summary>
    /// Represents the type of instruction
    /// </summary>
    public enum Type
    {
        None, Insert, Remove
    }
    
    /// <summary>
    /// Returns the type of instruction
    /// </summary>
    public Type InstructionType { get; protected init; }
    
    /// <summary>
    /// The line the instruction should be performed at
    /// </summary>
    public int StartingLine { get; }
    
    /// <summary>
    /// The number of lines following <see cref="StartingLine"/> that will change
    /// </summary>
    public int LinesCount { get; }
    
    /// <summary>
    /// The content of the lines the instruction will act on
    /// </summary>
    public string Content { get; }
    
    protected Instruction(int startingLine, int linesCount, string content)
    {
        StartingLine = startingLine;
        LinesCount = linesCount;
        Content = content;
    }
    
    /// <summary>
    /// Prints the instruction in a human-readable format
    /// </summary>
    public abstract void Print();
}
