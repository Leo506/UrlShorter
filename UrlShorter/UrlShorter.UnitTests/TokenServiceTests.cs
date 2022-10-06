using FluentAssertions;
using Moq;
using UrlShorter.Core;
using UrlShorter.Core.Abstractions;
using UrlShorter.Database;
using UrlShorter.UnitTests.Helpers;

namespace UrlShorter.UnitTests;

public class TokenServiceTests
{
    [Fact]
    public async Task GenerateTokenFor_All_Good_Returns_Token()
    {
        // arrange
        var valueGiver = new Mock<IEncryptValueGiver>();
        valueGiver.Setup(x => x.GetValue()).Returns(1000);
        var sut = new TokenService(valueGiver.Object,
            DbHelper.GetDbContext(nameof(GenerateTokenFor_All_Good_Returns_Token)));

        // act
        var result = await sut.GenerateTokenFor("https://longurl.com");
        var actual = string.IsNullOrEmpty(result);
        
        // assert
        actual.Should().Be(false);
    }

    [Fact]
    public async Task GenerateTokenFor_ValueGiver_Throws_Returns_Empty_String()
    {
        // arrange
        var valueGiver = new Mock<IEncryptValueGiver>();
        valueGiver.Setup(x => x.GetValue()).Throws<Exception>();
        var sut = new TokenService(valueGiver.Object,
            DbHelper.GetDbContext(nameof(GenerateTokenFor_ValueGiver_Throws_Returns_Empty_String)));

        // act
        var result = await sut.GenerateTokenFor("https://longurl.com");
        var actual = string.IsNullOrEmpty(result);
        
        // assert
        actual.Should().Be(true);
    }
}