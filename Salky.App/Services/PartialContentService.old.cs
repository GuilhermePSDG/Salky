using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Salky.App.Services
{

    //public struct PartialContent
    //{
    //    public string Content { get;  }
    //    public RenderType RenderType { get; }

    //    public PartialContent(string content, RenderType RenderType)
    //    {
    //        this.RenderType = RenderType;
    //        Content = content;
    //    }
    //}

    //public enum RenderType
    //{
    //    any, text, url, emoji, youtubeEmbed, gif,url_details,localImage
    //}

    //public record Pattern(
    //Regex Regex,
    //RenderType TargetType,
    //RenderType ResultType,
    //Func<Match, string>? ReplaceMatchFactory = null
    //);
    //public class MetaResult {
    //    public string? SiteName{get;set;}
    //    public string? Title{get;set;} 
    //    public string? Description{get;set;}
    //    public string? Picture{get;set;}
    //    public string? Href { get; set; }
    //}
    
    //public static class PartialContentServiceOLD
    //{
    //    private struct KeyValue
    //    {
    //        public KeyValue(string key, string value)
    //        {
    //            this.key = key;
    //            this.value = value;
    //        }
    //        public string key { get; set; }
    //        public string value { get; set; }
    //        public bool IsNotNullOrEmpty()
    //        {
    //            return !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value);
    //        }
    //    }        
    //    public static MetaResult ResolveMetaTags(string value)
    //    {
    //        var metaTags = new List<KeyValue>();
    //        var r = new Regex(@"<head>([\s\S]+)<\/head>");
    //        value = r.Match(value).Groups[0].Value.Replace("\r\n","").Replace("\n","");
    //        var matchs = new Regex("<meta.+property=\"([^\"]+).*content=\"([^\"]+).+\\/>").IsMatch(value);
    //        //while (matchs.Success)
    //        //{
    //        //    metaTags.Add(new(matchs.Groups[1].Value, matchs.Groups[2].Value));
    //        //    matchs = matchs.NextMatch();
    //        //}
          
          
    //        var res = new MetaResult();

    //        foreach (var tag in metaTags)
    //        {
    //            switch (tag.key)
    //            {
    //                case "og:url":
    //                    break;
    //                case "og:image":
    //                    res.Picture = tag.value;
    //                    break;
    //                case "og:title":
    //                    res.Title = tag.value;
    //                    break;
    //                case "og:description":
    //                    res.Description = tag.value;
    //                    break;
    //            }
    //        }
    //        return res;
    //    }

    //    public static async Task<MetaResult?> ResolveMetaTags2(string url)
    //    {
    //        var client = new HttpClient();
    //        client.DefaultRequestHeaders.Add("accept-language", "pt-BR,pt;");
    //        HttpContent content = (await client.GetAsync(url)).Content;
    //        var doc = new HtmlDocument();
    //        doc.Load(content.ReadAsStream());
    //        var metaTags = doc
    //        .DocumentNode
    //        .SelectNodes("//meta")
    //        .Select(node => new KeyValue(node.GetAttributeValue("property", ""), node.GetAttributeValue("content", ""))).
    //        Where(x => x.IsNotNullOrEmpty()).ToList();

    //        var res = new MetaResult();
    //        foreach (var tag in metaTags)
    //        {
    //            switch (tag.key)
    //            {
    //                case "og:url":
    //                    break;
    //                case "og:image":
    //                    res.Picture = tag.value;
    //                    break;
    //                case "og:title":
    //                    res.Title = tag.value;
    //                    break;
    //                case "og:description":
    //                    res.Description = tag.value;
    //                    break;
    //            }
    //        }
    //        return res;
    //    }

    //    public static Regex urlReg = new Regex(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)");
    //    public static Regex emojiReg = new Regex(@"(\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])");
    //    public static Regex ytRegex = new Regex(@"https:\/\/(www\.)?(youtube.com\/watch\?v=|youtu\.be\/)([A-z0-9_-]{11})");
    //    public static Regex gifRegex = new Regex(@"(https:\/\/media[0-9].giphy\.com\/media\/[A-Za-z0-9]{13,18}\/giphy\.gif)([^ ]+)?");
    //    public static Regex metaTagRegex = new Regex("<meta.*property=\"([^\"]+).*content=\"([^\"]+).*\\/>");
    //    public static Regex locaImageRegex = new Regex($@"^({ImageServiceConfig.FolderName}\/.{36,37})$");

    //    public static List<Pattern> patterns  = new List<Pattern>()
    //    {
    //        new(urlReg, RenderType.text, RenderType.url),
    //        new(emojiReg, RenderType.text, RenderType.emoji),
    //        new(ytRegex, RenderType.url, RenderType.youtubeEmbed, (x) => $"https://www.youtube.com/embed/{x.Groups[3].Value}"),
    //        new(gifRegex, RenderType.url, RenderType.gif, x => x.Groups[1].Value),
    //        new(locaImageRegex, RenderType.any, RenderType.localImage)
    //    };

    //    public static List<PartialContent> CreateEmbeds(string value)
    //    {
    //        var classified = new List<PartialContent>(value.Length / 10) { new PartialContent(value, RenderType.text) };
    //        for (int i = 0; i < patterns.Count; i++)
    //        {
    //            for (int q = 0; q < classified.Count; q++)
    //            {
    //                PartialContent item = classified[q];
    //                if (item.RenderType == RenderType.any || item.RenderType == patterns[i].TargetType)
    //                {
    //                    var matchColect = patterns[i].Regex.Matches(item.Content);
    //                    if (matchColect.Count > 0)
    //                    {
    //                        classified.RemoveAt(q);
    //                        var matchsString = matchColect.Select(x => x.Value).ToArray();
    //                        var splited = item.Content.Split(matchsString, StringSplitOptions.None);
    //                        for (int w = 0; w < splited.Length; w++)
    //                        {
    //                            if (splited[w] != "")
    //                            {
    //                                classified.Insert(q, new PartialContent(splited[w], item.RenderType));
    //                                q++;
    //                            }
    //                            if (w != splited.Length - 1)
    //                            {
    //                                classified.Insert(q, new PartialContent(patterns[i].ReplaceMatchFactory == null ? matchsString[w] : patterns[i].ReplaceMatchFactory(matchColect[w]), patterns[i].ResultType));
    //                                q++;
    //                            }
    //                        }
    //                    }

    //                }
    //            }
    //        }
    //        return classified;
    //    }

    //}
}
