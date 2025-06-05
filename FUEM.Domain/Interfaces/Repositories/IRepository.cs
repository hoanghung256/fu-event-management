using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Domain.Interfaces.Repositories
{
    // Basic CRUD actions of an entity
    internal interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task RemoveAsync(T entity);
        Task<T> UpdateAsync(T entity);
    }
}
