namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

public record GameHistorySettledResult(
    string PlayerCode,
    string ExternalRoundId,
    string ExternalGameId,
    int GameStateId,
    DateTimeOffset ExternalCreatedOn,
    DateTimeOffset ExternalClosedOn,
    decimal Bet,
    decimal ValidBet,
    decimal PlayerWinLoss,
    decimal Turnover,
    decimal Settle);
