using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.HabaneroService.Core.UnitTest;

[TestClass]
public class QueryPlayerUnitTest
{
	[TestMethod]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://ws-test.insvr.com/jsonapi/QueryPlayer");

		var response =
			"{" +
			"\"Found\": true," +
			"\"PlayerId\": \"d69ba962-3b50-ed11-ade6-0050f238e028\"," +
			"\"BrandId\": \"b97e1834-d142-ed11-a27c-0050f23876f7\"," +
			"\"BrandName\": \"Pega Gaming\"," +
			"\"RealBalance\": 50.0000," +
			"\"CurrencyCode\": \"RMB\"," +
			"\"CurrencySymbol\": \"¥\"," +
			"\"HasBonus\": false," +
			"\"BonusBalance\": 0.0," +
			"\"BonusSpins\": 0," +
			"\"BonusGameId\": 0," +
			"\"BonusPercentage\": 0.0," +
			"\"BonusWagerRemaining\": 0.0" +
			"}";

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, apiUrl.ToString())
			.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);
		var actual = await sut.QueryPlayerAsync(
				apiUrl: apiUrl,
				brandId: "b97e1834-d142-ed11-a27c-0050f23876f7",
				apiKey: "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
				username: "4ucmqplxb029",
				password: "HabaTfP@ssw0rd")
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.IsTrue(actual.IsPlayerExist);
		Assert.AreEqual(50.0000M, actual.Balance);
	}
}
