namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

public record PlayerBalanceResult(
    bool IsPlayerExist,
    decimal? Balance);
