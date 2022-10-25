using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.HabaneroService.Core.UnitTest;

[TestClass]
public class WithdrawPlayerMoneyUnitTest
{
    [TestMethod]
    public async Task 遊戲廠商錢包轉出主錢包成功()
    {
        var amount = 10M;
        var balance = 60M - amount;

        var transactionId = "24e62b7a-f050-ed11-ade6-0050f238e028";
        var apiUrl = new Uri("https://ws-test.insvr.com/jsonapi/WithdrawPlayerMoney");

        var response =
            "{" +
            "\"Success\": true," +
            "\"Amount\": -10.00," +
            $"\"RealBalance\": {balance}," +
            "\"CurrencyCode\": \"RMB\"," +
            $"\"TransactionId\": \"{transactionId}\"," +
            "\"Message\": \"Withdrawal Success\"," +
            "\"CurrencySymbol\": \"¥\"" +
            "}";

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, apiUrl.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);
        var actual = await sut.WithdrawPlayerMoneyAsync(
                apiUrl: apiUrl,
                brandId: "b97e1834-d142-ed11-a27c-0050f23876f7",
                apiKey: "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
                username: "4ucmqplxb029",
                password: "TgsP@ssw0rd",
                currencyCode: "RMB",
                amount: amount,
                requestId: Guid.NewGuid().ToString())
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(balance, actual.Balance);
        Assert.AreEqual(transactionId, actual.TransactionId.ToString());
    }
}
