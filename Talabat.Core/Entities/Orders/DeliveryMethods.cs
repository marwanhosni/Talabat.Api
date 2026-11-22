using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Orders;
public class DeliveryMethods : BaseEntity
{
    public DeliveryMethods()
    {
        
    }
    public DeliveryMethods(int id, string shortName, string description, string deliveryTime, decimal cost)
    {
        Id = id;
        ShortName = shortName;
        Description = description;
        DeliveryTime = deliveryTime;
        Cost = cost;
    }

    public int Id { get; set; }
    public string ShortName{ get; set; }
    public string Description{ get; set; }
    public string DeliveryTime{ get; set; }
    public decimal Cost { get; set; }
}
