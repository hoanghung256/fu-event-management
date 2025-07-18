using FUEM.Application.Interfaces.OrganizerUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.UseCases.OrganizerUseCases
{
    public class GetAllOrganizers : IGetAllOrganizers
    {
        private readonly IOrganizerRepository _repository;

        public GetAllOrganizers(IOrganizerRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Organizer>> GetAllOrganizersAsync()
            => await _repository.GetAllOrganizersAsync();
    }
}
