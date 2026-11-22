using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Repository.Interfaces;

namespace Talabat.APIs.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    private readonly IMapper _mapper;

    public BasketController(IBasketRepository basketRepository,IMapper mapper)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
    {
        var basket = await _basketRepository.GetBasketAsync(id);
        return Ok(basket is null ? new CustomerBasket() { Id = id } : basket);
    }

    // POST : /api/Basket
    [HttpPost]
    public async Task<ActionResult<CustomerBasket>> CreateOrUpdateBasket(CustmorBasketDto basket)
    {
        var mappedBasket = _mapper.Map<CustmorBasketDto, CustomerBasket>(basket);
        var CraeteOrUpdate = await _basketRepository.UpdateBasketAsync(mappedBasket);
        if (CraeteOrUpdate is null) return BadRequest(new ApiRequest(400));
        return Ok(CraeteOrUpdate);

    }

    // DELETE : /api/Basket?id=
    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteBasket(string id)
    {
        return await _basketRepository.DeleteBasketAsync(id);
    }
}
