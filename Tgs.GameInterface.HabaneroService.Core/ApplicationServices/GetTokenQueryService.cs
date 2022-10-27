using Tgs.ServiceUtility;

namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

internal class GetTokenQueryService : IGetService<GetTokenQuery, GetTokenResult>
{
    private readonly IGameApiClient _client;

    public GetTokenQueryService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GetTokenResult> GetAsync(GetTokenQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client.LoginOrCreatePlayerAsync(
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

        return new GetTokenResult(result.Token);
    }
}
