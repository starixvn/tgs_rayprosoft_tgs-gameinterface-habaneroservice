using Tgs.ServiceUtility;

namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

internal class FundStatusGetService : IGetService<FundStatusQuery, bool>
{
    private readonly IGameApiClient _client;

    public FundStatusGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<bool> GetAsync(FundStatusQuery command, CancellationToken cancellationToken = default)
        => _client.QueryTransferAsync(
            apiUrl: command.ApiUrl,
            brandId: command.BrandId,
            apiKey: command.ApiKey,
            username: command.Username,
            requestId: command.TransactionId,
            cancellationToken: cancellationToken);
}
