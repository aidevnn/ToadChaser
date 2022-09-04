using System.Text.RegularExpressions;
namespace TC;

public static class LetterExt
{
    public static Letter From(char c, int p)
    {
        var c0 = char.IsLower(c) ? c : char.ToLower(c);
        var p0 = c == c0 ? p : -p;
        return (c0, p0);
    }
    static Regex regX1 = new Regex(@"([a-zA-Z])\1*");
    static Regex regX2 = new Regex(@"([a-zA-Z])((\-{1}\d{1,})|(\d{0,}))");
    public static IEnumerable<Letter> ReduceRegEx(this IEnumerable<Letter> letters)
    {
        var word = string.Join("", letters.SelectMany(l => l.Extend())).Reduce();
        foreach (Match m in regX1.Matches(word))
        {
            var w = m.Value;
            yield return From(w[0], w.Length);
        }
    }
    public static IEnumerable<Letter> ParseReducedWord(this string word)
    {
        List<Letter> letters = new();
        foreach (Match m in regX2.Matches(word))
        {
            var powStr = m.Groups[2].Value;
            var c = char.Parse(m.Groups[1].Value);
            var p = string.IsNullOrEmpty(powStr) ? 1 : int.Parse(powStr);
            letters.Add(From(c, p));
        }

        return letters.ReduceRegEx();
    }
    public static IEnumerable<Letter> Reduce(this IEnumerable<Letter> letters)
    {
        Stack<Letter> stack = new();
        foreach (var l in letters)
        {
            if (stack.Count == 0)
            {
                stack.Push(l);
                continue;
            }
            else if (l.c == stack.Peek().c)
            {
                var l0 = stack.Pop();
                var p = l.pow + l0.pow;
                if (p != 0)
                    stack.Push((l.c, p));
            }
            else
                stack.Push(l);
        }

        return stack.Reverse();
    }
    public static IEnumerable<Letter> ParseExtendedWord(this string word) => word.Select(c => From(c, 1)).Reduce();
}
