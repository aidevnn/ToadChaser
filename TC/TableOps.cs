namespace TC;

public class TableOps
{
    Header sgHeader { get; }
    Header relHeader { get; }
    SortedDictionary<OpKey, Symbol> opsTable { get; }
    public TableOps(Header sgheader, Header relheader)
    {
        if (!sgheader.Generators.IsSubsetOf(relheader.Generators))
            throw new Exception();

        sgHeader = new(sgheader);
        relHeader = new(relheader);
        sgTable = new(sgHeader);
        rTable = new(relHeader);
        opsTable = new();
    }
    public TableOps(TableOps tableOps)
    {
        if (tableOps is null)
            throw new Exception();

        Previous = tableOps;
        sgHeader = tableOps.sgHeader;
        relHeader = tableOps.relHeader;
        sgTable = new(tableOps.sgTable);
        rTable = new(tableOps.rTable);
        opsTable = new(tableOps.opsTable);
    }
    public SubGroupTable sgTable { get; }
    public RelatorsTable rTable { get; }
    public TableOps? Previous { get; }
    void FillClassesTable(IEnumerable<Op> ops)
    {
        foreach (var op in ops)
        {
            if (op.i == Symbol.Unknown || op.j == Symbol.Unknown)
                continue;

            var opiKey = new OpKey(op.i, op.g);
            var opjKey = new OpKey(op.j, op.g.Invert());

            var opiCheck = opsTable.ContainsKey(opiKey);
            var opjCheck = opsTable.ContainsKey(opjKey);

            if (!opiCheck && !opjCheck)
            {
                opsTable[opiKey] = op.j;
                opsTable[opjKey] = op.i;
            }
            else
            {
                if (opiCheck && opjCheck)
                {
                    List<string> err = new();
                    if (opsTable[opiKey] != op.j)
                        err.Add($"{opiKey}={opsTable[opiKey]} ~ {opiKey}={op.j}");

                    if (opsTable[opjKey] != op.i)
                        err.Add($"{opjKey}={opsTable[opjKey]} ~ {opjKey}={op.i}");

                    if (err.Count != 0)
                        throw new Exception(err.Glue(" and ", "[{0}]"));
                }
                else
                    throw new Exception("TO DO");
            }
        }
    }
    public void BuildTable()
    {
        int sz = 0;
        do
        {
            FillClassesTable(sgTable.GetOps());
            FillClassesTable(rTable.GetOps());
            sz = sgTable.CountUnknown + rTable.CountUnknown;
            sgTable.ApplyOp(opsTable);
            rTable.ApplyOp(opsTable);
        } while (sz != sgTable.CountUnknown + rTable.CountUnknown);
    }
    public void ApplyOp(Op op)
    {
        sgTable.ApplyOp(op);
        rTable.ApplyOp(op);
    }
    public Op FirstOp()
    {
        if (opsTable.Count == 0)
            return new();

        var fop = opsTable.First();
        return new(fop.Key.i, fop.Key.g, fop.Value);
    }
    public Op NewOp()
    {
        var j = opsTable.SelectMany(e => new[] { e.Key.i, e.Value }).Descending().FirstOrDefault().Next;

        var sgOp = sgTable.GetOps().FirstOrDefault(op => op.i != Symbol.Unknown && op.g != Generator.Unknown && op.j == Symbol.Unknown);
        if (sgOp.i != Symbol.Unknown && sgOp.g != Generator.Unknown && sgOp.j == Symbol.Unknown)
            return new Op(sgOp.i, sgOp.g, j);

        var rOp = rTable.GetOps().FirstOrDefault(op => op.i != Symbol.Unknown && op.g != Generator.Unknown && op.j == Symbol.Unknown);
        if (rOp.i != Symbol.Unknown && rOp.g != Generator.Unknown && rOp.j == Symbol.Unknown)
            return new Op(rOp.i, rOp.g, j);

        return new();
    }
    string TableFind(OpKey opk)
    {
        if (opsTable.ContainsKey(opk))
            return opsTable[opk].ToString();

        return " ";
    }
    public void DisplayTable(int digits)
    {
        var fmt = $"{{0,{digits + 1}}}";
        Console.WriteLine("# Classes table");
        var gens = opsTable.Keys.Select(k => k.g).Distinct().Ascending();
        var symbs = opsTable.Keys.Select(k => k.i).Distinct().Ascending();

        var head = string.Format(fmt, "") + "│" + gens.Glue("│", fmt) + "│";
        var line = Enumerable.Range(0, head.Length).Select(i => i % (digits + 2) == digits + 1 ? (i == head.Length - 1 ? '┤' : '┼') : '─').Glue();

        var rows = new List<string>();
        foreach (var i in symbs)
        {
            var r = gens.Select(g => TableFind(new(i, g))).Prepend(i.ToString()).Glue("│", fmt);
            rows.Add(r);
        }

        Console.WriteLine(head);
        Console.WriteLine(line);
        rows.ForEach(r => Console.WriteLine(r + "│"));
        Console.WriteLine();
    }
    public void Display()
    {
        var digits = opsTable.Count == 0 ? 2 : opsTable.Max(p0 => Symbol.Max(p0.Key.i, p0.Value).ToString().Length);
        sgTable.Display(digits);
        rTable.Display(digits);
        DisplayTable(digits);
        Console.WriteLine();
    }
    public static Header CreateHeader(params string[] gens)
    {
        var head = gens.Select(Expr.ExpandRelator).OrderBy(w => w.Length).ThenBy(w => w).Select(w => w.Select(c => new Generator(c)));
        return new Header(head);
    }
}
