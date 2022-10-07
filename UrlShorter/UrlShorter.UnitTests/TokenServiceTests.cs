using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UrlShorter.Core;
using UrlShorter.Core.Abstractions;
using UrlShorter.Database;
using UrlShorter.Models;
using UrlShorter.UnitTests.Helpers;

namespace UrlShorter.UnitTests;

public class TokenServiceTests
{
    [Fact]
    public async Task GenerateTokenFor_All_Good_Returns_Token()
    {
        // arrange
        var valueGiver = new Mock<IEncryptValueGiver>();
        valueGiver.Setup(x => x.GetValueAsync()).Returns(Task.FromResult<long>(1000));
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
        valueGiver.Setup(x => x.GetValueAsync()).Throws<Exception>();
        var sut = new TokenService(valueGiver.Object,
            DbHelper.GetDbContext(nameof(GenerateTokenFor_ValueGiver_Throws_Returns_Empty_String)));

        // act
        var result = await sut.GenerateTokenFor("https://longurl.com");
        var actual = string.IsNullOrEmpty(result);
        
        // assert
        actual.Should().Be(true);
    }

    [Fact]
    public async Task GenerateTokenFor_Record_Already_Exists_Returns_Record()
    {
        // arrange
        var valueGiver = new Mock<IEncryptValueGiver>();
        valueGiver.Setup(giver => giver.GetValueAsync()).Returns(Task.FromResult<long>(100));
        var dbContext = DbHelper.GetDbContext(nameof(GenerateTokenFor_Record_Already_Exists_Returns_Record));
        var sut = new TokenService(valueGiver.Object, dbContext);

        const string longUrl = "https://long_long_url.com";
        const string token = "myToken";
        await dbContext.Links.AddAsync(new LinkModel()
        {
            LongUrl = longUrl,
            Token = token
        });
        await dbContext.SaveChangesAsync();

        // act
        var actual = await sut.GenerateTokenFor(longUrl);

        // assert
        actual.Should().Be(token);
    }
}