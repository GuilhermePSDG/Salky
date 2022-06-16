using System.Text.RegularExpressions;

public class TokenResolver : ITokenResolver
{
    private readonly Token[] Tokens;

    public TokenResolver(Token[] Tokens)
    {
        this.Tokens = Tokens;
    }

    public List<PartialContent> Resolve(string entry)
    {
        return ResolveAsIEnumerable(entry).ToList();
    }
    public IEnumerable<PartialContent> ResolveAsIEnumerable(string entry)
    {
        foreach (var token in Tokens)
        {
            var match = Regex.Match(entry, token.PatternRegex);
            if (match.Success)
            {
                var before = entry[0..match.Index];
                if(!string.IsNullOrEmpty(before))
                foreach (var n in ResolveAsIEnumerable(before))
                    yield return n;
                yield return (new(token.ParseMatch(match), token.ResultType));
                var after = entry[(match.Index + match.Length)..];
                if(!string.IsNullOrEmpty(after))
                foreach (var n in ResolveAsIEnumerable(after))
                    yield return n;
                yield break;
            }
        }
        yield return new(entry, TokenType.UNKNOW);
    }

}

public interface ITokenResolver
{
    public IEnumerable<PartialContent> ResolveAsIEnumerable(string entry);
    public List<PartialContent> Resolve(string entry);
}