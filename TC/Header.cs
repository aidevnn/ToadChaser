using System.Collections;
namespace TC;

public class Header : IEnumerable<Generator>
{
    List<Generator> head { get; }
    public Header(IEnumerable<IEnumerable<Generator>> gens)
    {
        List<int> seps = new() { 0 };
        foreach (var g in gens)
            seps.Add(seps.Last() + g.Count());

        Separators = seps.ToArray();
        head = gens.SelectMany(g => g).ToList();
        Generators = head.Union(head.Select(g => g.Invert())).ToHashSet();
        Count = head.Count;
    }
    public Header(Header header)
    {
        head = header.head.ToList();
        Separators = header.Separators.ToArray();
        Generators = head.ToHashSet();
        Count = head.Count;
    }
    public int[] Separators { get; }
    public int Count { get; }
    public HashSet<Generator> Generators { get; }
    public Generator this[int k] => head[k];
    public IEnumerator<Generator> GetEnumerator() => head.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => head.GetEnumerator();
    public string Display(int digits)
    {
        var fmt = $"{{0,{digits + 1}}}";
        return " " + head.Glue("", fmt);
    }
    public override string ToString() => "  " + head.Glue(" ");
}
