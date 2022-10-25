using Tgs.ServiceUtility;

namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

internal class FundInGetService : IGetService<FundInQuery, FundResult>
{
    private readonly IGameApiClient _client;

    public FundInGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client
            .WithdrawPlayerMoneyAsync(
                apiUrl: query.ApiUrl,
                brandId: query.BrandId,
                apiKey: query.ApiKey,
                username: query.Username,
                password: query.Password,
                currencyCode: query.CurrencyCode,
                amount: query.Amount,
                requestId: query.TransactionId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new FundResult(
            TransactionId: result.TransactionId,
            Balance: result.Balance);
    }
}
