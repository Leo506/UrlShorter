using Microsoft.EntityFrameworkCore;
using UrlShorter.Database;

namespace UrlShorter.UnitTests.Helpers;

public static class DbHelper
{
    public static AppDbContext GetDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName);

        return new AppDbContext(options.Options);
    }
}