using Tgs.ServiceUtility;

namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

internal class FundStatusGetService(IGameApiClient client) : IGetService<FundStatusQuery, bool>
{
    public ValueTask<bool> GetAsync(FundStatusQuery command, CancellationToken cancellationToken = default)
        => client.QueryTransferAsync(
            apiUrl: command.ApiUrl,
            brandId: command.BrandId,
            apiKey: command.ApiKey,
            username: command.Username,
            requestId: command.TransactionId,
            cancellationToken: cancellationToken);
}
