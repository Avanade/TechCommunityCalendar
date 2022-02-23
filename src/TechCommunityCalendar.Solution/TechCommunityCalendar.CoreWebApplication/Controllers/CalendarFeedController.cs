using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class CalendarFeedController : ControllerBase
    {
        public CalendarFeedController(IMemoryCache memoryCache,
            ITechEventQueryRepository techEventRepository)
            : base(memoryCache, techEventRepository)
        {

        }

        public async Task<IActionResult> IndexAsync2()
        {
            var events = await _techEventRepository.GetAll();

            StringBuilder rssBuilder = new StringBuilder();

            rssBuilder.AppendLine("<rss>");
            rssBuilder.AppendLine("<channel>");
            rssBuilder.AppendLine($"<title>Tech Community Calendar</title>");
            rssBuilder.AppendLine($"<description></description>");
            rssBuilder.AppendLine($"<link>https://techcommunitycalendar.com</link>");

            foreach (var item in events.Where(x=>x.EndDate.Date >= DateTime.Now.Date))
            {
                rssBuilder.AppendLine("<item>");
                rssBuilder.AppendLine($"<title>{item.Name}</title>");
                rssBuilder.AppendLine($"<description>{item.Name}</description>");
                rssBuilder.AppendLine("</item>");
            }

            rssBuilder.AppendLine("</channel>");

            rssBuilder.AppendLine("</rss>");

            return Content(rssBuilder.ToString(), "text/xml");
        }

        public async Task<IActionResult> IndexAsync()
        {
            var events = await _techEventRepository.GetAll();

            StringBuilder iCal = new StringBuilder();
            iCal.AppendLine("BEGIN:VCALENDAR");
            iCal.AppendLine("VERSION:2.0");
            iCal.AppendLine("PRODID:-//Avanade DevRel//NONSGML v1.0//EN");

            foreach(var item in events.Where(x => x.EndDate.Date >= DateTime.Now.Date))
            {
                iCal.AppendLine("BEGIN:VEVENT");
                iCal.AppendLine($"UID:{Guid.NewGuid()}");
                iCal.AppendLine("DTSTAMP:20120315T170000Z");
                iCal.AppendLine($"DTSTART:{item.StartDate.ToString("yyyyMMddTHHmmssZ")}");
                iCal.AppendLine($"DTEND:{item.EndDate.ToString("yyyyMMddTHHmmssZ")}");
                iCal.AppendLine($"LOCATION:{item.Country} {item.City}");
                iCal.AppendLine($"SUMMARY:{item.Name}");
                iCal.AppendLine($"DESCRIPTION:Event Url:{item.Url}\\nNote: Please check the event details with event organisers.\\n\\n" + "Make sure to check out other events at https://TechCommunityCalendar.com");
                

                iCal.AppendLine("END:VEVENT");
            }


            iCal.AppendLine("END:VCALENDAR");

            return Content(iCal.ToString());
        }



    }
}
