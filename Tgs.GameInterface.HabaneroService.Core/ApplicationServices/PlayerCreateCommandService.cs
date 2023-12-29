using Tgs.ServiceUtility;

namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

internal class PlayerCreateCommandService(
    IGameApiClient client) : ICommandService<PlayerCreateCommand>
{
    public async ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
    {
        _ = await client.LoginOrCreatePlayerAsync(
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
