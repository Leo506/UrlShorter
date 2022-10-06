using UrlShorter.Core.Abstractions;

namespace UrlShorter.Core;

public class MockEncryptValueGiver : IEncryptValueGiver
{
    private int _max = 1000;
    private int _current = 0;
    public long GetValue()
    {
        if (_current > _max)
        {
            _current = 0;
        }

        return _current++;
    }
}