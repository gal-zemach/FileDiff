using FileDiff.Services.Algorithm;
using FileDiff.Services.Data;

namespace FileDiff.Services;

public static class FileComparer
{
    /// <summary>
    /// Returns a sequence of instructions for transforming the 1st list of lines into the 2nd one 
    /// </summary>
    /// <param name="lines1">First list of lines</param>
    /// <param name="lines2">Second list of lines</param>
    /// <returns>List of <see cref="Instruction"/> containing the sequence</returns>
    public static List<Instruction> Compare(List<string> lines1, List<string> lines2)
    {
        
        return new MyersComparer(lines1, lines2).Compare();
    }
}
