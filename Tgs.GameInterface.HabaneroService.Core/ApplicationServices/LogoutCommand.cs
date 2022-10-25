namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

public record LogoutCommand(
    Uri ApiUrl,
    string BrandId,
    string ApiKey,
    string Username,
    string Password);
