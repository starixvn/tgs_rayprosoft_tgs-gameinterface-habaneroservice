namespace Tgs.GameInterface.HabaneroService.ViewModels;

public record FundViewModel(
	Uri ApiUrl,
	string BrandId,
	string ApiKey,
	string Username,
	string Password,
	string CurrencyCode,
	decimal Amount,
	string TransactionId);
