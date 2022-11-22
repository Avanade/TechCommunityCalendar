using System;
using System.Threading.Tasks;

namespace TechCommunityCalendar.Interfaces
{
    public interface ITechEventAdminRepository
    {
        public Task Add(ITechEvent techEvent);
        public Task Update(ITechEvent techEvent);
        public Task Remove(string id);
    }
}
