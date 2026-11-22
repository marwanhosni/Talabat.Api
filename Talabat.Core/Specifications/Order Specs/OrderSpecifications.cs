using Talabat.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.Order_Specs;
public class OrderSpecifications : BaseSpecification<Order>
{
    public OrderSpecifications(string email ) : base(o => o.BuyerEmail == email)
    {
        Include.Add(o=>o.Items);
        Include.Add(o => o.DeliveryMethods);
        AddOrderByDesc( o => o.OrderDate);
    }
    public OrderSpecifications(string email , int orderId ) : base(o => o.BuyerEmail == email && o.Id == orderId )
    {
        Include.Add(o => o.Items);
        Include.Add(o => o.DeliveryMethods);
    }
}
