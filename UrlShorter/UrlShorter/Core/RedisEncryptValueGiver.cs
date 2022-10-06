using StackExchange.Redis;
using UrlShorter.Core.Abstractions;

namespace UrlShorter.Core;

public class RedisEncryptValueGiver : IEncryptValueGiver
{
    private readonly IDatabase _database;

    private long _currentValue;
    private long _maxValue;

    public RedisEncryptValueGiver(IConnectionMultiplexer connection)
    {
        _database = connection.GetDatabase();
    }

    public long GetValue()
    {
        if (_currentValue + 1 > _maxValue)
        {
            _currentValue = _maxValue + 1;
            _maxValue = _database.StringIncrement("counter", 10);
        }

        return _currentValue++;
    }
}