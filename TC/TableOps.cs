using System.Diagnostics.CodeAnalysis;

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
            else if (opiCheck && opjCheck)
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
    public void BuildTable()
    {
        int sz = 0;
        HashSet<Op> newOps = new();
        (Symbol, Symbol) err = new();
        do
        {
            newOps.Clear();
            sz = sgTable.CountUnknown + rTable.CountUnknown;
            err = sgTable.ApplyOp(opsTable, newOps);
            if (Substitute(err.Item1, err.Item2))
                foreach (var op in newOps)
                    ApplyOp(op);

            newOps.Clear();
            err = rTable.ApplyOp(opsTable, newOps);
            if (Substitute(err.Item1, err.Item2))
                foreach (var op in newOps)
                    ApplyOp(op);

        } while (newOps.Count != 0 || sz != sgTable.CountUnknown + rTable.CountUnknown);
    }
    bool Substitute(Symbol s0, Symbol s1)
    {
        if (s0 == Symbol.Unknown)
            return true;

        opsTable.Clear();

        sgTable.Subtitute(s0, s1);
        rTable.SubtituteRemove(s0, s1);

        while (rTable.ContainsKey(s1.Next))
        {
            s0 = s1;
            s1 = s0.Next;
            sgTable.Subtitute(s0, s1);
            rTable.SubtituteWithKey(s0, s1);
        }

        return false;
    }
    public void ApplyOp(Op op)
    {
        var opKey = new OpKey(op.i, op.g);
        var opiKey = new OpKey(op.j, op.g.Invert());
        if (!opsTable.ContainsKey(opKey) && !opsTable.ContainsKey(opiKey))
        {
            opsTable[opKey] = op.j;
            opsTable[opiKey] = op.i;
        }
        else
        {
            if (opsTable.ContainsKey(opKey))
            {
                var (s0, s1) = Symbol.MinMax(op.j, opsTable[opKey]);
                Substitute(s0, s1);
            }
            if (opsTable.ContainsKey(opiKey))
            {
                var (s0, s1) = Symbol.MinMax(op.i, opsTable[opiKey]);
                Substitute(s0, s1);
            }
        }
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

        var head = string.Format(fmt, "") + "???" + gens.Glue("???", fmt) + "???";
        var lineTop = Enumerable.Range(0, head.Length - (digits + 2)).Select(i => i % (digits + 2) == digits + 1 ? (i == head.Length - digits - 3 ? '???' : '???') : '???').Glue();
        var lineMid = Enumerable.Range(0, head.Length).Select(i => i % (digits + 2) == digits + 1 ? (i == head.Length - 1 ? '???' : '???') : '???').Glue();
        var lineEnd = Enumerable.Range(0, head.Length).Select(i => i % (digits + 2) == digits + 1 ? (i == head.Length - 1 ? '???' : '???') : '???').Glue();

        var rows = new List<string>();
        foreach (var i in symbs)
        {
            var r = gens.Select(g => TableFind(new(i, g))).Prepend(i.ToString()).Glue("???", fmt);
            rows.Add(r.Glue());
        }

        Console.WriteLine(" " + string.Format(fmt, "") + "???" + lineTop);
        Console.WriteLine(" " + head);
        Console.WriteLine("???" + lineMid);
        rows.ForEach(r => Console.WriteLine("???" + r + "???"));
        Console.WriteLine("???" + lineEnd);
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
