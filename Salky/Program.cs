using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;
using Microsoft.EntityFrameworkCore;
using Salky.Domain.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Salky.API;
using LiteDB;
using Salky.Domain.Models.GroupModels;
using System.Security.Cryptography;
using Salky.Domain.Models.UserModels;
using StackExchange.Redis;
using Salky.WebSocket.Infra.Models;
using Salky.App.Services;
using System.Diagnostics;

var tokenizer = new Tokenizer<string>("default")
    .Token("Comment", @"(?<=^([^""\r\n]|""[^""\r\n]*"")*)//[^\r\n]*")
    .Token("QuotedItem", @"""([^""]|""{2})+""")
    .Token("Partial", @"<<[a-zA-Z0-9_-]+>>")
    .Token("Variable", @"\$\([a-zA-Z0-9_-]+\)")
    .Token("LineContinuation", @"[\r\n]+[ \t\r\n]+[ \t]+")
    .Token("NewLine", @"[\r\n]+")
    .Token("Whitespace", @"[ \t]+")
    .Token("PartialAssignment", "::")
    .Token("Equals", "=")
    .Token("Dot", @"\.")
    .Token("OpenSquareBracket", @"\[")
    .Token("CloseSquareBracket", @"\]")
    .Token("OpenCurlyBracket", "{")
    .Token("CloseCurlyBracket", "}")
    .Token("Comma", ",")
    .Token("QuestionMark", @"\?")
    .Token("ExclaimationMark", @"\!")
    .Token("OpenRoundBracket", @"\(")
    .Token("CloseRoundBracket", @"\)")
    .Token("Colon", ":")
    .Token("Item", "[a-zA-Z0-9_-]+")
    .Token("WildCard", @"\*");

var r  = tokenizer.Tokenize("Olá teste \" . { } ");

var msgS = new MessagePartialContentService();

var sw = new Stopwatch();
sw.Start();
var n = 1000;
var str = "🧐 https://github.com/webtorrent/webtorrent/blob/master/lib/worker.js#L10  🧐 https://www.youtube.com/watch?v=hLqe9v7GDfI  as🧐 d qwe qwe q🧐w ";

var reresult = msgS.Generate(str);

var strff = "";
reresult.ForEach(x => strff += x.Content);
Console.WriteLine(str);
Console.WriteLine(strff);
Console.WriteLine(str.Equals(strff));
return;
Console.WriteLine("Default");
var t1 = Bench(() => msgS.Generate(str).ToArray(),sw,n);
Console.WriteLine($"per len {t1.TotalMilliseconds / (double)str.Length}ms");
Console.WriteLine("---");
str += str;
Console.WriteLine("*2");
var t2 = Bench(() => msgS.Generate(str).ToArray(), sw, n);
Console.WriteLine($"len {str.Length}");
Console.WriteLine($"INC {(t1 / t2)}");
Console.WriteLine($"per len {t2.TotalMilliseconds / (double)str.Length}ms");
Console.WriteLine("---");
str += str;
Console.WriteLine("*3");
var t3 = Bench(() => msgS.Generate(str).ToArray(), sw, n);
Console.WriteLine($"len {str.Length}");
Console.WriteLine($"INC {(t2 / t3)}");
Console.WriteLine($"per len {t3.TotalMilliseconds / (double)str.Length}ms");
Console.WriteLine("---");
str += str;
Console.WriteLine("*4");
var t4 = Bench(() => msgS.Generate(str).ToArray(), sw, n);
Console.WriteLine($"len {str.Length}");
Console.WriteLine($"INC {(t3 / t4)}");
Console.WriteLine($"per len {t4.TotalMilliseconds / (double)str.Length}ms");
Console.WriteLine("---");
return;

Console.WriteLine("Hello");

var conR =  ConnectionMultiplexer.Connect("localhost:6379");

var sub = conR.GetSubscriber();
var db = conR.GetDatabase();
var channel = "a";
var h1 = (RedisChannel channel, RedisValue value) =>
{
    Console.WriteLine($"In Handler 1 - From Channel : {channel} - Value : {value}");
};
var h2 = (RedisChannel channel, RedisValue value) =>
{
    Console.WriteLine($"In Handler 2 - From Channel : {channel} - Value : {value}");
};
await sub.SubscribeAsync(channel, h1);
await sub.SubscribeAsync(channel, h2);

await sub.PublishAsync(channel, "Msg1");
await sub.PublishAsync(channel, "Msg2");
await Task.Delay(200);
await sub.UnsubscribeAsync(channel, h1);
Console.WriteLine("Handler 1 out");
await sub.PublishAsync(channel, "Msg3");

while (true) ;

TimeSpan Bench(Action act, Stopwatch sw ,int n)
{
    sw.Restart();
    for (int i = 0; i < n; i++)
        act();
    sw.Stop();
    Console.WriteLine($"total:        {sw.Elapsed.TotalMilliseconds}ms");
    Console.WriteLine($"per operation:{(double)sw.Elapsed.TotalMilliseconds / (double)n}ms");
    return sw.Elapsed;
}












public readonly struct Token<TType>
{
    public Token(TType type, string value)
    {
        this.Type = type;
        this.Value = value;
    }

    public TType Type { get; }

    public string Value { get; }
}

public class Tokenizer<TType>
{
    private readonly IList<TokenType> tokenTypes = new List<TokenType>();
    private readonly TType defaultTokenType;

    public Tokenizer(TType defaultTokenType) => this.defaultTokenType = defaultTokenType;

    public Tokenizer<TType> Token(TType type, params string[] matchingRegexs)
    {
        foreach (var matchingRegex in matchingRegexs)
            this.tokenTypes.Add(new TokenType(type, matchingRegex));

        return this;
    }

    public IList<Token<TType>> Tokenize(string input)
    {
        IEnumerable<Token<TType>> tokens = new[] { new Token<TType>(this.defaultTokenType, input) };
        foreach (var type in this.tokenTypes)
            tokens = ExtractTokenType(tokens, type);
        return tokens.ToList();
    }

    private IEnumerable<Token<TType>> ExtractTokenType(
        IEnumerable<Token<TType>> tokens,
        TokenType toExtract)
    {
        var tokenType = toExtract.Type;
        var tokenMatcher = new Regex(toExtract.MatchingRegex, RegexOptions.Multiline);
        foreach (var token in tokens)
        {
            if (!token.Type.Equals(this.defaultTokenType))
            {
                yield return token;
                continue;
            }

            var matches = tokenMatcher.Matches(token.Value);
            if (matches.Count == 0)
            {
                yield return token;
                continue;
            }

            var currentIndex = 0;
            foreach (Match match in matches)
            {
                if (currentIndex < match.Index)
                {
                    yield return new Token<TType>(
                        this.defaultTokenType,
                        token.Value.Substring(currentIndex, match.Index - currentIndex));
                }
                yield return new Token<TType>(tokenType, match.Value);
                currentIndex = match.Index + match.Length;
            }

            if (currentIndex < token.Value.Length)
            {
                yield return new Token<TType>(
                    this.defaultTokenType,
                    token.Value.Substring(currentIndex, token.Value.Length - currentIndex));
            }   
        }
    }

    private readonly struct TokenType
    {
        public TokenType(TType type, string matchingRegex)
        {
            this.Type = type;
            this.MatchingRegex = matchingRegex;
        }

        public TType Type { get; }

        public string MatchingRegex { get; }
    }
}
