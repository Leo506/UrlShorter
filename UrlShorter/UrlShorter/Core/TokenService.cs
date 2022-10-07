using HashidsNet;
using Microsoft.EntityFrameworkCore;
using UrlShorter.Core.Abstractions;
using UrlShorter.Database;
using UrlShorter.Models;

namespace UrlShorter.Core;

public class TokenService
{
    private readonly IEncryptValueGiver _valueGiver;
    private readonly AppDbContext _dbContext;

    public TokenService(IEncryptValueGiver valueGiver, AppDbContext dbContext)
    {
        _valueGiver = valueGiver;
        _dbContext = dbContext;
    }

    public async Task<string> GenerateTokenFor(string longUrl)
    {
        var hashids = new Hashids("this is my salt");
        try
        {
            var candidate = await _dbContext.Links.FirstOrDefaultAsync(x => x.LongUrl == longUrl);
            if (candidate != null)
                return candidate.Token;
            
            var id = await _valueGiver.GetValueAsync();
            var token = hashids.EncodeLong(id);

            await _dbContext.Links.AddAsync(new LinkModel()
            {
                LongUrl = longUrl,
                Token = token
            });
            await _dbContext.SaveChangesAsync();
            return token;
        }
        catch (Exception)
        {
        }

        return string.Empty;
    }
}