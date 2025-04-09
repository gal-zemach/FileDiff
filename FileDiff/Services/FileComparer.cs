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
        var rawInstructions = new MyersComparer(lines1, lines2).Compare();
        var instructions = ProcessInstructions(rawInstructions);

        return instructions;
    }

    private static List<Instruction> ProcessInstructions(List<RawInstruction> rawInstructions)
    {
        if (rawInstructions.Count == 0)
        {
            return new List<Instruction>();
        }
        
        List<List<RawInstruction>> groups = new List<List<RawInstruction>>();
        var currentGroup = new List<RawInstruction>(){ rawInstructions[0] };
        groups.Add(currentGroup);

        for (int i = 1; i < rawInstructions.Count; i++)
        {
            var current = rawInstructions[i];
            var previous = rawInstructions[i - 1];

            if (current.InstructionType == previous.InstructionType &&
                current.LineNumber == previous.LineNumber + 1)
            {
                currentGroup.Add(current);
            }
            else
            {
                currentGroup = new List<RawInstruction>() { current };
                groups.Add(currentGroup);
            }
        }

        var result = new List<Instruction>();
        foreach (var group in groups)
        {
            var newInstruction = BuildInstruction(group);
            if (newInstruction != null)
            {
                result.Add(newInstruction);
            }
        }
        
        return result;
    }

    private static Instruction? BuildInstruction(List<RawInstruction> instructions)
    {
        RawInstruction.Type instructionType = instructions[0].InstructionType;
        if (instructionType == RawInstruction.Type.None)
        {
            return null;
        }
        
        int startLine = instructions[0].LineNumber;
        int numberOfLines = 1;
        string content = instructions[0].Content;

        // TODO: Number of lines is not accumulating
        for (int i = 1; i < instructions.Count; i++)
        {
            content += "\n";
            numberOfLines++;
            content += instructions[i].Content;
        }

        switch (instructionType)
        {
            case RawInstruction.Type.Insert:
                return new InsertInstruction(startLine, numberOfLines, content);
            
            case RawInstruction.Type.Remove:
                return new RemoveInstruction(startLine, numberOfLines);
        }

        return null;
    }
}
