using Microsoft.Extensions.Configuration;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Orders;
using Talabat.Core.Services.Interface;
using Talabat.Core.Specifications.Order_Specs;
using Talabat.Repository;
using Talabat.Repository.Implementations;
using Talabat.Repository.Interfaces;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service;
public class PaymentServices : IPaymentServices
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public PaymentServices(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
    {
        // 1. Get Basket
        var basket = await _basketRepository.GetBasketAsync(BasketId);
        if (basket == null) return null;
        // 2. Total Price 
        if (basket?.Items.Count > 0)
        {
            foreach (var item in basket.Items)
            {
                var productPrice = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if (item.Price != productPrice.Price)
                {
                    item.Price = productPrice.Price;
                }
            }
        }
        // 3. Sub Total 
        var subtotal = basket.Items.Sum(sub => sub.Price * sub.Quantity);
        var shippingPrice = 0m;
        if (basket.DeliveryMethodsId.HasValue)
        {
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethods>().GetByIdAsync(basket.DeliveryMethodsId.Value);
            shippingPrice = deliveryMethod.Cost;
        }
        // 4. Create Payment Intent - Call Stripe
        StripeConfiguration.ApiKey = _configuration["StripeKeys:Secretkey"];
        var service = new PaymentIntentService();
        PaymentIntent PaymentIntent;
        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            // Create New PaymentIntent 
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)((subtotal + shippingPrice) * 100),
                Currency = "usd",
                //AllowedPaymentMethodTypes = new List<string> { "card" }
            };
            PaymentIntent = await service.CreateAsync(options);
            basket.PaymentIntentId = PaymentIntent.Id;
            basket.ClientSecret = PaymentIntent.ClientSecret;
            // 5. Update Basket with Payment Intent Id and Client Secret
        }
        else
        {
            // Update PaymentIntent 
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (long)((subtotal + shippingPrice) * 100),
            };
            PaymentIntent = await service.UpdateAsync(basket.PaymentIntentId, options);
            basket.PaymentIntentId = PaymentIntent.Id;
            basket.ClientSecret = PaymentIntent.ClientSecret;
        }
        // return basket including PaymentIntentId and ClientSecret

        await _basketRepository.UpdateBasketAsync(basket);
        return basket;
    }

    public async Task<Order?> UpdateOrderPaymentIntentToSucceededOrFailed(string PaymentIntentId, bool flag)
    {
        var spec = new OrderPaymentSpecifiactions(PaymentIntentId);
        var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
        if (flag)
        {
            order.Status = OrderStatus.PaymentReceived;
        }
        else
        {
            order.Status = OrderStatus.PaymentFailed;
        }
        _unitOfWork.Repository<Order>().Update(order);
        await _unitOfWork.CompleteAsync();
        return order;

    }
}
