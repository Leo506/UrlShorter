namespace UrlShorter.Models;

public class LinkModel
{
    public int Id { get; set; }

    public string LongUrl { get; set; } = null!;

    public string Token { get; set; } = null!;
}