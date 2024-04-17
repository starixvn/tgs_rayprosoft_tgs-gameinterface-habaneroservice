namespace Tgs.GameInterface.HabaneroService.ViewModels;

public record PlayerCreateViewModel(
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
