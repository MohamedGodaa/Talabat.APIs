using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.DTOs
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string Productname { get; set; }
        public string PictureUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}