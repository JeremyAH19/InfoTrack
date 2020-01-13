using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace InfoTrack.Services
{
    public class HtmlSourceService : IHtmlSourceService
    {
        private readonly HttpClient _client = new HttpClient();

        public async Task<string> GetSource(string keywords)
        {
            var url = $"https://www.google.com.au/search?num=100&q={keywords.Replace(' ', '+')}";

            HttpResponseMessage response = await _client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var htmlSource = await response.Content.ReadAsStringAsync();
            
            return htmlSource;
        }
    }
}