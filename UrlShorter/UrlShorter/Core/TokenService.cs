using HashidsNet;
using UrlShorter.Core.Abstractions;

namespace UrlShorter.Core;

public class TokenService
{
    private readonly IEncryptValueGiver _valueGiver;

    public TokenService(IEncryptValueGiver valueGiver)
    {
        _valueGiver = valueGiver;
    }

    public string GenerateTokenFor(string longUrl)
    {
        var hashids = new Hashids("this is my salt");
        var id = _valueGiver.GetValue();
        var token = hashids.EncodeLong(id);

        // TODO save id and token in database 
        
        return token;
    }
}