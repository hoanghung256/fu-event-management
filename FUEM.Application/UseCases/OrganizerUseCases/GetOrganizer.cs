using FUEM.Application.Interfaces.OrganizerUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.UseCases.OrganizerUseCases
{
    public class GetOrganizer : IGetOrganizer
    {
        private readonly IOrganizerRepository _repository;

        public GetOrganizer(IOrganizerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Organizer> GetOrganizerByEmailAsync(string email)
            => await _repository.GetOrganizerByEmailAsync(email);

        public async Task<Organizer> GetOrganizerByIdAsync(int id)
            => await _repository.GetOrganizerByIdAsync(id);
    }
}
