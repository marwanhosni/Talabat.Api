using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Orders;
using Talabat.Repository.Interfaces;
using OrderAddress = Talabat.Core.Entities.Orders.Address;
using System.Security.Claims;
using Talabat.Repository;
using StackExchange.Redis;

namespace Talabat.APIs.Controllers;

public class OrdersController : BaseApiController
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public OrdersController(IOrderService orderService, IMapper mapper , IUnitOfWork unitOfWork)
    {
        _orderService = orderService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    [Authorize]
    [HttpPost()]
    public async Task <ActionResult<OrderToReturnDto>> CreateOrder(OrderDto model)
    {
        var userBuyerEmail =  User.FindFirstValue(ClaimTypes.Email);
        var orderAddress = _mapper.Map<AddressDto,OrderAddress>(model.ShippingAddress);
        var order = await _orderService.GreateOrderAsync(userBuyerEmail, model.BasketId, model.DeleviryMethodId, orderAddress);
        if (order == null) return BadRequest(new ApiRequest(400, "There Is A proble in your Order"));
        var result =  _mapper.Map<OrderToReturnDto>(order);
        return Ok(result);

    }
    [HttpGet()]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
    {
        var userBuyerEmail = User.FindFirstValue(ClaimTypes.Email);
        var orders = await _orderService.GetOrederForSpecAsync(userBuyerEmail);
        if (orders == null) 
            return NotFound(new ApiRequest(404, "There Is No Orders For This User"));

        return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
    }
    [HttpGet("{Id}")]
    [Authorize]
    public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int Id)
    {
        var userBuyerEmail = User.FindFirstValue(ClaimTypes.Email);
        var order = await _orderService.GetOrederByIdForSpecAsync(Id, userBuyerEmail);
        if (order == null)
            return NotFound(new ApiRequest(404, "There Is No Order For This User With This Id"));
        return Ok(_mapper.Map<OrderToReturnDto>(order));
    }
    [HttpGet("DeliveryMethods")]
    public async Task<ActionResult<IReadOnlyList<DeliveryMethods>>> GetAllDeliveryMethods()
    {

        var deliveryMethods = await _unitOfWork.Repository<DeliveryMethods>().GetAllAsync();
        return Ok(deliveryMethods);
    }

}
