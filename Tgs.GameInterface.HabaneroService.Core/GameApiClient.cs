using System.Diagnostics;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.HabaneroService.Core.Models;

namespace Tgs.GameInterface.HabaneroService.Core;

internal class GameApiClient(
	HttpClient client,
	ILogger<GameApiClient> logger) : IGameApiClient
{
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
		var jsonNode = await PostAsync(
			apiUrl: url,
			request: request,
			logPrefix: nameof(DepositPlayerMoneyAsync),
			cancellationToken: cancellationToken).ConfigureAwait(false)
			?? throw new Exception("Document is empty.");

		if (jsonNode["Success"]!.GetValue<bool>() is false)
		{
			throw new Exception($"DepositPlayerMoney failed.({JsonSerializer.Serialize(jsonNode)})");
		}

		var transactionId = jsonNode["TransactionId"]!.GetValue<string>();
		var realBalance = jsonNode["RealBalance"]!.GetValue<decimal>();

		return new DepositPlayerMoneyResult(transactionId, realBalance, jsonNode.ToJsonString());
	}

	public async IAsyncEnumerable<GetBrandCompletedGameResultsV2Result> GetBrandCompletedGameResultsV2Async(Uri apiUrl, string brandId, string apiKey, DateTime startTime, DateTime endTime, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var request = new GetBrandCompletedGameResultsV2Request
		{
			BrandId = brandId,
			APIKey = apiKey,
			DtStartUTC = startTime.ToString("yyyyMMddHHmmss"),
			DtEndUTC = endTime.ToString("yyyyMMddHHmmss")
		};

		var url = new Uri(apiUrl, "/jsonapi/GetBrandCompletedGameResultsV2");
		var jsonNode = await PostAsync(
			apiUrl: url,
			request: request,
			logPrefix: nameof(GetBrandCompletedGameResultsV2Async),
			cancellationToken: cancellationToken).ConfigureAwait(false)
			?? throw new Exception("Document is empty.");

		var results = jsonNode.AsArray();

		if (results.Count == 0)
		{
			yield break;
		}

		foreach (var result in results)
		{
			if (result is null)
			{
				continue;
			}

			yield return new GetBrandCompletedGameResultsV2Result(
				PlayerCode: result["Username"]!.ToString(),
				ExternalRoundId: result["FriendlyGameInstanceId"]!.ToString(),
				ExternalGameId: result["GameKeyName"]!.ToString(),
				GameStateId: result["GameStateId"]!.GetValue<int>(),
				ExternalCreatedOn: result["DtStarted"]!.GetValue<DateTimeOffset>(),
				ExternalClosedOn: result["DtCompleted"]!.GetValue<DateTimeOffset>(),
				Bet: result["Stake"]!.GetValue<decimal>(),
				Settle: result["Payout"]!.GetValue<decimal>(),
				AdditionalData: result.ToJsonString());
		}
	}

	public async ValueTask<string> LoginOrCreatePlayerAsync(Uri apiUrl, string brandId, string apiKey, string username, string password, string currencyCode, string useragent, string playerHostAddress, string? firstName, string? lastName, CancellationToken cancellationToken = default)
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
		var jsonNode = await PostAsync(
			apiUrl: url,
			request: request,
			logPrefix: nameof(LoginOrCreatePlayerAsync),
			cancellationToken: cancellationToken).ConfigureAwait(false)
			?? throw new Exception("Document is empty.");

		return jsonNode["Token"]!.GetValue<string>();
	}

	public async ValueTask LogOutPlayerAsync(Uri apiUrl, string brandId, string apiKey, string username, string password, CancellationToken cancellationToken = default)
	{
		var request = new LogOutPlayerRequest
		{
			BrandId = brandId,
			APIKey = apiKey,
			Username = username,
			Password = password,
		};

		var url = new Uri(apiUrl, "/jsonapi/logoutplayer");
		var jsonNode = await PostAsync(
			apiUrl: url,
			request: request,
			logPrefix: nameof(LogOutPlayerAsync),
			cancellationToken: cancellationToken).ConfigureAwait(false)
			?? throw new Exception("Document is empty.");

		if (!jsonNode["Success"]!.GetValue<bool>())
		{
			throw new Exception("Player failed to log out.");
		}
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
		var jsonNode = await PostAsync(
			apiUrl: url,
			request: request,
			logPrefix: nameof(QueryPlayerAsync),
			cancellationToken: cancellationToken).ConfigureAwait(false)
			?? throw new Exception("Document is empty.");

		return !jsonNode["Found"]!.GetValue<bool>()
			? new QueryPlayerResult(
				IsPlayerExist: false,
				Balance: 0)
			: new QueryPlayerResult(
				IsPlayerExist: true,
				Balance: jsonNode["RealBalance"]!.GetValue<decimal>());
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
		var jsonNode = await PostAsync(
			apiUrl: url,
			request: request,
			logPrefix: nameof(QueryTransferAsync),
			cancellationToken: cancellationToken).ConfigureAwait(false)
			?? throw new Exception("Document is empty.");

		return jsonNode["Success"]!.GetValue<bool>();
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
		var jsonNode = await PostAsync(
			apiUrl: url,
			request: request,
			logPrefix: nameof(WithdrawPlayerMoneyAsync),
			cancellationToken: cancellationToken).ConfigureAwait(false)
			?? throw new Exception("Document is empty.");

		if (jsonNode["Success"]!.GetValue<bool>() is false)
		{
			throw new Exception($"WithdrawPlayerMoney failed.({JsonSerializer.Serialize(jsonNode)})");
		}

		var transactionId = jsonNode["TransactionId"]!.GetValue<string>();
		var realBalance = jsonNode["RealBalance"]!.GetValue<decimal>();

		return new WithdrawPlayerMoneyResult(transactionId, realBalance, jsonNode.ToJsonString());
	}

	private async ValueTask<JsonNode> PostAsync(Uri apiUrl, object request, string logPrefix, CancellationToken cancellationToken = default)
	{
		var sw = Stopwatch.StartNew();

		HttpContent content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);

		var httpResponseMessage = await client.PostAsync(apiUrl, content, cancellationToken).ConfigureAwait(false);

		sw.Stop();

		var responseMessage = default(string);

		using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
		{
			responseMessage = await sr
				.ReadToEndAsync(
				cancellationToken: cancellationToken)
				.ConfigureAwait(false) ?? string.Empty;
		}

		var apiLogEvent = new
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

		logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

		_ = httpResponseMessage.EnsureSuccessStatusCode();

		return JsonNode.Parse(responseMessage)!;
	}
}
