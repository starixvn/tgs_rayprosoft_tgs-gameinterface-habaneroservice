using Tgs.ServiceUtility;

namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

internal class PlayerCreateCommandService : ICommandService<PlayerCreateCommand>
{
    private readonly IGameApiClient _client;

    public PlayerCreateCommandService(
        IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
    {
        _ = await _client.LoginOrCreatePlayerAsync(
           apiUrl: command.ApiUrl,
           brandId: command.BrandId,
           apiKey: command.ApiKey,
           username: command.Username,
           password: command.Password,
           currencyCode: command.CurrencyCode,
           useragent: command.UserAgent,
           playerHostAddress: command.PlayerHostAddress,
           firstName: command.FirstName,
           lastName: command.LastName,
           cancellationToken).ConfigureAwait(false);
    }
}
