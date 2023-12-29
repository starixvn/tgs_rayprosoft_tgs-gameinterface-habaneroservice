using Tgs.ServiceUtility;

namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

internal class PlayerBalanceGetService(IGameApiClient client) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
    {
        var result = await client.QueryPlayerAsync(
            apiUrl: query.ApiUrl,
            brandId: query.BrandId,
            apiKey: query.ApiKey,
            username: query.Username,
            password: query.Password,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return new PlayerBalanceResult(
             IsPlayerExist: result.IsPlayerExist,
             Balance: result.Balance);
    }
}
