using InfoTrack.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace InfoTrack.Services
{
    public class WebScrapeService : IWebScrapeService
    {
        private readonly IHtmlSourceService _service;

        public WebScrapeService(IHtmlSourceService service)
        {
            _service = service;
        }

        public async Task<List<SearchResult>> GetSearchResults(SearchCriteria searchCriteria)
        {
            string source = await _service.GetSource(searchCriteria.Keywords);

            List<SearchResult> results = ParseHtml(source, searchCriteria.TargetUrl);

            return results;
        }

        private List<SearchResult> ParseHtml(string source, string targetUrl)
        {
            var results = new List<SearchResult>();
            var resultNumber = 1;
            var startTag = "<div class=\"BNeawe UPmit AP7Wnd\">";
            var endTag = "</div>";
            var startTagIndex = source.IndexOf(startTag) + startTag.Length;
            var endTagIndex = source.IndexOf(endTag, startTagIndex);
            var currentIndex = 1;

            while (resultNumber <= 100 && startTagIndex > currentIndex && endTagIndex > currentIndex)
            {
                string text = source.Substring(startTagIndex, endTagIndex - startTagIndex);
                text = HttpUtility.HtmlDecode(text).Replace(" › ", "/");

                if (text.Contains(targetUrl)) results.Add(new SearchResult() { Number = resultNumber, Url = text });

                currentIndex = endTagIndex + 1;

                startTagIndex = source.IndexOf(startTag, currentIndex) + startTag.Length;
                endTagIndex = source.IndexOf(endTag, startTagIndex);

                resultNumber++;
            }

            return results;
        }
    }
}