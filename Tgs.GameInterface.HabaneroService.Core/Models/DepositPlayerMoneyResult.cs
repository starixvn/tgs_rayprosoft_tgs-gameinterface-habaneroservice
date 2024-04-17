namespace Tgs.GameInterface.HabaneroService.Core.Models;

public record DepositPlayerMoneyResult(
	string TransactionId,
	decimal Balance);
