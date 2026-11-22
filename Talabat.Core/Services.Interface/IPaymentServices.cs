using Talabat.Core.Entities;
using Talabat.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Services.Interface;
public interface IPaymentServices
{
    public Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId);
    public Task<Order?> UpdateOrderPaymentIntentToSucceededOrFailed(string PaymentIntentId, bool flag);
}
