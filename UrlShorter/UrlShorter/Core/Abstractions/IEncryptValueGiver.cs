namespace UrlShorter.Core.Abstractions;

public interface IEncryptValueGiver
{
    Task<long> GetValue();
}