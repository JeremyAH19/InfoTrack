using System.Collections.Generic;

namespace InfoTrack.Models
{
    public class SearchViewModel
    {
        public string Keywords { get; set; }
        public string TargetUrl { get; set; }  
        public List<SearchResult> SearchResults { get; set; }
    }
}