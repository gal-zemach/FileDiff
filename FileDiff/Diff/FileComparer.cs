using FileDiff.Diff.Algorithm;
using FileDiff.Diff.Data;

namespace FileDiff.Diff;

public static class FileComparer
{
    /// <summary>
    /// Returns a sequence of instructions for transforming the 1st list of lines into the 2nd one 
    /// </summary>
    /// <param name="lines1">First list of lines</param>
    /// <param name="lines2">Second list of lines</param>
    /// <returns>List of <see cref="Instruction"/> containing the sequence</returns>
    public static List<Instruction> Compare(IEnumerable<string> lines1, IEnumerable<string> lines2)
    {
        var rawInstructions = new MyersComparer(lines1, lines2).Compare();
        var instructions = ProcessInstructions(rawInstructions);

        return instructions;
    }

    private static List<Instruction> ProcessInstructions(List<Instruction> rawInstructions)
    {
        if (rawInstructions.Count == 0)
        {
            return new List<Instruction>();
        }
        
        List<List<Instruction>> groups = GroupRawInstructions(rawInstructions);
        List<Instruction> instructions = BuildInstructions(groups);
        
        return instructions;
    }

    private static List<List<Instruction>> GroupRawInstructions(List<Instruction> rawInstructions)
    {
        List<List<Instruction>> groups = new List<List<Instruction>>();
        var currentGroup = new List<Instruction>(){ rawInstructions[0] };
        groups.Add(currentGroup);

        for (int i = 1; i < rawInstructions.Count; i++)
        {
            var previous = rawInstructions[i - 1];
            var current = rawInstructions[i];

            if (current.InstructionType == previous.InstructionType &&
                current.StartingLine - 1 == previous.StartingLine)
            {
                currentGroup.Add(current);
            }
            else
            {
                currentGroup = new List<Instruction>() { current };
                groups.Add(currentGroup);
            }
        }

        return groups;
    }

    private static List<Instruction> BuildInstructions(List<List<Instruction>> rawInstructionGroups)
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

    private static Instruction? BuildInstruction(List<Instruction> instructions, ref int lineNumberOffset)
    {
        Instruction.Type instructionType = instructions[0].InstructionType;
        if (instructionType == Instruction.Type.None)
        {
            return null;
        }
        
        int startLine = instructions[0].StartingLine + 1;  // 1 is added for 1-based printing
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
            case Instruction.Type.Insert:
                // no need to add lineNumberOffset since all lines up-to startLine are identical to file2
                result = new InsertInstruction(startLine, numberOfChangedLines, content);
                lineNumberOffset += numberOfChangedLines;
                break;

            case Instruction.Type.Remove:
                result = new RemoveInstruction(startLine + lineNumberOffset, numberOfChangedLines, content);
                lineNumberOffset -= numberOfChangedLines;
                break;
        }
        
        return result;
    }
}
