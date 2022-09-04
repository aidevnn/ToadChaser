namespace TC;

public class TableOps
{
    Header sgHeader { get; }
    Header relHeader { get; }
    Dictionary<Generator, Dictionary<Symbol, Symbol>> table { get; }
    public TableOps(Header sgheader, Header relheader)
    {
        if (!sgheader.Generators.IsSubsetOf(relheader.Generators))
            throw new Exception();

        sgHeader = new(sgheader);
        relHeader = new(relheader);
        sgTable = new(sgHeader);
        rTable = new(relHeader);
        table = new();
    }
    public SubGroupTable sgTable { get; }
    public RelatorsTable rTable { get; }
    public TableOps? Previous { get; }
    void FillTableOps(IEnumerable<Op> ops)
    {
        foreach (var gr in ops.GroupBy(e => e.g).OrderBy(e => e.Key))
        {
            var g = gr.Key;
            var gi = g.Invert();
            if (!table.ContainsKey(g))
                table[g] = new Dictionary<Symbol, Symbol>();

            if (!table.ContainsKey(gi))
                table[gi] = new Dictionary<Symbol, Symbol>();

            var col = table[g];
            var coli = table[gi];
            foreach (var op in gr.Where(e => e.i != Symbol.Unknown && e.j != Symbol.Unknown))
            {
                if (col.ContainsKey(op.i))
                {
                    if (col[op.i] != op.j)
                    {
                        Display();
                        throw new Exception($"{op} ~ {new Op(op.i, g, col[op.i])}");
                    }
                }
                else
                    col[op.i] = op.j;

                if (coli.ContainsKey(op.j))
                {
                    if (coli[op.j] != op.i)
                    {
                        Display();
                        throw new Exception($"{op} ~ {new Op(coli[op.j], gi, op.i)}");
                    }
                }
                else
                    coli[op.j] = op.i;
            }
        }
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

        table = tableOps.table.ToDictionary(a => a.Key, b => b.Value.ToDictionary(c => c.Key, c => c.Value));
    }
    public void BuildTable()
    {
        int sz = 0;
        while (sz != sgTable.CountUnknown + rTable.CountUnknown)
        {
            var allOps = sgTable.GetOps().Concat(rTable.GetOps());
            FillTableOps(allOps);
            var nOps = table.SelectMany(kv => kv.Value.Select(p => new Op(p.Key, kv.Key, p.Value)));
            sz = sgTable.CountUnknown + rTable.CountUnknown;
            foreach (var op in nOps)
            {
                sgTable.ApplyOp(op);
                rTable.ApplyOp(op);
            }
        }
    }
    public Op NewOp()
    {
        var j = table.SelectMany(e => e.Value.SelectMany(f => new[] { f.Key, f.Value })).Descending().FirstOrDefault().Next;

        var sgOp = sgTable.GetOps().FirstOrDefault(op => op.i != Symbol.Unknown && op.g != Generator.Unknown && op.j == Symbol.Unknown);
        if (sgOp.i != Symbol.Unknown && sgOp.g != Generator.Unknown && sgOp.j == Symbol.Unknown)
            return sgOp.g.sgn == 1 ? new Op(sgOp.i, sgOp.g, j) : new Op(sgOp.i, sgOp.g, j).Invert();

        var rOp = rTable.GetOps().FirstOrDefault(op => op.i != Symbol.Unknown && op.g != Generator.Unknown && op.j == Symbol.Unknown);
        if (rOp.i != Symbol.Unknown && rOp.g != Generator.Unknown && rOp.j == Symbol.Unknown)
            return rOp.g.sgn == 1 ? new Op(rOp.i, rOp.g, j) : new Op(rOp.i, rOp.g, j).Invert();

        return new();
    }

    string TableFind(Symbol i, Generator g)
    {
        var col = table[g];
        if (col.ContainsKey(i))
            return col[i].ToString();

        return " ";
    }
    public void DisplayTable(int digits)
    {
        var fmt = $"{{0,{digits + 1}}}";
        Console.WriteLine("# Classes table");
        var gens = table.Keys.Ascending();
        var symbs = table.SelectMany(kv => kv.Value.Keys).Distinct().Ascending();
        var head = string.Format(fmt, "") + "|" + gens.Glue("|", fmt) + "|";
        var line = Enumerable.Range(0, head.Length).Select(i => i % (digits + 2) == digits + 1 ? '|' : '−').Glue();
        var rows = new List<string>();
        foreach (var i in symbs)
        {
            var r = gens.Select(g => TableFind(i, g)).Prepend(i.ToString()).Glue("|", fmt);
            rows.Add(r);
        }

        Console.WriteLine(head);
        Console.WriteLine(line);
        rows.ForEach(r => Console.WriteLine(r + "|"));
        Console.WriteLine();
    }
    public void Display()
    {
        var digits = table.Count == 0 ? 2 : table.Max(p0 => p0.Value.Count == 0 ? 2 : p0.Value.Max(p1 => p1.Key.ToString().Length));
        sgTable.Display(digits);
        rTable.Display(digits);
        DisplayTable(digits);
        Console.WriteLine();
    }
    public void BuildWordGroup()
    {
        var keys = table.SelectMany(a0 => a0.Value.SelectMany(a1 => new[] { a1.Key, a1.Value })).Distinct().Ascending();
        var allOps = sgTable.GetOps().Concat(rTable.GetOps()).Where(op => op.i != Symbol.Unknown && op.g != Generator.Unknown && op.j != Symbol.Unknown);

    }
    public static Header CreateHeader(params string[] gens)
    {
        var head = gens.Select(w => new Word(w).extStr).OrderBy(w => w.Length).ThenBy(w => w).Select(w => w.Select(c => new Generator(c)));
        return new Header(head);
    }
}
