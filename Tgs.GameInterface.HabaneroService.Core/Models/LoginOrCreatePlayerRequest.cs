namespace Tgs.GameInterface.HabaneroService.Core.Models;

public record LoginOrCreatePlayerRequest
{
    public string BrandId { get; set; } = default!;

    public string APIKey { get; set; } = default!;

    /// <summary>
    /// Player IP Address (PLEASE PROVIDE)
    /// </summary>
    public string PlayerHostAddress { get; set; } = default!;

    /// <summary>
    /// Send the players browser useragent string so we can display the Device in reports.
    /// </summary>
    public string UserAgent { get; set; } = default!;

    /// <summary>
    /// Default True : If the player is already logged in, do not create a new session token.Set to False if you want to invalidate other sessions
    /// </summary>
    public bool KeepExistingToken { get; set; } = true;

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string CurrencyCode { get; set; } = default!;

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
