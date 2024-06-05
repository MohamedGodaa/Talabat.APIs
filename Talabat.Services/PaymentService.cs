using Microsoft.Extensions.Configuration;
using Stripe;
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
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration,IBasketRepository basketRepository
                              , IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
        {
            // Secret key
            StripeConfiguration.ApiKey = _configuration["StripeKeys:Secretkey"];
            //Get Basket
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            if (Basket is null) return null;
            var ShippingPrice = 0M;
            if (Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                ShippingPrice = DeliveryMethod.Cost;
            }
            // Total = SubTotal + DM.Cost
            if (Basket.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if(item.Price != Product.Price)
                    {
                        item.Price = Product.Price;
                    }
                }
            }
            var SubTotal = Basket.Items.Sum(item => item.Price * item.Quantity);
            var Total = ShippingPrice + SubTotal;

            // create Payment Intent
            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(Basket.PaymentIntentId)) // create
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long) Total * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> {"card"}
                };
                paymentIntent = await Service.CreateAsync(Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)Total * 100
                };
                paymentIntent = await Service.UpdateAsync(Basket.PaymentIntentId, Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            
            await _basketRepository.UpdateBasketAsync(Basket);
            return Basket;
        }
    }
}
