using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.EventUseCases
{
    public interface IGetOrganizedEvents
    {
        Task<Page<Event>> GetOrganizedEventsForOrganizerAsync(string organizerId, int pageNumber, int pageSize);

       
        Task<Page<Event>> GetOrganizedEventsForAdminAsync(int pageNumber, int pageSize);
    }
}