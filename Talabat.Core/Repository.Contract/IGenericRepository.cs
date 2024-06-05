using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repository.Contract
{
    public interface IGenericRepository<T> where T : BaseEntitiy
    {
        Task<IReadOnlyList<T>> GetAllWithspecificationAsync(ISpecification<T> specification);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetEntityWithSpecAsync(ISpecification<T> specification);

        Task<T> GetByIdAsync(int Id);
        Task<int> GetCountAsync(ISpecification<T> specification);

        Task Add(T item);
        void Delete(T item);
        void Update(T item);

    }
}
