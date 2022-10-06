using FluentAssertions;
using Moq;
using UrlShorter.Core;
using UrlShorter.Core.Abstractions;

namespace UrlShorter.UnitTests;

public class TokenServiceTests
{
    [Fact]
    public void GenerateTokenFor_All_Good_Returns_Token()
    {
        // arrange
        var valueGiver = new Mock<IEncryptValueGiver>();
        valueGiver.Setup(x => x.GetValue()).Returns(1000);
        var sut = new TokenService(valueGiver.Object);

        // act
        var result = sut.GenerateTokenFor("https://longurl.com");
        var actual = string.IsNullOrEmpty(result);
        
        // assert
        actual.Should().Be(false);
    }

    [Fact]
    public void GenerateTokenFor_ValueGiver_Throws_Returns_Empty_String()
    {
        // arrange
        var valueGiver = new Mock<IEncryptValueGiver>();
        valueGiver.Setup(x => x.GetValue()).Throws<Exception>();
        var sut = new TokenService(valueGiver.Object);

        // act
        var result = sut.GenerateTokenFor("https://longurl.com");
        var actual = string.IsNullOrEmpty(result);
        
        // assert
        actual.Should().Be(true);
    }
}