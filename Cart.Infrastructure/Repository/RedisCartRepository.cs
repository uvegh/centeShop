

using Cart.Domain.IRepository;
using Cart.Infrastructure.Redis;
using StackExchange.Redis;
using System.Text.Json;
using CartEntity = Cart.Domain.Entities.Cart;
namespace Cart.Infrastructure.Repository;

public  class RedisCartRepository:ICartRespository
{
   
    private readonly IDatabase _db;
   
    public RedisCartRepository(RedisConnectionFactory factory)
    {
        _db = factory.GetDatabase();
    }
public async Task<CartEntity?> GetByUserIdAsync(Guid UserId)
    {
        var key = $"cart:{UserId}";
     var data=  await _db.StringGetAsync(key);
        if (data.IsNullOrEmpty)
            return null;
     return   JsonSerializer.Deserialize<CartEntity>(data);
      
    }

    public async Task SaveCartAsync(CartEntity cart)
    {

        var key = $"cart:{cart.UserId}";
        await _db.StringSetAsync(key, JsonSerializer.Serialize(cart));
    }
}
