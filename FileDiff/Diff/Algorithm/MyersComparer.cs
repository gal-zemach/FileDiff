using FileDiff.Diff.Data;
using FileDiff.Common;

namespace FileDiff.Diff.Algorithm;

/// <summary>
/// Myers diff algorithm for comparing files.
/// Explanation and references: https://blog.jcoglan.com/2017/02/12/the-myers-diff-algorithm-part-1/
/// </summary>
public static class MyersComparer
{
    /// <summary>
    /// Compares the 2 files and returns a sequence of instructions for transforming file1 into file2
    /// </summary>
    /// <param name="file1">source file</param>
    /// <param name="file2">destination file</param>
    /// <returns>Sequence of single-line instructions for transforming file1 into file2</returns>
    public static List<Instruction> Compare(IEnumerable<string> file1, IEnumerable<string> file2)
    {
        List<string> lines1 = file1.ToList();
        List<string> lines2 = file2.ToList();
        
        List<OffsetArray<int>> trace = BuildMyersTrace(lines1, lines2);
        List<Instruction> instructions = Traceback(trace, lines1, lines2);
        
        return instructions;
    }

    /// <summary>
    /// Finds the shortest amount of steps to transform lines1 into lines2.
    /// Returns a trace of all iterations for later backtracking.
    /// </summary>
    private static List<OffsetArray<int>> BuildMyersTrace(List<string> lines1, List<string> lines2)
    {
        // Main Variables:
        //      x, y: current indices in list1 and list2 respectively
        //      k = x-y: tracks how many removals/insertions we've made
        //      d: current amount of steps applied to lines1
        //      v: stores the furthest-x achieved so far in each relevant k position
        
        var trace = new List<OffsetArray<int>>();
        
        // tokenizing lists for quicker comparisons
        var tokenized = TokenizeLists(lines1, lines2);
        List<int> lines1Tokens = tokenized.Item1;
        List<int> lines2Tokens = tokenized.Item2;
        
        int maxSteps = lines1.Count + lines2.Count;
        var v = new OffsetArray<int>(2 * Math.Max(maxSteps, 1) + 1, maxSteps);
        v[1] = 0;

        for (int d = 0; d <= maxSteps; d++)
        {
            for (int k = -d; k <= d; k += 2)
            {
                int x;
                if (k == -d || (k != d && v[k - 1] < v[k + 1]))
                {
                    x = v[k + 1];  // downward move
                }
                else
                {
                    x = v[k - 1] + 1;  // rightward move
                }

                int y = x - k;
                
                // diagonal moves
                while (x < lines1Tokens.Count && y < lines2Tokens.Count && lines1Tokens[x] == lines2Tokens[y])
                {
                    x++;
                    y++;
                }
                
                v[k] = x;

                if (x >= lines1Tokens.Count && y >= lines2Tokens.Count)
                {
                    trace.Add(v.Clone());
                    return trace;
                }
            }
            
            trace.Add(v.Clone());
        }
        
        return trace;
    }

    /// <summary>
    /// Traces back the path found by the Myers algorithm while creating the set of instructions
    /// </summary>
    private static List<Instruction> Traceback(List<OffsetArray<int>> trace, List<string> lines1, List<string> lines2)
    {
        var instructions = new List<Instruction>();

        int x = lines1.Count;
        int y = lines2.Count;

        for (int d = trace.Count - 1; d >= 0; d--)
        {
            OffsetArray<int> v = trace[d];
            int k = x - y;

            int prevK;
            bool isDownwardMove = false;
            if (k == -d || k != d && v[k - 1] < v[k + 1])
            {
                isDownwardMove = true;
                prevK = k + 1;
            }
            else  // rightward move
            {
                prevK = k - 1;
            }
            
            int prevX = v[prevK];
            int prevY = prevX - prevK;

            while (x > prevX && y > prevY)  // diagonal moves
            {
                x--;
                y--;
            }

            if (d > 0)
            {
                instructions.Add(isDownwardMove
                    ? new InsertInstruction(y - 1, 1, lines2[y - 1])
                    : new RemoveInstruction(x - 1, 1, lines1[x - 1]));
            }
            
            x = prevX;
            y = prevY;
        }

        instructions.Reverse();
        return instructions;
    }

    private static Tuple<List<int>, List<int>> TokenizeLists(List<string> lines1, List<string> lines2)
    {
        Dictionary<string, int> tokensDictionary = new Dictionary<string, int>();
        int nextToken = 0;
        
        List<int> lines1Tokens = TokenizeList(lines1, tokensDictionary, ref nextToken);
        List<int> lines2Tokens = TokenizeList(lines2, tokensDictionary, ref nextToken);
        
        return Tuple.Create(lines1Tokens, lines2Tokens);
    }

    private static List<int> TokenizeList(List<string> list, Dictionary<string, int> tokensDictionary, ref int nextToken)
    {
        var listTokens = new List<int>();
        foreach (var line in list)
        {
            if (!tokensDictionary.TryGetValue(line, out var token))
            {
                token = nextToken++;
                tokensDictionary[line] = token;
            }
            listTokens.Add(token);
        }

        return listTokens;
    }
}
