namespace TC;

public struct OpKey : IEquatable<OpKey>, IComparable<OpKey>
{
    public Generator g { get; }
    public Symbol i { get; }
    int hash { get; }
    public OpKey(Symbol i0, Generator g0)
    {
        i = i0;
        g = g0;
        hash = HashCode.Combine(i, g);
    }

    public bool Equals(OpKey other) => i == other.i && g == other.g;
    public int CompareTo(OpKey other)
    {
        var compS = i.CompareTo(other.i);
        if (compS != 0)
            return compS;

        return g.CompareTo(other.g);
    }

    public override int GetHashCode() => hash;
    public override string ToString() => $"({i})·{g}";
}
