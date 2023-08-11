using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

internal class GameHistorySettledQueryService : IQueryService<GameHistorySettledQuery, GameHistorySettledResult>
{
    private readonly IGameApiClient _client;

    public GameHistorySettledQueryService(
        IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameHistorySettledResult> QueryAsync(GameHistorySettledQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var bettingHistoryBets = await _client
                .GetBrandCompletedGameResultsV2Async(
                    apiUrl: query.ApiUrl,
                    brandId: query.BrandId,
                    apiKey: query.ApiKey,
                    dtStartUTC: query.StartTime,
                    dtEndUTC: query.EndTime,
                    cancellationToken)
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);

        if (!bettingHistoryBets.Any())
        {
            yield break;
        }

        foreach (var bettingHistoryBet in bettingHistoryBets)
        {
            decimal playerWinLoss = bettingHistoryBet.Settle - bettingHistoryBet.Bet;

            yield return new GameHistorySettledResult(
                PlayerCode: bettingHistoryBet.PlayerCode,
                ExternalRoundId: bettingHistoryBet.ExternalRoundId.ToString(),
                ExternalGameId: bettingHistoryBet.ExternalGameId,
                GameStateId: bettingHistoryBet.GameStateId,
                ExternalCreatedOn: bettingHistoryBet.ExternalCreatedOn,
                ExternalClosedOn: bettingHistoryBet.ExternalClosedOn,
                Bet: bettingHistoryBet.Bet,
                ValidBet: bettingHistoryBet.Bet,
                PlayerWinLoss: playerWinLoss,
                Turnover: bettingHistoryBet.Bet,
                Settle: bettingHistoryBet.Settle,
                AdditionalData: bettingHistoryBet.AdditionalData);
        }
    }
}
