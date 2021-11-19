using System;
using System.Linq;
using System.Threading.Tasks;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Models
{
    public class TechEventViewModel
    {
        public ITechEvent TechEvent { get; set; }
    }

}
