namespace Tgs.GameInterface.HabaneroService.ViewModels;

public record KickPlayerViewModel(
    Uri ApiUrl,
    string BrandId,
    string ApiKey,
    string Username,
    string Password);
