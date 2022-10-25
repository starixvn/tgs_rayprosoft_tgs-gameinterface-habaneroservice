using System.Runtime.CompilerServices;
using System.Text.Json;
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
            decimal playerWinLoss = bettingHistoryBet.Payout - bettingHistoryBet.Stake;

            yield return new GameHistorySettledResult(
                PlayerCode: bettingHistoryBet.Username,
                ExternalRoundId: bettingHistoryBet.FriendlyGameInstanceId.ToString(),
                ExternalGameId: bettingHistoryBet.GameKeyName,
                ExternalCreatedOn: bettingHistoryBet.DtStarted,
                ExternalClosedOn: bettingHistoryBet.DtCompleted,
                Bet: bettingHistoryBet.Stake,
                ValidBet: bettingHistoryBet.Stake,
                PlayerWinLoss: playerWinLoss,
                Turnover: bettingHistoryBet.Stake,
                Settle: bettingHistoryBet.Payout);
        }
    }
}
