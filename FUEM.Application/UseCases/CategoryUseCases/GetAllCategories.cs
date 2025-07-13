using FUEM.Application.Interfaces.CategoryUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.UseCases.CategoryUseCases
{
    public class GetAllCategories : IGetAllCategories
    {
        private readonly ICategoryRepository _repository;

        public GetAllCategories(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
           => await _repository.GetAllCategoriesAsync();
    }
}
