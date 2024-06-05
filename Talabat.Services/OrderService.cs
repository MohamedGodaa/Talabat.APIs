using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Repository.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.OrderSpecification;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository
                            ,IUnitOfWork unitOfWork
                            ,IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string busketId, int deliveryMethodId, Address shippingAddress)
        {
            //1.Get Basket From Basket Repo
            var Basket = await _basketRepository.GetBasketAsync(busketId);

            //2.Get Selected Items at Basket From Product Repo
            var OrderItems = new List<OrderItem>();
            if(Basket?.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItemOrder = new ProductItemOrder(Product.Id,Product.Name,Product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItemOrder, item.Quantity, Product.Price);
                    OrderItems.Add(OrderItem);
                }
            }

            //3.Calculate SubTotal => Price Of Product * Quantity
            var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);

            //4.Get Delivery Method From DeliveryMethod Repo
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            //5.Create Order
            var Spec = new OrderWithPaymentIntentSpec(Basket.PaymentIntentId);
            var ExOrder = _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            if(ExOrder is not null)
            {
                 _unitOfWork.Repository<Order>().Delete(await ExOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(busketId);
            }
            var Order = new Order(buyerEmail,shippingAddress,DeliveryMethod,OrderItems,SubTotal,Basket.PaymentIntentId);

            //6.Add Order Locally
            await _unitOfWork.Repository<Order>().Add(Order);

            //7.Save Order To Database[ToDo]
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return null;
            return Order;
        }

        

        public async Task<Order> GetOrdersByIdForSpecificUserAsync(string buyerEmail, int orderId)
        {
            var Spec = new OrderSpecifications(buyerEmail, orderId);
            var Order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            return Order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var Spec = new OrderSpecifications(buyerEmail);
            var Orders = await _unitOfWork.Repository<Order>().GetAllWithspecificationAsync(Spec);
            return Orders;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var DeliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return DeliveryMethods;
        }
    }
}
