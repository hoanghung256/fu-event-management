using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;

namespace FUEM.Web.ViewModels
{
    public class EventListViewModel
    {
        public required IEnumerable<Event> Items { get; init; }
        public required int CurrentPage { get; init; }
        public required int TotalPages { get; init; }
        public SearchEventCriteria? SearchCriteria { get; init; }
    }
}
