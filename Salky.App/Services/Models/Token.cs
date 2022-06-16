using System.Text.RegularExpressions;

public struct Token
{
    public string PatternRegex { get; }
    public TokenType ResultType { get; }
    private Func<Match, string>? TranformMatch { get; }
    public Token(string PatternRegex, TokenType ResultType, Func<Match, string>? TranformMatch = null)
    {
        this.PatternRegex = PatternRegex;
        this.ResultType = ResultType;
        this.TranformMatch = TranformMatch;
    }
    public string ParseMatch(Match match) => TranformMatch == null ? match.Value : TranformMatch(match);
}
