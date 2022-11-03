using System;
using System.Threading.Tasks;
using TechCommunityCalendar.Enums;

namespace TechCommunityCalendar.Interfaces
{
    public interface ITechEventQueryRepository
    {
        Task<ITechEvent[]> GetAll();
        Task<ITechEvent[]> GetByMonth(int year, int month);
        Task<ITechEvent[]> GetByYear(int year);
        Task<ITechEvent[]> GetByEventType(int year, int month, EventType eventType);
        Task<ITechEvent[]> GetByEventType(EventType eventType);
        Task<ITechEvent[]> GetByCountry(EventType eventType, string country);
        //Task<ITechEvent> Get(int year, int month, Guid id);
        Task<ITechEvent> Get(string id);
        Task<string[]> GetAllCountries();
        void AppendTrailingComma();
    }
}
