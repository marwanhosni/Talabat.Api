using Talabat.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Repository.Interfaces;
public interface IOrderService
{
    public  Task<Order?> GreateOrderAsync(string BuyerName , string BasketId , int DeleviryMethodId , Address ShippingAddress);
    
    public  Task<IReadOnlyList<Order>?> GetOrederForSpecAsync(string Email);

    public  Task<Order?> GetOrederByIdForSpecAsync(int Id, string Email);

}
