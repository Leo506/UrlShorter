using FluentAssertions;
using Moq;
using UrlShorter.Core;
using UrlShorter.Core.Abstractions;

namespace UrlShorter.UnitTests;

public class RedisEncryptValueGiverTests
{
    [Fact]
    public async Task GetValue_First_Call_Redis_Service_Invoked()
    {
        // arrange
        var redisService = new Mock<IRedisService>();
        redisService.Setup(service => service.GetCounter()).Returns(Task.FromResult<long>(0));
        var sut = new RedisEncryptValueGiver(redisService.Object);

        // act
        await sut.GetValueAsync();

        // assert
        redisService.Invocations.Count.Should().Be(1);
    }

    [Fact]
    public async Task GetValue_Value_In_Range_Redis_Service_Not_Invoked()
    {
        // arrange
        var redisService = new Mock<IRedisService>();
        redisService.Setup(service => service.GetCounter()).Returns(Task.FromResult<long>(10));
        var sut = new RedisEncryptValueGiver(redisService.Object);
        await sut.GetValueAsync();

        // act
        await sut.GetValueAsync();

        // assert
        redisService.Invocations.Count.Should().Be(1);
    }
}