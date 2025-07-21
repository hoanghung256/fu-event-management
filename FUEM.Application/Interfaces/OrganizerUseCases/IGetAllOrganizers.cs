using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.OrganizerUseCases
{
    public interface IGetAllOrganizers
    {
        Task<List<Organizer>> GetAllOrganizersAsync();
    }
}
