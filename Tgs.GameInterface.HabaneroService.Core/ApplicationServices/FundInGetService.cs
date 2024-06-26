using Tgs.ServiceUtility;

namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

internal class FundInGetService(IGameApiClient client) : IGetService<FundInQuery, FundResult>
{
	public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client
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
			Balance: result.Balance,
			Resp: result.Resp);
	}
}
