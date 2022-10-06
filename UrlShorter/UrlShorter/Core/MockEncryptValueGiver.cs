using UrlShorter.Core.Abstractions;

namespace UrlShorter.Core;

public class MockEncryptValueGiver : IEncryptValueGiver
{
    private int _max = 1000;
    private int _current = 0;
    public Task<long> GetValue()
    {
        if (_current > _max)
        {
            _current = 0;
        }

        return Task.FromResult<long>(_current++);
    }
}