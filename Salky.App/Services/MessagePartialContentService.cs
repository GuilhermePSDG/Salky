using HtmlAgilityPack;
using Salky.App.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Salky.App.Services
{
    public class MessagePartialContentService
    {
        public TokenResolver TokenResolver { get; }
        public MessagePartialContentService()
        {
            TokenResolver = new TokenResolver(Tokens);
        }
        private static string urlReg = (@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)");
        private static string emojiReg = (@"(\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])");
        private static string ytRegex = (@"https:\/\/(www\.)?(youtube.com\/watch\?v=|youtu\.be\/)([A-z0-9_-]{11})");
        private static string gifRegex = (@"(https:\/\/media[0-9].giphy\.com\/media\/[A-Za-z0-9]{13,18}\/giphy\.gif)([^ ]+)?");
        private static Token[] Tokens = new Token[]
        {
            new Token(ytRegex,TokenType.URL_YOUTUBE_EMBED,(x) => $"https://www.youtube.com/embed/{x.Groups[3].Value}"),
            new Token(gifRegex,TokenType.URL_GIPHY, x => x.Groups[1].Value),
            new Token(urlReg,TokenType.URL),
            new Token(emojiReg,TokenType.EMOJI),
        };

        public async Task<List<PartialContent>> GenerateAsync(string Input) 
            => await Task.Run(() =>TokenResolver.Resolve(Input));
        public List<PartialContent> Generate(string Input) 
            => TokenResolver.Resolve(Input);
        public IEnumerable<PartialContent> GenerateAsIEnumerable(string Input) 
            => TokenResolver.ResolveAsIEnumerable(Input);
        public async Task<IEnumerable<PartialContent>> GenerateAsIEnumerableAsync(string Input) 
            => await Task.Run(() => TokenResolver.ResolveAsIEnumerable(Input));
       
    }
}