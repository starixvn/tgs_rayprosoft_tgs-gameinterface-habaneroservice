using Tgs.ServiceUtility;

namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

internal class PlayerLogoutCommandService(
	IGameApiClient client) : ICommandService<PlayerLogoutCommand>
{
	public ValueTask ExecuteAsync(PlayerLogoutCommand command, CancellationToken cancellationToken = default)
		=> client.LogOutPlayerAsync(
			apiUrl: command.ApiUrl,
			brandId: command.BrandId,
			apiKey: command.ApiKey,
			username: command.Username,
			password: command.Password,
			cancellationToken);
}
