using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.EventUseCases
{
    public interface ICreateEvent
    {
        Task ExecuteAsync(Event createEvent, Role role, Stream avatar, List<EventImage> additionalImages);
    }
}
