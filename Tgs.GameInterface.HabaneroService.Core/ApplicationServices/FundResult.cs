namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

public record FundResult(
	string TransactionId,
	decimal Balance,
	string Resp);
