using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class Product:BaseEntitiy
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }

        public int BrandId { get; set; }
        public int categorieId { get; set; }
        public ProductBrand ProductBrand { get; set; }
        public Productcategories Productcategories { get; set; }

    }
}
