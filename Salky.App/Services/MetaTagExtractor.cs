using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.App.Services
{
    public class MetaTagExtractor
    {
        public async Task<MetaTag?> Extract(Uri url)
        {
            var content = await FetchUrl(url);
            var doc = new HtmlDocument();
            doc.Load(content.ReadAsStream());
            var res = new MetaTag();
            res.Href = url.ToString();
            foreach (var tag in GetSiteMetaTags(doc))
            {
                switch (tag.key)
                {
                    case "og:url":
                        break;
                    case "og:image":
                        res.Picture = tag.value;
                        break;
                    case "og:title":
                        res.Title = tag.value;
                        break;
                    case "og:description":
                        res.Description = tag.value;
                        break;
                }
            }
            return res.IsMinialFilled() ? res : null;
        }
       

        private async Task<HttpContent> FetchUrl(Uri url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept-language", "pt-BR,pt;");
            return (await client.GetAsync(url)).Content;
        }

        private IEnumerable<KeyValue> GetSiteMetaTags(HtmlDocument HtmlDocument)
        {
             return HtmlDocument.DocumentNode
            .SelectNodes("//meta")
            .Select(node => new KeyValue(node.GetAttributeValue("property", ""), node.GetAttributeValue("content", ""))).
            Where(x => x.IsNotNullOrEmpty());
        }

    }
}
