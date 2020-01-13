using InfoTrack.Models;
using InfoTrack.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InfoTrack.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebScrapeService _service; 

        public HomeController(IWebScrapeService service)
        {
            _service = service;
        }

        public ViewResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetResults(string keywords, string targetUrl)
        {
            var searchCriteria = new SearchCriteria() { Keywords = keywords, TargetUrl = targetUrl };
            List<SearchResult> searchResults;

            try
            {
                searchResults = await _service.GetSearchResults(searchCriteria);
            }
            catch(HttpRequestException ex)
            {
                return Json(new { Success = "False", ResponseText = ex.Message });
            }

            var result = new SearchViewModel() { Keywords = keywords, TargetUrl = targetUrl, SearchResults = searchResults };

            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}