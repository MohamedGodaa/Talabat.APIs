using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repository.Contract;
using Talabat.Core.Specifications;

namespace Talabat.Repository.Repository
{
    public class GenaricRepository<T> : IGenericRepository<T> where T : BaseEntitiy
    {
        private readonly StoreContext _dbContext;

        public GenaricRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(T item)
            =>await _dbContext.Set<T>().AddAsync(item);
        

        public async Task<IReadOnlyList<T>> GetAllWithspecificationAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetEntityWithSpecAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<T?> GetByIdAsync(int Id)
        {
            return await _dbContext.Set<T>().Where(t => t.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return  SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), specification);
        }

        public void Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
        }

        public void Update(T item)
        {
            _dbContext.Set<T>().Update(item);
        }
    }
}
