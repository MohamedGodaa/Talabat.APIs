using Talabat.Core.Entities;

namespace Talabat.DTOs
{
    public class ProductDTo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        
        public string BrandName { get; set; }
        public string CategorieName { get; set; }
    }
}
