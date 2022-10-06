namespace UrlShorter.Core.Settings;

public class RedisSettings
{
    public string CounterKey { get; set; } = null!;
    
    public long Step { get; set; }
}