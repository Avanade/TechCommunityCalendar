using System;
using System.ComponentModel.DataAnnotations;

namespace TechCommunityCalendar.CoreWebApplication.Models
{
    public class AddEventViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        
        [Display(Name="Event Type")]
        [Required]
        public string EventType { get; set; }
        
        [Display(Name = "Event Format")]
        [Required]
        public string EventFormat { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Display(Name = "Duration")]
        [Required]
        public string Duration { get; set; }

        public string City { get; set; }

        // Make this drop down list
        public string Country { get; set; }
        
    }
}
