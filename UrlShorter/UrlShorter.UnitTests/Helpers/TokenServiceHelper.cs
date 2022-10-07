using Moq;
using UrlShorter.Core;
using UrlShorter.Core.Abstractions;

namespace UrlShorter.UnitTests.Helpers;

public static class TokenServiceHelper
{
    public static TokenService GetService()
    {
        var encryptGiver = new Mock<IEncryptValueGiver>();
        encryptGiver.Setup(giver => giver.GetValueAsync()).Returns(Task.FromResult<long>(100));
        
        return new TokenService(encryptGiver.Object, DbHelper.GetDbContext(Guid.NewGuid().ToString()));
    }
}