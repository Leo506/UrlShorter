using StackExchange.Redis;
using UrlShorter.Core.Abstractions;
using UrlShorter.Core.Settings;

namespace UrlShorter.Core;

public class RedisEncryptValueGiver : IEncryptValueGiver
{
    private readonly IRedisService _redisService;

    private long _currentValue;
    private long _maxValue;


    public RedisEncryptValueGiver(IRedisService redisService)
    {
        _redisService = redisService;
    }

    public async Task<long> GetValueAsync()
    {
        if (_currentValue + 1 > _maxValue)
        {
            _currentValue = _maxValue + 1;
            _maxValue = await _redisService.GetCounter();
        }

        return _currentValue++;
    }
}