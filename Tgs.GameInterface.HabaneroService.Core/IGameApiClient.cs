using Tgs.GameInterface.HabaneroService.Core.Models;

namespace Tgs.GameInterface.HabaneroService.Core;

public interface IGameApiClient
{
    ValueTask<DepositPlayerMoneyResult> DepositPlayerMoneyAsync(
        Uri apiUrl,
        string brandId,
        string apiKey,
        string username,
        string password,
        string currencyCode,
        decimal amount,
        string requestId,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GetBrandCompletedGameResultsV2Result> GetBrandCompletedGameResultsV2Async(
        Uri apiUrl,
        string brandId,
        string apiKey,
        DateTime dtStartUTC,
        DateTime dtEndUTC,
        CancellationToken cancellationToken = default);

    ValueTask<string> LoginOrCreatePlayerAsync(
        Uri apiUrl,
        string brandId,
        string apiKey,
        string username,
        string password,
        string currencyCode,
        string useragent,
        string playerHostAddress,
        string? firstName,
        string? lastName,
        CancellationToken cancellationToken = default);

    ValueTask LogOutPlayerAsync(
        Uri apiUrl,
        string brandId,
        string apiKey,
        string username,
        string password,
        CancellationToken cancellationToken = default);

    ValueTask<QueryPlayerResult> QueryPlayerAsync(
        Uri apiUrl,
        string brandId,
        string apiKey,
        string username,
        string password,
        CancellationToken cancellationToken = default);

    ValueTask<bool> QueryTransferAsync(
        Uri apiUrl,
        string brandId,
        string apiKey,
        string username,
        string requestId,
        CancellationToken cancellationToken = default);

    ValueTask<WithdrawPlayerMoneyResult> WithdrawPlayerMoneyAsync(
                    Uri apiUrl,
        string brandId,
        string apiKey,
        string username,
        string password,
        string currencyCode,
        decimal amount,
        string requestId,
        CancellationToken cancellationToken = default);
}
