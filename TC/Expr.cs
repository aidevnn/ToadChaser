using System.Text.RegularExpressions;
namespace TC;

public static class Expr
{
    static bool AreInvert(char c0, char c1)
    {
        var d = c0 - c1;
        return d == 32 || d == -32;
    }
    static char Revert(char c) => char.IsLower(c) ? char.ToUpper(c) : char.ToLower(c);
    static IEnumerable<char> Invert(IEnumerable<char> ic) => ic.Select(Revert).Reverse();
    static string Reduce(string word)
    {
        Stack<char> stack = new Stack<char>(30);
        foreach (var c in word)
        {
            if (stack.Count == 0)
            {
                stack.Push(c);
                continue;
            }
            else if (AreInvert(c, stack.Peek()))
                stack.Pop();
            else
                stack.Push(c);
        }

        return new String(stack.Reverse().ToArray());
    }
    static Regex regX = new Regex(@"([a-zA-Z])((\-{1}\d{1,})|(\d{0,}))");
    static string ParseReducedWord(string word)
    {
        var word0 = "";
        foreach (Match m in regX.Matches(word))
        {
            var powStr = m.Groups[2].Value;
            var c = char.Parse(m.Groups[1].Value);
            var p = string.IsNullOrEmpty(powStr) ? 1 : int.Parse(powStr);
            word0 += p > 0 ? Enumerable.Repeat(c, p).Glue() : Enumerable.Repeat(Revert(c), -p).Glue();
        }

        return Reduce(word0).Glue();
    }
    public static string ExpandRelator(this string relation)
    {
        string word = "";
        var split = relation.Split('=');
        if (split.Length == 1)
            word = ParseReducedWord(relation);
        else if (split.Length == 2)
        {
            var w0 = ParseReducedWord(split[0]);
            var w1 = ParseReducedWord(split[1]);
            word = w0.Concat(Invert(w1)).Glue();
        }
        else
            throw new Exception();

        return word;
    }
}
