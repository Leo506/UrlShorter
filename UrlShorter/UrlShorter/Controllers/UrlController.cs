using Microsoft.AspNetCore.Mvc;
using UrlShorter.Core;

namespace UrlShorter.Controllers;

public class UrlController : Controller
{
    private readonly TokenService _tokenService;

    public UrlController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("/short/url")]
    public IActionResult ShortUrl([FromBody] string longUrl)
    {
        var token = _tokenService.GenerateTokenFor(longUrl);
        return Ok($"https://{HttpContext.Request.Host}/{token}");
    }
}