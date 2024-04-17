namespace Tgs.GameInterface.HabaneroService.ViewModels;

public record FundStatusViewModel(
	Uri ApiUrl,
	string BrandId,
	string ApiKey,
	string Username,
	string TransactionId);
