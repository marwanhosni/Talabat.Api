using Talabat.Core.Entities.Orders;

namespace Talabat.APIs.Dtos;

public class OrderToReturnDto
{
    public int Id { get; set; }

    public string BuyerEmail { get; set; }

    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

    public string Status { get; set; } 

    public Address ShippingAddress { get; set; }

    public string DeliveryMethods { get; set; }
    public decimal DeliveryMethodsCost { get; set; }

    public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();

    public decimal Subtotal { get; set; }

    public decimal Total { get; set; }

    public string PaymentIntentId { get; set; } = string.Empty;

}
/*
 [
    {
        "buyerEmail": "Aya123@gmail.com",
        "orderDate": "2026-11-05T18:08:27.1482342+02:00",
        "status": 0,
        "shippingAddress": {
            "fName": "Marwan",
            "lName": "Hosni",
            "street": "HassanElGaniny",
            "city": "Zagazig",
            "country": "Egypt"
        },
        "deliveryMethods": {
            "id": 1,
            "shortName": "UPS1",
            "description": "Fastest delivery time",
            "deliveryTime": "1-2 Days",
            "cost": 10.00
        },
        "items": [
            {
                "product": {
                    "productId": 9,
                    "productName": "Blueberry Cheesecake",
                    "pictureUrl": "images/products/hat-react2.png"
                },
                "price": 15.00,
                "quantity": 50,
                "id": 1
            }
        ],
        "subtotal": 750.00,
        "paymentIntentId": "",
        "id": 1
    }
]
 */