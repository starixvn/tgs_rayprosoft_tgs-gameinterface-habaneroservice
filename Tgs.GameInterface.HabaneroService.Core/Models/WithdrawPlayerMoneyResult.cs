namespace Tgs.GameInterface.HabaneroService.Core.Models;

public record WithdrawPlayerMoneyResult(
    string TransactionId,
    decimal Balance);
