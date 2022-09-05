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
        Console.WriteLine(header.Display(digits));
        var strLine = table.First().Value.Display(digits);
        var s1 = Enumerable.Repeat('─', strLine.Length).Glue().ToArray();
        foreach (var k in header.Separators)
            s1[(digits + 1) * k] = s1[(digits + 1) * (k + 1)] = '┬';

        s1[0] = '┌';
        s1[s1.Length - 1] = '┐';
        Console.WriteLine(s1.Glue());
        foreach (var kv in table.OrderBy(p => p.Key))
            Console.WriteLine(kv.Value.Display(digits));

        Console.WriteLine();
    }
}
