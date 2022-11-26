using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.HabaneroService.Core.Models;

namespace Tgs.GameInterface.HabaneroService.Core;

internal class GameApiClient : IGameApiClient
{
    private readonly HttpClient _client;
    private readonly ILogger<GameApiClient> _logger;

    public GameApiClient(
        HttpClient client,
        ILogger<GameApiClient> logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask<DepositPlayerMoneyResult> DepositPlayerMoneyAsync(Uri apiUrl, string brandId, string apiKey, string username, string password, string currencyCode, decimal amount, string requestId, CancellationToken cancellationToken = default)
    {
        var request = new DepositPlayerMoneyRequest
        {
            BrandId = brandId,
            APIKey = apiKey,
            Username = username,
            Password = password,
            CurrencyCode = currencyCode,
            Amount = amount,
            RequestId = requestId
        };

        var url = new Uri(apiUrl, "/jsonapi/DepositPlayerMoney");
        var document = await PostAsync(
            url,
            request,
            "DepositPlayerMoney",
            cancellationToken).ConfigureAwait(false);

        if (document is null)
        {
            throw new Exception("Document is empty.");
        }

        if (!document.RootElement.TryGetProperty("Success", out var jsonSuccess))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var success = jsonSuccess.GetBoolean();

        if (success is false)
        {
            throw new Exception($"DepositPlayerMoney failed.({JsonSerializer.Serialize(document)})");
        }

        var jsonTransactionId = document.RootElement.GetProperty("TransactionId");
        var jsonRealBalance = document.RootElement.GetProperty("RealBalance");

        var transactionId = jsonTransactionId.ToString();
        var realBalance = jsonRealBalance.GetDecimal();

        return new DepositPlayerMoneyResult(transactionId, realBalance);
    }

    public async ValueTask<WithdrawPlayerMoneyResult> WithdrawPlayerMoneyAsync(Uri apiUrl, string brandId, string apiKey, string username, string password, string currencyCode, decimal amount, string requestId, CancellationToken cancellationToken = default)
    {
        var request = new WithdrawPlayerMoneyRequest
        {
            BrandId = brandId,
            APIKey = apiKey,
            Username = username,
            Password = password,
            CurrencyCode = currencyCode,
            Amount = -amount,
            WithdrawAll = false,
            RequestId = requestId
        };

        var url = new Uri(apiUrl, "/jsonapi/WithdrawPlayerMoney");
        var document = await PostAsync(
            url,
            request,
            "WithdrawPlayerMoney",
            cancellationToken).ConfigureAwait(false);

        if (document is null)
        {
            throw new Exception("Document is empty.");
        }

        if (!document.RootElement.TryGetProperty("Success", out var jsonSuccess))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var success = jsonSuccess.GetBoolean();

        if (success is false)
        {
            throw new Exception($"WithdrawPlayerMoney failed.({JsonSerializer.Serialize(document)})");
        }

        var jsonTransactionId = document.RootElement.GetProperty("TransactionId");
        var jsonRealBalance = document.RootElement.GetProperty("RealBalance");

        var transactionId = jsonTransactionId.ToString();
        var realBalance = jsonRealBalance.GetDecimal();

        return new WithdrawPlayerMoneyResult(transactionId, realBalance);
    }

    public async ValueTask<LoginOrCreatePlayerResult> LoginOrCreatePlayerAsync(
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
        CancellationToken cancellationToken = default)
    {
        var request = new LoginOrCreatePlayerRequest
        {
            BrandId = brandId,
            APIKey = apiKey,
            PlayerHostAddress = playerHostAddress,
            UserAgent = useragent,
            KeepExistingToken = true,
            Username = username,
            Password = password,
            CurrencyCode = currencyCode,
            FirstName = firstName,
            LastName = lastName,
        };

        var url = new Uri(apiUrl, "/jsonapi/loginorcreateplayer");
        var document = await PostAsync(
            url,
            request,
            "LoginOrCreatePlayer",
            cancellationToken).ConfigureAwait(false);

        if (document is null)
        {
            throw new Exception("Document is empty.");
        }

        if (!document.RootElement.TryGetProperty("Token", out var jsonToken))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var token = jsonToken.ToString();

        return new LoginOrCreatePlayerResult(token);
    }

    public async ValueTask<bool> LogOutPlayerAsync(Uri apiUrl, string brandId, string apiKey, string username, string password, CancellationToken cancellationToken = default)
    {
        var request = new LogOutPlayerRequest
        {
            BrandId = brandId,
            APIKey = apiKey,
            Username = username,
            Password = password,
        };

        var url = new Uri(apiUrl, "/jsonapi/logoutplayer");
        var document = await PostAsync(
            url,
            request,
            "LogOutPlayer",
            cancellationToken).ConfigureAwait(false);

        if (document is null)
        {
            throw new Exception("Document is empty.");
        }

        if (!document.RootElement.TryGetProperty("Success", out var jsonSuccess))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var success = jsonSuccess.GetBoolean();

        return success;
    }

    public async ValueTask<QueryPlayerResult> QueryPlayerAsync(Uri apiUrl, string brandId, string apiKey, string username, string password, CancellationToken cancellationToken = default)
    {
        var request = new QueryPlayerRequest
        {
            BrandId = brandId,
            APIKey = apiKey,
            Username = username,
            Password = password,
        };

        var url = new Uri(apiUrl, "/jsonapi/QueryPlayer");
        var document = await PostAsync(
            url,
            request,
            "QueryPlayer",
            cancellationToken).ConfigureAwait(false);

        if (document is null)
        {
            throw new Exception("Document is empty.");
        }

        if (!document.RootElement.TryGetProperty("Found", out var jsonFound))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var found = jsonFound.GetBoolean();

        if (!found)
        {
            return new QueryPlayerResult(
                        IsPlayerExist: found,
                        Balance: 0);
        }

        var jsonRealBalance = document.RootElement.GetProperty("RealBalance");

        var realBalance = jsonRealBalance.GetDecimal();

        return new QueryPlayerResult(
            IsPlayerExist: found,
            Balance: realBalance);
    }

    public async IAsyncEnumerable<GetBrandCompletedGameResultsV2Result> GetBrandCompletedGameResultsV2Async(
        Uri apiUrl,
        string brandId,
        string apiKey,
        DateTime startTime,
        DateTime endTime,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GetBrandCompletedGameResultsV2Request
        {
            BrandId = brandId,
            APIKey = apiKey,
            DtStartUTC = startTime.ToString("yyyyMMddHHmmss"),
            DtEndUTC = endTime.ToString("yyyyMMddHHmmss")
        };

        var url = new Uri(apiUrl, "/jsonapi/GetBrandCompletedGameResultsV2");
        var document = await PostAsync(
            url,
            request,
            "GetBrandCompletedGameResultsV2",
            cancellationToken).ConfigureAwait(false);

        if (document is null)
        {
            throw new Exception("Document is empty.");
        }

        var results = JsonSerializer.Deserialize<IEnumerable<GetBrandCompletedGameResultsV2Result>>(document)
            ?? Enumerable.Empty<GetBrandCompletedGameResultsV2Result>();

        if (!results.Any())
        {
            yield break;
        }

        foreach (var result in results)
        {
            yield return result;
        }
    }

    public async ValueTask<bool> QueryTransferAsync(Uri apiUrl, string brandId, string apiKey, string username, string requestId, CancellationToken cancellationToken = default)
    {
        var request = new QueryTransferRequest
        {
            BrandId = brandId,
            APIKey = apiKey,
            Username = username,
            RequestId = requestId,
        };

        var url = new Uri(apiUrl, "/jsonapi/QueryTransfer");
        var document = await PostAsync(
            url,
            request,
            "QueryTransfer",
            cancellationToken).ConfigureAwait(false);

        if (document is null)
        {
            throw new Exception("Document is empty.");
        }

        if (!document.RootElement.TryGetProperty("Success", out var jsonSuccess))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var success = jsonSuccess.GetBoolean();

        return success;
    }

    private async ValueTask<JsonDocument> PostAsync(Uri apiUrl, object request, string logPrefix, CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var sw = Stopwatch.StartNew();

        HttpContent content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8,
            "application/json");

        var httpResponseMessage = await _client.PostAsync(apiUrl, content, cancellationToken);

        sw.Stop();

        var responseMessage = default(string);

        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr.ReadToEndAsync().ConfigureAwait(false) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = (await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };

        _logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        _ = httpResponseMessage.EnsureSuccessStatusCode();

        return JsonDocument.Parse(responseMessage);
    }
}
