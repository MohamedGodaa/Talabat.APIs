using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repository.Contract;
using Talabat.Repository.Repository;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        private readonly Hashtable _repositories;
        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
            => await _dbContext.SaveChangesAsync();
        

        public async ValueTask DisposeAsync()
            => await _dbContext.DisposeAsync();
        

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntitiy
        {
            // Key => Product , Order
            // Value => GenaricRepository<Product> or  GenaricRepository<Order>
            var type =typeof(TEntity).Name;//=>Product , Order
            if (!_repositories.ContainsKey(type))
            {
                var Repository = new GenaricRepository<TEntity>(_dbContext);
                _repositories.Add(type, Repository);
            }
            
            return _repositories[type] as IGenericRepository<TEntity>;
        }
    }
}
