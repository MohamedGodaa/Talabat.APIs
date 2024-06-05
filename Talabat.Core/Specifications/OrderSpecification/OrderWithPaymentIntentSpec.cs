using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Specifications.OrderSpecification
{
    public class OrderWithPaymentIntentSpec :BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpec(string PaymentIntentId):base(O=>O.PaymentIntentId==PaymentIntentId)
        {
            
        }
    }
}
