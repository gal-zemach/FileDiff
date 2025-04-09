using FileDiff.Services.Data;

namespace FileDiff.Services.Algorithm;

public class MyersComparer
{
    private readonly List<string> _lines1;
    private readonly List<string> _lines2;

    private int N => _lines1.Count;
    private int M => _lines2.Count;
    private int MaxSteps => _lines1.Count + _lines2.Count;
    
    /// <summary>
    /// Used for negative array indices
    /// </summary>
    private int Index(int i) => MaxSteps + i;
    
    public MyersComparer(List<string> lines1, List<string> lines2)
    {
        _lines1 = lines1;
        _lines2 = lines2;
    }

    public List<RawInstruction> Compare()
    {
        List<int[]> trace = BuildMyersTrace();
        var instructions = Traceback(trace);
        
        return instructions;
    }

    private List<RawInstruction> Traceback(List<int[]> trace)
    {
        List<RawInstruction> instructions = [];

        int x = _lines1.Count;
        int y = _lines2.Count;

        for (int d = trace.Count - 1; d >= 0; d--)
        {
            int[] v = trace[d];
            int k = x - y;

            int prevK;
            bool isInsertMove = false;
            if (k == -d || k != d && v[Index(k - 1)] < v[Index(k + 1)])
            {
                prevK = k + 1;
                isInsertMove = true;
            }
            else
            {
                prevK = k - 1;
                // move == remove
            }
            
            int prevX = v[Index(prevK)];
            int prevY = prevX - prevK;

            while (x > prevX && y > prevY)
            {
                x--;
                y--;
                // instructions.Add(new RawInstruction(RawInstruction.Type.Match, x - 1, _lines1[x - 1]));
            }

            if (d > 0)
            {
                if (isInsertMove)
                {
                    // instructions.Add(new InsertInstruction(y - 1, 1, _lines2[y - 1]));
                    instructions.Add(new RawInstruction(RawInstruction.Type.Insert, y - 1, _lines2[y - 1]));
                }
                else
                {
                    // instructions.Add(new RemoveInstruction(x - 1, 1));
                    instructions.Add(new RawInstruction(RawInstruction.Type.Remove, x - 1, _lines1[x - 1]));
                }
            }
            
            x = prevX;
            y = prevY;
        }

        instructions.Reverse();
        return instructions;
    }
    
    private List<int[]> BuildMyersTrace()
    {
        var trace = new List<int[]>();
        
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
                while (x < _lines1.Count && y < _lines2.Count && _lines1[x] == _lines2[y])
                {
                    x++;
                    y++;
                }
                
                v[Index(k)] = x;

                if (x >= _lines1.Count && y >= _lines2.Count)
                {
                    trace.Add((int[])v.Clone());
                    return trace;
                }
            }
            
            trace.Add((int[])v.Clone());
        }
        
        return trace;
    }
}
