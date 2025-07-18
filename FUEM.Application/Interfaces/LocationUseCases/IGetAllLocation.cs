using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.LocationUseCases
{
    public interface IGetAllLocation
    {
        Task<List<Location>> ExecuteAsync();
    }
}
