using Talabat.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.Order_Specs;
public class OrderPaymentSpecifiactions : BaseSpecification<Order>
{
    public OrderPaymentSpecifiactions(string paymentIntentId) : base(o => o.PaymentIntentId == paymentIntentId)
    {
       
    }
}
