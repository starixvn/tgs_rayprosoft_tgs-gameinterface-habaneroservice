using Tgs.ServiceUtility;

namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

internal class GetTokenQueryService(IGameApiClient client) : IGetService<GetTokenQuery, GetTokenResult>
{
	public async ValueTask<GetTokenResult> GetAsync(GetTokenQuery query, CancellationToken cancellationToken = default)
	{
		var token = await client.LoginOrCreatePlayerAsync(
		   apiUrl: query.ApiUrl,
		   brandId: query.BrandId,
		   apiKey: query.ApiKey,
		   username: query.Username,
		   password: query.Password,
		   currencyCode: query.CurrencyCode,
		   useragent: query.UserAgent,
		   playerHostAddress: query.PlayerHostAddress,
		   firstName: query.FirstName,
		   lastName: query.LastName,
		   cancellationToken).ConfigureAwait(false);

		return new GetTokenResult(token);
	}
}
