namespace UrlShorter.Core.Abstractions;

public interface IRedisService
{
    Task<long> GetCounter();
}