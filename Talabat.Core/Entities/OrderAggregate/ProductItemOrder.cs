using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.OrderAggregate
{
    public class ProductItemOrder
    {
        public ProductItemOrder()
        {
            
        }
        public ProductItemOrder(int productId, string productname, string pictureUrl)
        {
            ProductId = productId;
            Productname = productname;
            PictureUrl = pictureUrl;
        }

        public int ProductId { get; set; }
        public string Productname { get; set; }
        public string PictureUrl { get; set; }
    }
}
