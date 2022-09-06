using System.Diagnostics;
namespace TC;

public static class TableOpsExt
{
    static Stopwatch sw = Stopwatch.StartNew();

    public static TableOps ToddCoxeter(string sg, string rels)
    {
        var sg0 = TableOps.CreateHeader(sg.Split(',', StringSplitOptions.TrimEntries));
        var rels0 = TableOps.CreateHeader(rels.Split(',', StringSplitOptions.TrimEntries));
        var tops = new TableOps(sg0, rels0);
        tops.ToddCoxeter();
        return tops;
    }

    public static void ToddCoxeter(this TableOps tableOps)
    {
        sw.Restart();
        var tOps = new TableOps(tableOps);
        tOps.BuildTable();
        int k = 1;
        var fop = tOps.FirstOp();
        Console.WriteLine($"#### Step {k++} Op : {fop} ####");

        tOps.Display();

        while (true)
        {
            var op = tOps.NewOp();
            if (op.i == Symbol.Unknown)
                break;

            Console.WriteLine($"#### Step {k++} Op : {op} ####");

            tOps = new(tOps);
            tOps.ApplyOp(op);
            tOps.BuildTable();
            tOps.Display();
        }

        Console.WriteLine($"####     End    ####");
        Console.WriteLine($"Time : {sw.ElapsedMilliseconds} ms");
    }
}
