using Tgs.ServiceUtility;

namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

internal class FundOutGetService(IGameApiClient client) : IGetService<FundOutQuery, FundResult>
{
    public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
    {
        var result = await client
            .DepositPlayerMoneyAsync(
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
