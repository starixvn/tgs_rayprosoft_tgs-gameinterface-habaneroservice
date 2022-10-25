using Tgs.ServiceUtility;

namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

internal class LogoutQueryService : IGetService<LogoutCommand, bool>
{
    private readonly IGameApiClient _client;

    public LogoutQueryService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<bool> GetAsync(LogoutCommand command, CancellationToken cancellationToken = default)
    {
        return _client.LogOutPlayerAsync(
            apiUrl: command.ApiUrl,
            brandId: command.BrandId,
            apiKey: command.ApiKey,
            username: command.Username,
            password: command.Password,
            cancellationToken);
    }
}
