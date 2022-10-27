namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

public record GetTokenQuery(
    Uri ApiUrl,
    string BrandId,
    string ApiKey,
    string PlayerHostAddress,
    string UserAgent,
    string Username,
    string Password,
    string CurrencyCode,
    string? FirstName,
    string? LastName);
