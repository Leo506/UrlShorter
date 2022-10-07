using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShorter.Core;
using UrlShorter.Database;

namespace UrlShorter.Controllers;

public class UrlController : Controller
{
    private readonly TokenService _tokenService;

    public UrlController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("/short/url")]
    public async Task<ActionResult> ShortUrl([FromBody] string longUrl)
    {
        if (string.IsNullOrEmpty(longUrl))
            return BadRequest();
        var token = await _tokenService.GenerateTokenFor(longUrl);
        return Ok($"https://{HttpContext.Request.Host}/long/{token}");
    }

    [HttpGet("/long/{token}")]
    public async Task<IActionResult> LongUrl(string token)
    {
        var tokenResult = await _tokenService.GetByToken(token);
        if (tokenResult is null)
            return BadRequest();
        return Redirect(tokenResult);
    }
}