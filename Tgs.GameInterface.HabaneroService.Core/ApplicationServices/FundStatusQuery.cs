namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

public record FundStatusQuery(
    Uri ApiUrl,
    string BrandId,
    string ApiKey,
    string Username,
    string TransactionId);
