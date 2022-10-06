using StackExchange.Redis;
using UrlShorter.Core.Abstractions;
using UrlShorter.Core.Settings;

namespace UrlShorter.Core;

public class RedisEncryptValueGiver : IEncryptValueGiver
{
    private readonly IDatabase _database;

    private long _currentValue;
    private long _maxValue;

    private readonly RedisSettings _settings;

    public RedisEncryptValueGiver(IConnectionMultiplexer connection, RedisSettings settings)
    {
        _settings = settings;
        _database = connection.GetDatabase();
    }

    public long GetValue()
    {
        if (_currentValue + 1 > _maxValue)
        {
            _currentValue = _maxValue + 1;
            _maxValue = _database.StringIncrement(_settings.CounterKey, _settings.Step);
        }

        return _currentValue++;
    }
}