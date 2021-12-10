using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class ControllerBase : Controller
    {
        public string ToTitleCase(string originalText)
        {
            string[] acronyms = { "USA", "UK" };

            if (acronyms.Contains(originalText.ToUpper()))
                return originalText.ToUpper();

            if (originalText == "callforpaper")
                return "Call For Paper";

            TextInfo englishTextInfo = new CultureInfo("en-GB", false).TextInfo;
            return englishTextInfo.ToTitleCase(originalText);
        }
    }
}
