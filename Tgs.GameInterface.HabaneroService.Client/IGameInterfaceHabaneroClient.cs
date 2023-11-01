using Tgs.GameInterface.HabaneroService.Protos;

namespace Tgs.GameInterface.HabaneroService.Client;

public interface IGameInterfaceHabaneroClient
{
    ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundInAsync(Uri apiUrl, string brandId, string apiKey, string username, string password, string currencyCode, decimal amount, string transactionId, CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundOutAsync(Uri apiUrl, string brandId, string apiKey, string username, string password, string currencyCode, decimal amount, string transactionId, CancellationToken cancellationToken = default);

    ValueTask<FundStatusReply> FundStatusAsync(Uri apiUrl, string brandId, string apiKey, string username, string transactionId, CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameHistorySettled> GameHistorySettledAsync(Uri apiUrl, string brandId, string apiKey, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default);

    ValueTask<GameTokenReply> GameTokenAsync(Uri apiUrl, string brandId, string apiKey, string playerHostAddress, string useragent, string username, string password, string currencyCode, string? firstName, string? lastName, CancellationToken cancellationToken = default);

    ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string brandId, string apiKey, string username, string password, CancellationToken cancellationToken = default);

    ValueTask PlayerCreateAsync(Uri apiUrl, string brandId, string apiKey, string playerHostAddress, string useragent, string username, string password, string currencyCode, string? firstName, string? lastName, CancellationToken cancellationToken = default);

    ValueTask PlayerLogoutAsync(Uri apiUrl, string brandId, string apiKey, string username, string password, CancellationToken cancellationToken = default);
}
