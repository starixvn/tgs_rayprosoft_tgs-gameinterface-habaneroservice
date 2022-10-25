namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

public record PlayerBalanceQuery(
    Uri ApiUrl,
    string BrandId,
    string ApiKey,
    string Username,
    string Password);
