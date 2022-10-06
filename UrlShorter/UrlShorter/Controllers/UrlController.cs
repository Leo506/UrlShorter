using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShorter.Core;
using UrlShorter.Database;

namespace UrlShorter.Controllers;

public class UrlController : Controller
{
    private readonly TokenService _tokenService;
    private readonly AppDbContext _dbContext;

    public UrlController(TokenService tokenService, AppDbContext dbContext)
    {
        _tokenService = tokenService;
        _dbContext = dbContext;
    }

    [HttpPost("/short/url")]
    public async Task<IActionResult> ShortUrl([FromBody] string longUrl)
    {
        var token = await _tokenService.GenerateTokenFor(longUrl);
        return Ok($"https://{HttpContext.Request.Host}/long/{token}");
    }

    [HttpGet("/long/{token}")]
    public async Task<IActionResult> LongUrl(string token)
    {
        var link = await _dbContext.Links.FirstOrDefaultAsync(x => x.Token == token);
        var longUrl = link.LongUrl;
        return Redirect(longUrl);
    }
}