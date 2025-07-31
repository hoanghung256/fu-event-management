using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Domain.Interfaces.Repositories
{
    public interface INotificationReceiverRepository
    {
        Task<List<int>> GetStudentIdsByEventIdAsync(int eventId);
        Task<List<int>> GetAllStudentIdsAsync(); 
        Task<List<int>> GetClubPresidentIdsAsync();
    }
}
