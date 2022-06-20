using Salky.App.Services;

public struct PartialContent
{
    public object Content { get;private set; }
    public TokenType Type { get; private set;}

  
    public PartialContent(object value, TokenType type)
    {
        Content = value;
        Type = type;
    }

}

