using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications.ProductSpecifications;

namespace Talabat.Core.Specifications
{
    public class ProductWithFiltrationWithCountAsync:BaseSpecification<Product>
    {
        public ProductWithFiltrationWithCountAsync(ProductSpecParams Params)
            : base(
                P =>
                (string.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search))
                && 
                (!Params.BrandId.HasValue || P.BrandId == Params.BrandId)
                &&
                (!Params.CategorieId.HasValue || P.categorieId == Params.CategorieId)
             )
        {
            
        }
    }
}
