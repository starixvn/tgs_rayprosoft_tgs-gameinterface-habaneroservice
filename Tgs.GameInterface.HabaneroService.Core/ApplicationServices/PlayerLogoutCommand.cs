namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

public record PlayerLogoutCommand(
	Uri ApiUrl,
	string BrandId,
	string ApiKey,
	string Username,
	string Password);
