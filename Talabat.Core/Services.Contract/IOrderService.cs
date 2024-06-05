using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Services.Contract
{
    public interface IOrderService
    {
        //Create Order [BuyerEmail , BasketId , Delivery Method Id , Shipping Address]

        Task<Order?> CreateOrderAsync(string buyerEmail, string busketId, int deliveryMethodId, Address shippingAddress);

        //2.Get Orders For Specific User [BuyerEmail]

        Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail);

        //3.Get Order By Id For Specific User [BuyerEmail , Order Id]
        Task<Order> GetOrdersByIdForSpecificUserAsync(string buyerEmail, int orderId);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
