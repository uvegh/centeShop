
using StackExchange.Redis;
namespace Cart.Infrastructure.Redis;

public  class RedisConnectionFactory
{
    private readonly Lazy<ConnectionMultiplexer> _connection;

    public RedisConnectionFactory( string connectionStr)
    {
        //
        _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionStr));
    }
    public IDatabase GetDatabase() => _connection.Value.GetDatabase();
}
