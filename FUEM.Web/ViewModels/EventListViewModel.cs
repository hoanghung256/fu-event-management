using FUEM.Domain.Entities;

namespace FUEM.Web.ViewModels
{
    public class EventListViewModel
    {
        public required IEnumerable<Event> Items { get; init; }
    }
}
