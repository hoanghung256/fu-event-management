using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Domain.Interfaces.Repositories
{
    public interface IOrganizerRepository
    {
        Task<Organizer> GetOrganizerByEmailAsync(string email);

        Task UpdatePasswordHashAsync(int id, string newPasswordHash);
      
        Task<List<Organizer>> GetAllOrganizersAsync();
    }
}
