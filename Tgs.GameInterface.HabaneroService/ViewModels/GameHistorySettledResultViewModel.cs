namespace Tgs.GameInterface.HabaneroService.ViewModels;

public record GameHistorySettledResultViewModel(
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
	decimal Settle,
	string AdditionalData);
