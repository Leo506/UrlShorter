using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UrlShorter.Controllers;

namespace UrlShorter.UnitTests.Helpers;

public static class UrlControllerHelper
{
    public static UrlController GetController()
    {
        var sut = new UrlController(TokenServiceHelper.GetService());
        var context = new Mock<HttpContext>();
        context.Setup(c => c.Request.Host).Returns(new HostString("localhost", 443));
        sut.ControllerContext = new ControllerContext()
        {
            HttpContext = context.Object
        };

        return sut;
    }
}