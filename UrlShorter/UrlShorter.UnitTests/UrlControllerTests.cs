using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using UrlShorter.Controllers;
using UrlShorter.Core;
using UrlShorter.UnitTests.Helpers;

namespace UrlShorter.UnitTests;

public class UrlControllerTests
{
    [Fact]
    public async Task ShortUrl_All_Good_Returns_Url()
    {
        // arrange
        var sut = UrlControllerHelper.GetController();

        // act
        var response = await sut.ShortUrl("https://very_long_my_url/to_something");
        var actual = (response as OkObjectResult)?.Value as string;

        // assert
        actual.Should().NotBeNull();
    }

    [Fact]
    public async Task ShortUrl_Url_Is_Empty_Returns_Bad_Request()
    {
        // arrange
        var sut = UrlControllerHelper.GetController();

        // act
        var response = await sut.ShortUrl(string.Empty);
        var statusCode = (response as IStatusCodeActionResult)?.StatusCode;
        
        // assert
        statusCode.Should().NotBeNull();
        statusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task LongUrl_All_Good_Returns_Redirect()
    {
        // arrange
        var sut = UrlControllerHelper.GetController();
        const string longUrl = "https://very_long_my_url/to_something";
        var shortUrlResponse = await sut.ShortUrl(longUrl);
        var shortUrl = (shortUrlResponse as OkObjectResult)?.Value as string;
        var token = shortUrl![(shortUrl!.LastIndexOf('/') + 1)..];

        // act
        var longUrlResponse = await sut.LongUrl(token);
        var actual = (longUrlResponse as RedirectResult)?.Url;

        // assert
        actual.Should().NotBeNull();
        actual.Should().Be(longUrl);
    }

    [Fact]
    public async Task LongUrl_No_Token_Returns_Bad_Request()
    {
        // arrange
        var sut = UrlControllerHelper.GetController();
        const string longUrl = "https://very_long_my_url/to_something";
        var shortUrlResponse = await sut.ShortUrl(longUrl);
        var token = "not_existing_token";

        // act
        var longUrlResponse = await sut.LongUrl(token);
        var actual = (longUrlResponse as IStatusCodeActionResult)!.StatusCode;

        // assert
        actual.Should().NotBeNull();
        actual.Should().Be((int)HttpStatusCode.BadRequest);
    }
}