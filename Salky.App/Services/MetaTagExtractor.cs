using HtmlAgilityPack;

namespace Salky.App.Services
{
    public class MetaTagExtractor
    {
        public MetaTagExtractor()
        {
            throw new Exception("Low performance");
        }
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
        private IEnumerable<KeyValue> GetSiteMetaTags(HtmlDocument HtmlDocument)
        {
            return HtmlDocument.DocumentNode
           .SelectNodes("//meta")
           .Select(node => new KeyValue(node.GetAttributeValue("property", ""), node.GetAttributeValue("content", ""))).
           Where(x => x.IsNotNullOrEmpty());
        }
        private async Task<HttpContent> FetchUrl(Uri url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept-Language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
            return (await client.GetAsync(url)).Content;
        }
       


        private List<string> GetAllMetaTags(Stream stream)
        {
            var results = new List<string>();

            var init = "<meta".ToCharArray();
            var end = ">".ToCharArray();
            var ilegal = "</head>".ToCharArray();
            List<char>? temp;
            while (true)
            {
                temp = Next(stream, init, ilegal);
                if (temp == null) break;
                temp = Next(stream, end, ilegal);
                if (temp == null) break;
                results.Add($"<meta{new string(temp.ToArray())}>");
            }
            return results;
        }
        private List<char>? Next(Stream stream, char[] TargetMatch, char[] IlegalMatch, int Capacity = 256)
        {
            var memory = new List<char>(Capacity);
            long ilegalPosition = 0;
            long targetPosition = 0;
            bool succeed = false;
            while (stream.CanRead)
            {
                var Byte = stream.ReadByte();
                if (Byte == -1) break;
                var Char = (char)Byte;
                if (Char == TargetMatch[targetPosition])
                {
                    targetPosition++;
                    if (targetPosition == TargetMatch.Length)
                    {
                        succeed = true;
                        break;
                    }
                }
                else
                {
                    targetPosition = 0;
                }

                if (Char == IlegalMatch[ilegalPosition])
                {
                    ilegalPosition++;
                    if (ilegalPosition == IlegalMatch.Length)
                        break;
                }
                else
                {
                    ilegalPosition = 0;
                }

                memory.Add(Char);
            }
            return succeed ? memory : null;
        }
    }
}
