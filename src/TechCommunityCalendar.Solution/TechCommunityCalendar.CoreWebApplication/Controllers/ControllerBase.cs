using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class ControllerBase : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly string CacheKey_AllEvents = "TechEvents";
        private readonly string CacheKey_Countries = "Countries";
        public ITechEventQueryRepository _techEventRepository;

        public ControllerBase(IMemoryCache memoryCache,
            ITechEventQueryRepository techEventRepository)
        {
            _memoryCache = memoryCache;
            _techEventRepository = techEventRepository;
        }

        public async Task<ITechEvent[]> GetEventsFromCache()
        {
            if (!_memoryCache.TryGetValue(CacheKey_AllEvents, out ITechEvent[] events))
            {
                events = await _techEventRepository.GetAll();

                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(60),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(20)
                };
                _memoryCache.Set(CacheKey_AllEvents, events, cacheExpiryOptions);
            }

            return events;
        }

        public async Task<string[]> GetCountriesFromCache()
        {
            if (!_memoryCache.TryGetValue(CacheKey_Countries, out string[] countries))
            {
                var events = await GetEventsFromCache();

                countries = events.Select(x => x.Country)
                .Distinct()
                .Where(x => !String.IsNullOrWhiteSpace(x))
                .OrderBy(x => x).ToArray();

                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(60),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(20)
                };
                _memoryCache.Set(CacheKey_Countries, countries, cacheExpiryOptions);
            }

            return countries;
        }

        public string ToTitleCase(string originalText)
        {
            string[] acronyms = { "USA", "UK" };

            if (acronyms.Contains(originalText.ToUpper()))
                return originalText.ToUpper();

            if (originalText == "callforpaper")
                return "Call For Paper";

            if (originalText == "callforpapers")
                return "Call For Papers";

            TextInfo englishTextInfo = new CultureInfo("en-GB", false).TextInfo;
            return englishTextInfo.ToTitleCase(originalText);
        }
    }
}
