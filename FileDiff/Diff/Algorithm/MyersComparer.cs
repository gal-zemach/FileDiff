using FileDiff.Diff.Data;

namespace FileDiff.Diff.Algorithm;

/// <summary>
/// Myers diff algorithm for comparing files.
/// Explanation and references: https://blog.jcoglan.com/2017/02/12/the-myers-diff-algorithm-part-1/
/// </summary>
public class MyersComparer
{
    private readonly List<string> _lines1;
    private readonly List<string> _lines2;

    private int MaxSteps => _lines1.Count + _lines2.Count;
    
    /// <summary>
    /// Used for negative array indices
    /// </summary>
    private int Index(int i) => MaxSteps + i;
    
    public MyersComparer(IEnumerable<string> lines1, IEnumerable<string> lines2)
    {
        _lines1 = lines1.ToList();
        _lines2 = lines2.ToList();
    }

    /// <summary>
    /// Compares the 2 files and returns a sequence of instructions for transforming file1 into file2
    /// </summary>
    public List<Instruction> Compare()
    {
        List<int[]> trace = BuildMyersTrace(_lines1, _lines2);
        List<Instruction> instructions = Traceback(trace);
        
        return instructions;
    }

    private List<int[]> BuildMyersTrace(List<string> lines1, List<string> lines2)
    {
        var trace = new List<int[]>();
        
        // tokenizing lists for quicker comparisons
        var tokenized = TokenizeLists(lines1, lines2);
        List<int> lines1Tokens = tokenized.Item1;
        List<int> lines2Tokens = tokenized.Item2;
        
        int[] v = new int[2 * Math.Max(MaxSteps, 1) + 1];
        v[Index(1)] = 0;

        for (int d = 0; d <= MaxSteps; d++)
        {
            for (int k = -d; k <= d; k += 2)
            {
                int x;
                if (k == -d || (k != d && v[Index(k - 1)] < v[Index(k + 1)]))
                {
                    x = v[Index(k + 1)];  // downward move
                }
                else
                {
                    x = v[Index(k - 1)] + 1;  // rightward move
                }

                int y = x - k;
                
                // diagonal moves
                while (x < lines1Tokens.Count && y < lines2Tokens.Count && lines1Tokens[x] == lines2Tokens[y])
                {
                    x++;
                    y++;
                }
                
                v[Index(k)] = x;

                if (x >= lines1Tokens.Count && y >= lines2Tokens.Count)
                {
                    trace.Add((int[])v.Clone());
                    return trace;
                }
            }
            
            trace.Add((int[])v.Clone());
        }
        
        return trace;
    }

    private List<Instruction> Traceback(List<int[]> trace)
    {
        List<Instruction> instructions = [];

        int x = _lines1.Count;
        int y = _lines2.Count;

        for (int d = trace.Count - 1; d >= 0; d--)
        {
            int[] v = trace[d];
            int k = x - y;

            int prevK;
            bool isDownwardMove = false;
            if (k == -d || k != d && v[Index(k - 1)] < v[Index(k + 1)])
            {
                isDownwardMove = true;
                prevK = k + 1;
            }
            else  // rightward move
            {
                prevK = k - 1;
            }
            
            int prevX = v[Index(prevK)];
            int prevY = prevX - prevK;

            while (x > prevX && y > prevY)  // diagonal moves
            {
                x--;
                y--;
            }

            if (d > 0)
            {
                instructions.Add(isDownwardMove
                    ? new InsertInstruction(y - 1, 1, _lines2[y - 1])
                    : new RemoveInstruction(x - 1, 1, _lines1[x - 1]));
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
        List<int> lines1Tokens = new List<int>();
        List<int> lines2Tokens = new List<int>();
        
        int nextToken = 0;
        foreach (var line in lines1)
        {
            if (!tokensDictionary.TryGetValue(line, out var token))
            {
                token = nextToken++;
                tokensDictionary[line] = token;
            }
            lines1Tokens.Add(token);
        }
        
        foreach (var line in lines2)
        {
            if (!tokensDictionary.TryGetValue(line, out var token))
            {
                token = nextToken++;
                tokensDictionary[line] = token;
            }
            lines2Tokens.Add(token);
        }
        
        return new Tuple<List<int>, List<int>>(lines1Tokens, lines2Tokens);
    }
}
