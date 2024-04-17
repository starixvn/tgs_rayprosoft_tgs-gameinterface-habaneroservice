namespace Tgs.GameInterface.HabaneroService.ViewModels;

public record PlayerBalanceViewModel(
	Uri ApiUrl,
	string BrandId,
	string ApiKey,
	string Username,
	string Password);
