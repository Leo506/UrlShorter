using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using UrlShorter.Core;
using UrlShorter.Core.Abstractions;
using UrlShorter.Core.Settings;
using UrlShorter.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddDbContext<AppDbContext>(optionsBuilder => optionsBuilder.UseInMemoryDatabase(nameof(AppDbContext)))
    .AddSingleton(provider => builder.Configuration.GetSection("RedisSettings").Get<RedisSettings>())
    .AddSingleton<IEncryptValueGiver, RedisEncryptValueGiver>()
    .AddScoped<TokenService>()
    .AddSingleton<IConnectionMultiplexer>(provider =>
    {
        var connString = builder.Configuration.GetConnectionString("redis");
        return ConnectionMultiplexer.Connect(connString);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();