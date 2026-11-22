using Talabat.Core.Entities;
using Talabat.Repository.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Talabat.Repository.Implementations;
public class BasketRepository : IBasketRepository
{
    private readonly IDatabase _contextRedis;

    public BasketRepository(IConnectionMultiplexer redis)
    {
        _contextRedis = redis.GetDatabase();
    }
    public async Task<bool> DeleteBasketAsync(string basketId)
    {
        return await _contextRedis.KeyDeleteAsync(basketId);
    }

    public async Task<CustomerBasket?> GetBasketAsync(string basketId)
    {
        var basket = await _contextRedis.StringGetAsync(basketId);
        return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
    }

    public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
    {
        var CreateOrUpdate = await _contextRedis.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
        if(!CreateOrUpdate)
        {
            return null;
        }
        return await GetBasketAsync(basket.Id);

    }
}
