using FUEM.Application.Interfaces.LocationUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.UseCases.LocationUseCases
{
    internal class GetAllLocation : IGetAllLocation
    {
        private readonly ILocationRepository _repository;

        public GetAllLocation(ILocationRepository repository) 
        {
            _repository = repository;
        }
        public Task<List<Location>> ExecuteAsync()
        {
            return _repository.GetAllAsync();
        }
    }
}
