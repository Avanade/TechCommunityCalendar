using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading.Tasks;
using TechCommunityCalendar.Concretions;
using TechCommunityCalendar.CoreWebApplication.Models;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class SearchController : ControllerBase
    {
        public SearchController(
            IMemoryCache memoryCache, 
            ITechEventQueryRepository techEventRepository) 
            : base(memoryCache, techEventRepository)
        {
        }

        // Search Form
        [HttpPost]
        [Route("Search/Search")]

        public async Task<IActionResult> Search(SearchModel searchModel)
        {
            var events = await GetEventsFromCache();
            events = events.Where(x => !x.Hidden).ToArray();
            var searchResults = TechEventCalendar.Search(searchModel.SearchTerm, events);
            
            var model = new SearchResultsViewModel(searchResults, searchModel.SearchTerm);

            // Page properties
            ViewBag.Title = $"Tech Community Events Search Results for {searchModel.SearchTerm}";

            return View("SearchResults", model);

        }

        // QueryString?
        [HttpGet]
        [Route("search/{searchTerm}")]
        public async Task<IActionResult> Search(string searchTerm)
        {           
            var events = await GetEventsFromCache();
            var searchResults = TechEventCalendar.Search(searchTerm, events);
            var model = new SearchResultsViewModel(searchResults, searchTerm);

            ViewBag.Title = $"Tech Community Events Search Results for {searchTerm}";

            return View("SearchResults",model);
        }
    }
}
