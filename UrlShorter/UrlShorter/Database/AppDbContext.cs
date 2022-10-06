using Microsoft.EntityFrameworkCore;
using UrlShorter.Models;

namespace UrlShorter.Database;

public sealed class AppDbContext : DbContext
{
    public DbSet<LinkModel> Links { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
    {
        Database.EnsureCreated();
    }
}