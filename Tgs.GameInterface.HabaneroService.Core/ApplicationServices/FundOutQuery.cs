namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

public record FundOutQuery(
    Uri ApiUrl,
    string BrandId,
    string ApiKey,
    string Username,
    string Password,
    string CurrencyCode,
    decimal Amount,
    string TransactionId);
