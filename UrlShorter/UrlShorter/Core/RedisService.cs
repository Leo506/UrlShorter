using StackExchange.Redis;
using UrlShorter.Core.Abstractions;
using UrlShorter.Core.Settings;

public class RedisService : IRedisService
{
    private readonly RedisSettings _settings;
    private readonly IDatabase _database;
    
    public RedisService(IConnectionMultiplexer multiplexer, RedisSettings settings)
    {
        _settings = settings;
        _database = multiplexer.GetDatabase();
    }

    public async Task<long> GetCounter()
    {
        return await _database.StringIncrementAsync(_settings.CounterKey, _settings.Step);
    }
}