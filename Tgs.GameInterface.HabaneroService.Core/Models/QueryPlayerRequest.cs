namespace Tgs.GameInterface.HabaneroService.Core.Models;

public record QueryPlayerRequest
{
    public string BrandId { get; set; } = default!;
    public string APIKey { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}
