using System.Diagnostics;
namespace TC;

public static class TableOpsExt
{
    static Stopwatch sw = Stopwatch.StartNew();

    public static string Relator(string relation)
    {
        var word = Word.Empty;
        var split = relation.Split('=');
        if (split.Length == 1)
            word = new(relation);
        else if (split.Length == 2)
        {
            var w0 = new Word(split[0]);
            var w1 = new Word(split[1]);
            word = new(w0.ToString() + w1.Invert().ToString());
        }
        else
            throw new Exception();

        return word.extStr;
    }
    public static TableOps ApplyOp(this TableOps tOps, Op op)
    {
        var tOps0 = new TableOps(tOps);
        var opi = op.Invert();
        tOps0.sgTable.ApplyOp(op);
        tOps0.sgTable.ApplyOp(opi);
        tOps0.rTable.ApplyOp(op);
        tOps0.rTable.ApplyOp(opi);
        tOps0.BuildTable();
        return tOps0;
    }
    public static void ToddCoxeter(this TableOps tableOps)
    {
        sw.Restart();
        var tOps = new TableOps(tableOps);
        tOps.BuildTable();
        Console.WriteLine($"#### Step 0 ####");
        tOps.Display();

        int k = 1;
        while (true)
        {
            var op = tOps.NewOp();
            if (op.i == Symbol.Unknown)
                break;

            Console.WriteLine($"#### Step {k++} Op : {op} ####");
            var opi = op.Invert();
            tOps = new(tOps);
            tOps.sgTable.ApplyOp(op);
            tOps.sgTable.ApplyOp(opi);
            tOps.rTable.ApplyOp(op);
            tOps.rTable.ApplyOp(opi);
            tOps.BuildTable();
            tOps.Display();
        }

        Console.WriteLine($"####     End    ####");
        Console.WriteLine($"Time : {sw.ElapsedMilliseconds} ms");
    }
}
