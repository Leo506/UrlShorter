using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using UrlShorter.Core;
using UrlShorter.Core.Abstractions;
using UrlShorter.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(optionsBuilder => optionsBuilder.UseInMemoryDatabase(nameof(AppDbContext)));
builder.Services.AddSingleton<IEncryptValueGiver, RedisEncryptValueGiver>()
    .AddScoped<TokenService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
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