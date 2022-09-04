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
    public void ApplyOp(Op op)
    {
        var containsOPi = table.ContainsKey(op.i);
        var containsOPj = table.ContainsKey(op.j);
        if (!containsOPi && !containsOPj)
            throw new Exception($"op");

        if (!containsOPi)
            table[op.i] = NewLine(op.i);

        if (!containsOPj)
            table[op.j] = NewLine(op.j);

        foreach (var kv in table)
            kv.Value.ApplyOp(op);
    }
    public void Display(int digits)
    {
        Console.WriteLine("# Relators table");
        Console.WriteLine(header.Display(digits));
        var strLine = table.First().Value.Display(digits);
        Console.WriteLine(Enumerable.Repeat('_', strLine.Length).Glue());
        foreach (var kv in table.OrderBy(p => p.Key))
            Console.WriteLine(kv.Value.Display(digits));

        Console.WriteLine();
    }
}
