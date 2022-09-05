namespace TC;

public class SubGroupTable
{
    Header header { get; }
    Line line { get; }
    public SubGroupTable(Header head)
    {
        header = head;
        line = new(Symbol.One, header);
    }
    public SubGroupTable(SubGroupTable sgtable)
    {
        header = sgtable.header;
        line = new(sgtable.line);
    }
    public int CountUnknown => line.CountUnknown;
    public bool IsComplete() => line.IsComplete();
    public void Subtitute(Symbol s0, Symbol s1) => line.Subtitute(s0, s1);
    public IEnumerable<Op> GetOps() => line.GetOps();
    public void ApplyOp(SortedDictionary<OpKey, Symbol> opsTable)
    {
        line.ApplyOp(opsTable);
    }
    public void ApplyOp(Op op)
    {
        var opi = op.Invert();
        line.ApplyOp(op);
        line.ApplyOp(opi);
    }

    public void Display(int digits)
    {
        Console.WriteLine("# SubGroup table");
        Console.WriteLine(header.Display(digits));
        var strLine = line.Display(digits);
        Console.WriteLine(Enumerable.Repeat('−', strLine.Length).Glue());
        Console.WriteLine(strLine);
        Console.WriteLine();
    }
}
