using FUEM.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.EventUseCases
{
    public interface ICompareEventUseCase
    {
        Task<List<Event>> CompareEvent(int eventId, int? comparedId);
        Task<List<Event>> GetRemainEventAsync(string organizedId,int eventId, int? comparedId);
    }
}
