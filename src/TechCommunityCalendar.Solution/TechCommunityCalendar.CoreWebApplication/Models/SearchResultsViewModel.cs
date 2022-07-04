using System.Collections.Generic;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Models
{
    public class SearchResultsViewModel : EventsViewModelBase
    {
        public string Title { get; set; }

        public SearchResultsViewModel(IEnumerable<ITechEvent> searchResults, string searchTerm)
        {
            Title = searchTerm;
            Events = searchResults;
        }
    }
}
