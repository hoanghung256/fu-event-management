using FUEM.Domain.Entities;

namespace FUEM.Web.ViewModels
{
    public class AdminDashboardViewModel
    {
        public List<Event> UpcommingEventList { get; set; }
        public List<Event> PendingEventList { get; set; }
        public List<Event> OrganizedEventThisMonthList { get; set; }
    }
}
