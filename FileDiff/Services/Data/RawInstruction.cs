namespace FileDiff.Services.Data;

public struct RawInstruction(RawInstruction.Type instructionType, int lineNumber, string content)
{
    public enum Type
    {
        None, Insert, Remove, Match
    }
    
    public Type InstructionType = instructionType;
    public int LineNumber = lineNumber;
    public string Content = content;
}
