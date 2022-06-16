public struct PartialContent
{
    public string Content { get; }
    public TokenType Type { get; }
    public PartialContent(string value, TokenType type)
    {
        Content = value;
        Type = type;
    }
}
