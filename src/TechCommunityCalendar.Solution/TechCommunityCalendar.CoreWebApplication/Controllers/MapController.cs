using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechCommunityCalendar.CoreWebApplication.Models;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class MapController : ControllerBase
    {
        public MapController(IMemoryCache memoryCache,
            ITechEventQueryRepository techEventRepository)
            : base(memoryCache, techEventRepository)
        {

        }
        public async Task<IActionResult> IndexAsync()
        {
            var events = await _techEventRepository.GetAll();

            // Get distinct list of city names
            var cities = events.Select(x => x.City).Distinct();

            StringBuilder cityArrayCode = new StringBuilder();
            cityArrayCode.AppendLine("const cities = [");

            foreach (var city in cities)
            {
                cityArrayCode.AppendLine($"\"{city}\",");
            }

            cityArrayCode.AppendLine("];");

            var model = new MapViewModel();
            model.ArrayCode = cityArrayCode.ToString();
            model.Cities = cities;

            return View(model);
        }
    }
}
