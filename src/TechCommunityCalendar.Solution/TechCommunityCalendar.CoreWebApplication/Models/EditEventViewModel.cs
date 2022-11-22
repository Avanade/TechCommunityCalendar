using System.ComponentModel.DataAnnotations;

namespace TechCommunityCalendar.CoreWebApplication.Models
{
    public class EditEventViewModel : AddEventViewModel
    {
        public string Id { get; set; }
        public bool Hidden { get; set; }
        public bool Delete { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string AdminPassword { get; set; }
    }
}
