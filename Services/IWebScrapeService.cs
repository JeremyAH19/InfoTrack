using InfoTrack.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfoTrack.Services
{
    public interface IWebScrapeService
    {
        Task<List<SearchResult>> GetSearchResults(SearchCriteria searchCriteria);
    }
}
