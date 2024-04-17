using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.HabaneroService.Core.UnitTest;

[TestClass]
public class DepositPlayerMoneyUnitTest
{
	[TestMethod]
	public async Task 主錢包轉入遊戲廠商成功()
	{
		var amount = 10M;
		var balance = 0M + amount;

		var transactionId = "7183ccd2-3d50-ed11-ade6-0050f238e028";
		var apiUrl = new Uri("https://ws-test.insvr.com/jsonapi/DepositPlayerMoney");

		var response =
			"{" +
			"\"Success\": true," +
			"\"Amount\": 10.00," +
			$"\"RealBalance\": {balance}," +
			"\"CurrencyCode\": \"RMB\"," +
			$"\"TransactionId\": \"{transactionId}\"," +
			"\"Message\": \"Deposit Success\"," +
			"\"CurrencySymbol\": \"¥\"" +
			"}";

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, apiUrl.ToString())
			.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);
		var actual = await sut.DepositPlayerMoneyAsync(
				apiUrl: apiUrl,
				brandId: "b97e1834-d142-ed11-a27c-0050f23876f7",
				apiKey: "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
				username: "4ucmqplxb029",
				password: "HabaTfP@ssw0rd",
				currencyCode: "RMB",
				amount: amount,
				requestId: Guid.NewGuid().ToString())
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.AreEqual(balance, actual.Balance);
		Assert.AreEqual(transactionId, actual.TransactionId.ToString());
	}
}
