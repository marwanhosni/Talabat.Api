using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Orders;
public class Order : BaseEntity
{
    public Order()
    {
        
    }
    public Order(string buyerEmail, Address shippingAddress, DeliveryMethods deliveryMethods, ICollection<OrderItem> items, decimal subtotal , string paymentIntentId)
    {
        BuyerEmail = buyerEmail;
        ShippingAddress = shippingAddress;
        DeliveryMethods = deliveryMethods;
        Items = items;
        Subtotal = subtotal;
        PaymentIntentId = paymentIntentId;
    }

    public string BuyerEmail { get; set; }
    
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public Address ShippingAddress { get; set; }
     
    public DeliveryMethods DeliveryMethods { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();

    public decimal Subtotal { get; set; }

    public decimal GetTotal () => Subtotal + DeliveryMethods.Cost;

    public string PaymentIntentId { get; set; }



}
    