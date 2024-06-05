using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.DTOs
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }

        public int DeliveryMethodId { get; set; }
        public OrderAddressDto ShippingAddress { get; set; }
    }
}
