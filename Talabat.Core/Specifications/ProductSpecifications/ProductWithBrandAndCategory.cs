using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecifications
{
    public class ProductWithBrandAndCategory:BaseSpecification<Product>
    {
        public ProductWithBrandAndCategory(ProductSpecParams Params) 
            :base(
                P => 
                (string.IsNullOrEmpty(Params.Search)|| P.Name.ToLower().Contains(Params.Search))
                &&
                (!Params.BrandId.HasValue ||P.BrandId == Params.BrandId)
                &&
                (!Params.CategorieId.HasValue || P.categorieId == Params.CategorieId)
             )
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(P => P.Productcategories);
            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(P => P.Price);
                        break; 
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else 
                AddOrderBy(p => p.Name);


            //Product = 100
            //PageSize = 10
            // PageIndex = 5
            //Skip => 40
            // Take => 10
            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);
        }
        public ProductWithBrandAndCategory(int id):base(p=>p.Id == id)
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(P => P.Productcategories);
        }
    }
}
