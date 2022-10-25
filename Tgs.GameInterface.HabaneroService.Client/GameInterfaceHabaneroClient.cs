using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.HabaneroService.Protos;

namespace Tgs.GameInterface.HabaneroService.Client;

internal class GameInterfaceHabaneroClient : IGameInterfaceHabaneroClient
{
    private readonly GameInterfaceHabaneroService.GameInterfaceHabaneroServiceClient _client;

    public GameInterfaceHabaneroClient(GameInterfaceHabaneroService.GameInterfaceHabaneroServiceClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundReply> FundInAsync(
        Uri apiUrl,
        string brandId,
        string apiKey,
        string username,
        string password,
        string currencyCode,
        decimal amount,
        string transactionId,
        CancellationToken cancellationToken = default)
    {
        var request = new FundRequest
        {
            ApiUrl = apiUrl.ToString(),
            BrandId = brandId,
            ApiKey = apiKey,
            Username = username,
            Password = password,
            CurrencyCode = currencyCode,
            Amount = amount,
            TransactionId = transactionId,
        };

        return await _client
            .FundInAsync(request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<FundReply> FundOutAsync(
        Uri apiUrl,
        string brandId,
        string apiKey,
        string username,
        string password,
        string currencyCode,
        decimal amount,
        string transactionId,
        CancellationToken cancellationToken = default)
    {
        var request = new FundRequest
        {
            ApiUrl = apiUrl.ToString(),
            BrandId = brandId,
            ApiKey = apiKey,
            Username = username,
            Password = password,
            CurrencyCode = currencyCode,
            Amount = amount,
            TransactionId = transactionId,
        };

        return await _client
            .FundOutAsync(request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<FundStatusReply> FundStatusAsync(
        Uri apiUrl,
        string brandId,
        string apiKey,
        string username,
        string transactionId,
        CancellationToken cancellationToken = default)
    {
        var request = new FundStatusRequest
        {
            ApiUrl = apiUrl.ToString(),
            BrandId = brandId,
            ApiKey = apiKey,
            Username = username,
            TransactionId = transactionId,
        };

        return await _client
            .FundStatusAsync(request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async IAsyncEnumerable<GameHistorySettled> GameHistorySettledAsync(
        Uri apiUrl,
        string brandId,
        string apiKey,
        DateTime startTime,
        DateTime endTime,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GameHistorySettledRequest
        {
            ApiUrl = apiUrl.ToString(),
            BrandId = brandId,
            ApiKey = apiKey,
            StartTime = startTime.ToTimestamp(),
            EndTime = endTime.ToTimestamp()
        };

        var reply = await _client
            .GameHistorySettledAsync(request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!reply.Items.Any())
        {
            yield break;
        }

        foreach (var item in reply.Items)
        {
            yield return item;
        }
    }

    public async ValueTask<GameTokenReply> GameTokenAsync(
        Uri apiUrl,
        string brandId,
        string apiKey,
        string playerHostAddress,
        string useragent,
        string username,
        string password,
        string currencyCode,
        CancellationToken cancellationToken = default)
    {
        var request = new GameTokenRequest
        {
            ApiUrl = apiUrl.ToString(),
            BrandId = brandId,
            ApiKey = apiKey,
            PlayerHostAddress = playerHostAddress,
            UserAgent = useragent,
            Username = username,
            Password = password,
            CurrencyCode = currencyCode,
        };
        return await _client
            .GameTokenAsync(request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string brandId,
        string apiKey,
        string username,
        string password,
        CancellationToken cancellationToken = default)
    {
        var request = new PlayerBalanceRequest
        {
            ApiUrl = apiUrl.ToString(),
            BrandId = brandId,
            ApiKey = apiKey,
            Username = username,
            Password = password,
        };
        return await _client
            .PlayerBalanceAsync(request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask PlayerCreateAsync(
        Uri apiUrl,
        string brandId,
        string apiKey,
        string playerHostAddress,
        string useragent,
        string username,
        string password,
        string currencyCode,
        CancellationToken cancellationToken = default)
    {
        var request = new PlayerCreateRequest
        {
            ApiUrl = apiUrl.ToString(),
            BrandId = brandId,
            ApiKey = apiKey,
            PlayerHostAddress = playerHostAddress,
            UserAgent = useragent,
            Username = username,
            Password = password,
            CurrencyCode = currencyCode,
        };
        _ = await _client
            .PlayerCreateAsync(request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
