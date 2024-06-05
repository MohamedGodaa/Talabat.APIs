using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<TEntity> where TEntity : BaseEntitiy
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inpQuery,ISpecification<TEntity> specification)
        {
            var query = inpQuery;

            if(specification.Criteria is not null)
            {
                query = query.Where(specification.Criteria);
            }


            if(specification.OrderBy is not null)
                query = query.OrderBy(specification.OrderBy);

            else if(specification.OrderByDesc is not null)
                query = query.OrderByDescending(specification.OrderByDesc);


            if (specification.IsPaginationEnable)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }



            query = specification.Includes.Aggregate(query, (CurrentQuery, IncludeQuery) => CurrentQuery.Include(IncludeQuery));
            
            return query;
        }
    }
}
