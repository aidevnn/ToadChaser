using System.Collections;

namespace TC;

public struct Word : IEnumerable<Letter>, IEquatable<Word>, IComparable<Word>
{
    public static int count { get; private set; }
    public static void ResetCounter() => count = 0;
    public static Word Empty => new Word();
    IEnumerable<Letter> letters;
    public Word()
    {
        letters = Enumerable.Empty<Letter>();
        weight = length = 0;
        extStr = "";
    }
    public Word(IEnumerable<Letter> l)
    {
        letters = l;
        length = letters.Count();
        weight = letters.Sum(l => l.weight);
        extStr = new String(letters.SelectMany(l => l.Extend()).ToArray());
        ++Word.count;
    }
    public Word(string word)
    {
        letters = word.ParseReducedWord();
        length = letters.Count();
        weight = letters.Sum(l => l.weight);
        extStr = new String(letters.SelectMany(l => l.Extend()).ToArray());
        ++Word.count;
    }
    public Word Add(Word w) => new Word(this.Concat(w).Reduce());
    public Word Invert() => new Word(letters.Reverse().Select(l => l.Invert()));
    public int length { get; }
    public int weight { get; }
    public string extStr { get; }
    public string extStr2 => length == 0 ? "()" : extStr;

    public IEnumerator<Letter> GetEnumerator() => letters.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => letters.GetEnumerator();

    public bool Equals(Word other) => string.Equals(extStr, other.extStr);

    public int CompareTo(Word other)
    {
        var compL = length.CompareTo(other.length);
        if (compL != 0)
            return compL;

        var compW = weight.CompareTo(other.weight);
        if (compW != 0)
            return compW;

        var compP = this.Count(l => l.pow < 0).CompareTo(other.Count(l => l.pow < 0));
        if (compP != 0)
            return compP;

        return this.SequenceCompare(other);
    }

    public override string ToString() => length == 0 ? "()" : letters.Glue();
    public override int GetHashCode() => extStr.GetHashCode();

}
