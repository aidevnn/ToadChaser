namespace TC;

public class Line
{
    Symbol[] row { get; }
    Header header { get; }
    public Line(Symbol key, Header head)
    {
        row = new Symbol[head.Count + 1];
        header = head;
        Key = key;

        foreach (var k in header.Separators)
            row[k] = key;
    }
    public Line(Line line)
    {
        header = line.header;
        row = line.row.Select(s => s).ToArray();
        Key = line.Key;
    }
    public int CountUnknown => row.Count(s => s == Symbol.Unknown);
    public bool IsComplete() => row.All(r => r != Symbol.Unknown);
    public void Subtitute(Symbol s0, Symbol s1)
    {
        if (s0 == Key)
            throw new Exception();

        for (int k = 0; k < row.Length; ++k)
        {
            if (row[k] == s0)
                row[k] = s1;
        }
    }
    public void ApplyOp(SortedDictionary<OpKey, Symbol> opsTable)
    {
        for (int k = 0; k < header.Count; ++k)
        {
            var i = row[k];
            var g = header[k];
            var j = row[k + 1];
            if (i == Symbol.Unknown && j == Symbol.Unknown)
                continue;
            else if (i != Symbol.Unknown && j != Symbol.Unknown)
            {
                var opKey = new OpKey(i, g);
                var opiKey = new OpKey(j, g.Invert());
                if (opsTable.ContainsKey(opKey) && opsTable[opKey] != j)
                    throw new Exception("TO DO");

                if (opsTable.ContainsKey(opiKey) && opsTable[opiKey] != i)
                    throw new Exception("TO DO");
            }
            else if (j == Symbol.Unknown)
            {
                var opKey = new OpKey(i, g);
                if (opsTable.ContainsKey(opKey))
                    row[k + 1] = opsTable[opKey];
            }
            else // if (i == Symbol.Unknown)
            {
                var opiKey = new OpKey(j, g.Invert());
                if (opsTable.ContainsKey(opiKey))
                    row[k] = opsTable[opiKey];
            }
        }
    }
    public void ApplyOp(Op op)
    {
        if (op.i == Symbol.Unknown || op.j == Symbol.Unknown || op.g == Generator.Unknown)
            return;

        for (int k = 0; k < header.Count; ++k)
        {
            var g = header[k];
            if (g != op.g)
                continue;

            var i = row[k];
            var j = row[k + 1];
            if (i == op.i)
            {
                if (j == Symbol.Unknown)
                    row[k + 1] = op.j;
                else if (j != op.j)
                    throw new Exception($"{op} ? {new Op(i, g, j)}");
            }
            else if (j == op.j)
            {
                if (i == Symbol.Unknown)
                    row[k] = op.i;
                else if (i != op.i)
                    throw new Exception($"{op} ? {new Op(i, g, j)}");
            }
        }
    }
    public IEnumerable<Op> GetOps() => header.Select((g, k) => new Op(row[k], g, row[k + 1]));
    public Symbol Key { get; }
    public string Display(int digits)
    {
        var fmt = $"{{0,{digits + 1}}}";
        var s0 = row.Glue("", fmt);
        var s1 = (s0 + " ").ToArray();
        foreach (var k in header.Separators)
            s1[(digits + 1) * k] = s1[(digits + 1) * (k + 1)] = '│';

        var s2 = s1.Glue();
        return s2;
    }
    public override string ToString()
    {
        var digits = row.Max(s => s.ToString().Length);
        return Display(digits);
    }
}
