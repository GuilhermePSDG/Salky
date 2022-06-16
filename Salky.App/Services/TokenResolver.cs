using System.Text.RegularExpressions;

public class TokenResolver
{
    private readonly Token[] Tokens;

    public TokenResolver(Token[] Tokens)
    {
        this.Tokens = Tokens;
    }

    public List<PartialContent> Resolve(string entry)
    {
        if (string.IsNullOrEmpty(entry)) return new(0);
        foreach (var token in Tokens)
        {
            var match = Regex.Match(entry, token.PatternRegex);
            if (match.Success)
            {
                var res = (Resolve(entry[0..match.Index]));
                res.Add(new(token.ParseMatch(match), token.ResultType));
                res.AddRange(Resolve(entry[(match.Index + match.Length)..]));
                return res;
            }
        }
        return new() { new(entry, TokenType.UNKNOW) };
    }
    public IEnumerable<PartialContent> ResolveAsIEnumerable(string entry)
    {
        if (string.IsNullOrEmpty(entry)) yield break;
        foreach (var token in Tokens)
        {
            var match = Regex.Match(entry, token.PatternRegex);
            if (match.Success)
            {
                foreach (var n in ResolveAsIEnumerable(entry[0..match.Index]))
                    yield return n;
                yield return (new(token.ParseMatch(match), token.ResultType));
                foreach (var n in ResolveAsIEnumerable(entry[(match.Index + match.Length)..]))
                    yield return n;
                yield break;
            }
        }
        yield return new(entry, TokenType.UNKNOW);
    }

}
