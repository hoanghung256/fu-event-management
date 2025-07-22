using FUEM.Application.Interfaces.RegistrationUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.UseCases.RegistrationUseCases
{
    public class RegisterIntoEvent : IRegisterIntoEvent
    {
        private readonly IEventCollaboratorRepository _eventCollaboratorRepository;
        private readonly IEventGuestRepository _eventGuestRepository;

        public RegisterIntoEvent(IEventCollaboratorRepository eventCollaboratorRepository, IEventGuestRepository eventGuestRepository)
        {
            _eventCollaboratorRepository = eventCollaboratorRepository;
            _eventGuestRepository = eventGuestRepository;
        }

        public async Task RegisterGuestAsync(int eventId, int studentId)
        {
            var exists = await _eventGuestRepository.IsAlreadyRegisteredAsync(eventId, studentId);
            if (exists)
                throw new InvalidOperationException("Already registered.");

            var guest = new EventGuest
            {
                EventId = eventId,
                GuestId = studentId,
                IsRegistered = true,
                IsCancelRegister = false
            };

            await _eventGuestRepository.AddAsync(guest);
        }

        public async Task RegisterCollaboratorAsync(int eventId, int studentId)
        {
            var exists = await _eventCollaboratorRepository.IsAlreadyRegisteredAsync(eventId, studentId);
            if (exists)
                throw new InvalidOperationException("Already registered.");

            var collab = new EventCollaborator
            {
                EventId = eventId,
                StudentId = studentId,
                IsCancel = 0
            };
            await _eventCollaboratorRepository.AddAsync(collab);
        }
        public async Task<bool> CheckIfResgisterAsGuest(int eventId, int studentId) => await _eventGuestRepository.IsAlreadyRegisteredAsync(eventId, studentId);

        public async Task<bool> CheckIfResgisterAsCollaborator(int eventId, int studentId) => await _eventCollaboratorRepository.IsAlreadyRegisteredAsync(eventId, studentId);
    }
}
