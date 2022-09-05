namespace TC;

public class RelatorsTable
{
    Dictionary<Symbol, Line> table { get; }
    Header header { get; }
    public RelatorsTable(Header head)
    {
        table = new();
        header = head;
        table[Symbol.One] = NewLine(Symbol.One);
    }
    public RelatorsTable(RelatorsTable rTable)
    {
        header = rTable.header;
        table = rTable.table.ToDictionary(s => s.Key, l => new Line(l.Value));
    }
    Line NewLine(Symbol s) => new(s, header);
    public IEnumerable<Op> GetOps() => table.SelectMany(kv => kv.Value.GetOps());
    public int CountUnknown => table.Sum(r => r.Value.CountUnknown);
    public void ApplyOp(SortedDictionary<OpKey, Symbol> opsTable)
    {
        var symbols = opsTable.Values.Distinct().Ascending();
        foreach (var s in symbols)
        {
            if (!table.ContainsKey(s))
                table[s] = NewLine(s);
        }

        foreach (var kv in table)
            kv.Value.ApplyOp(opsTable);
    }
    public void ApplyOp(Op op)
    {
        var opi = op.Invert();
        foreach (var kv in table)
        {
            kv.Value.ApplyOp(op);
            kv.Value.ApplyOp(opi);
        }
    }
    public void Display(int digits)
    {
        Console.WriteLine("# Relators table");
        header.DisplayHead(digits);
        int k = 0;
        foreach (var kv in table.OrderBy(p => p.Key))
        {
            if (k > 0 && k % 40 == 0) header.ReDisplayHead(digits);
            Console.WriteLine(kv.Value.Display(digits));
            ++k;
        }
        header.DisplayLineUp(digits);
        Console.WriteLine();
    }
}
