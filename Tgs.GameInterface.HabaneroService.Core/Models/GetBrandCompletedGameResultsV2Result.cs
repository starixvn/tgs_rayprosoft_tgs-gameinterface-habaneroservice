namespace Tgs.GameInterface.HabaneroService.Core.Models;

public record GetBrandCompletedGameResultsV2Result(
    string PlayerCode,
    string ExternalRoundId,
    string ExternalGameId,
    int GameStateId,
    DateTimeOffset ExternalCreatedOn,
    DateTimeOffset ExternalClosedOn,
    decimal Bet,
    decimal Settle,
    string AdditionalData);
