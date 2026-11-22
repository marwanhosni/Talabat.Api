using Talabat.Core.Entities;
using Talabat.Core.Entities.Orders;
using Talabat.Core.Services.Interface;
using Talabat.Core.Specifications.Order_Specs;
using Talabat.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Repository.Implementations;
public class OrderService : IOrderService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentServices _paymentServices;

    public OrderService(IBasketRepository basketRepository,
        IUnitOfWork unitOfWork,
        IPaymentServices paymentServices)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
        _paymentServices = paymentServices;
    }
    public async Task<Order?> GreateOrderAsync(string BuyerEmail, string BasketId, int DeleviryMethodId, Address ShippingAddress)
    {
        //1. Get Basket from BasketRepo
        var basket = await _basketRepository.GetBasketAsync(BasketId);

        //2. Get Items from Product Repo
        var OrderItems = new List<OrderItem>();
        if (basket?.Items.Count > 0) 
        {
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                var productItemOrder = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
                var orderItem = new OrderItem(productItemOrder, product.Price, item.Quantity);

                OrderItems.Add(orderItem);
            }
        }

        //3. Calculate Subtotal
        var subtotal = OrderItems.Sum(sub => sub.Price * sub.Quantity);

        //4. Delevery Method
        var deleveryMethod = await _unitOfWork.Repository<DeliveryMethods>().GetByIdAsync(DeleviryMethodId);

        // Check if Payment Intent Is is Existing  From Another Order
        var spec = new OrderPaymentSpecifiactions(basket.PaymentIntentId);
        var ExistOrder = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
        if(ExistOrder is not null)
        {
             _unitOfWork.Repository<Order>().Delete(ExistOrder)    ;
            // Update PaymentIntent If With Amount Of Basket If Changed   
            basket =  await _paymentServices.CreateOrUpdatePaymentIntent(BasketId);
        }
        //5. Create Order
        var order = new Order(BuyerEmail, ShippingAddress, deleveryMethod,OrderItems,subtotal , basket.PaymentIntentId);

        //6. Add Order Locally
        await _unitOfWork.Repository<Order>().Add(order);

        //7. Add Order TO DataBase 
        var result = await _unitOfWork.CompleteAsync();

        return result > 0 ? order : null;
    }

    public async Task<IReadOnlyList<Order>?> GetOrederForSpecAsync(string Email)
    {
        var spec = new OrderSpecifications(Email);
        var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
        return orders;
    }
    public async Task<Order?> GetOrederByIdForSpecAsync(int Id, string Email)
    {
        var spec = new OrderSpecifications(Email, Id);
        var orders = await   _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec );
        if(orders == null) return null;
        return orders;
    }

    

    
}
