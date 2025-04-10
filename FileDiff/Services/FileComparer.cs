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
        
        List<List<RawInstruction>> groups = GroupRawInstructions(rawInstructions);
        List<Instruction> instructions = BuildInstructions(groups);
        
        return instructions;
    }

    private static List<List<RawInstruction>> GroupRawInstructions(List<RawInstruction> rawInstructions)
    {
        List<List<RawInstruction>> groups = new List<List<RawInstruction>>();
        var currentGroup = new List<RawInstruction>(){ rawInstructions[0] };
        groups.Add(currentGroup);

        for (int i = 1; i < rawInstructions.Count; i++)
        {
            var previous = rawInstructions[i - 1];
            var current = rawInstructions[i];

            if (current.InstructionType == previous.InstructionType &&
                current.LineNumber - 1 == previous.LineNumber)
            {
                currentGroup.Add(current);
            }
            else
            {
                currentGroup = new List<RawInstruction>() { current };
                groups.Add(currentGroup);
            }
        }

        return groups;
    }

    private static List<Instruction> BuildInstructions(List<List<RawInstruction>> rawInstructionGroups)
    {
        var result = new List<Instruction>();
        int lineNumberOffset = 0;
        foreach (var group in rawInstructionGroups)
        {
            var newInstruction = BuildInstruction(group, ref lineNumberOffset);
            if (newInstruction != null)
            {
                result.Add(newInstruction);
            }
        }
        
        return result;
    }

    private static Instruction? BuildInstruction(List<RawInstruction> instructions, ref int lineNumberOffset)
    {
        RawInstruction.Type instructionType = instructions[0].InstructionType;
        if (instructionType == RawInstruction.Type.None)
        {
            return null;
        }
        
        int startLine = instructions[0].LineNumber + 1;  // 1 is added for 1-based printing
        int numberOfChangedLines = 1;
        string content = instructions[0].Content;

        for (int i = 1; i < instructions.Count; i++)
        {
            content += "\n";
            numberOfChangedLines++;
            content += instructions[i].Content;
        }
        
        Instruction? result = null;
        switch (instructionType)
        {
            case RawInstruction.Type.Insert:
                result = new InsertInstruction(startLine, numberOfChangedLines, content);
                lineNumberOffset += numberOfChangedLines;
                break;

            case RawInstruction.Type.Remove:
                result = new RemoveInstruction(startLine + lineNumberOffset, numberOfChangedLines);
                lineNumberOffset -= numberOfChangedLines;
                break;
        }
        
        result.Print();
        
        return result;
    }
}
