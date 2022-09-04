namespace TC;

public struct Op : IEquatable<Op>
{
    public static Op Unknown = new();
    public Symbol i { get; }
    public Generator g { get; }
    public Symbol j { get; }
    int hash { get; }
    public Op(Symbol i0, Generator g0)
    {
        i = i0;
        g = g0;
        j = Symbol.Unknown;
        hash = HashCode.Combine(i, g, j);
    }
    public Op(Generator g0, Symbol j0)
    {
        i = Symbol.Unknown;
        g = g0;
        j = j0;
        hash = HashCode.Combine(i, g, j);
    }
    public Op(Symbol i0, Generator g0, Symbol j0)
    {
        i = i0;
        g = g0;
        j = j0;
        hash = HashCode.Combine(i, g, j);
    }
    public override int GetHashCode() => hash;
    public bool Equals(Op other) => hash == other.hash;
    public Op Invert() => new(j, g.Invert(), i);
    public bool Contain(Symbol s) => i == s || j == s;
    public override string ToString() => $"({i})·{g}=({j})";
    public static implicit operator Op((int i, char g, int j) p) => new(p.i, p.g, p.j);
}
