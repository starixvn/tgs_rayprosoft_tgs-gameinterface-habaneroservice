using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.HabaneroService.Core.UnitTest;

[TestClass]
public class LoginOrCreatePlayerUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://ws-test.insvr.com/jsonapi/loginorcreateplayer");

        var response =
            "{" +
            "\"Authenticated\": true," +
            "\"PlayerId\": \"d69ba962-3b50-ed11-ade6-0050f238e028\"," +
            "\"BrandId\": \"b97e1834-d142-ed11-a27c-0050f23876f7\"," +
            "\"BrandName\": \"Pega Gaming\"," +
            "\"Token\": \"pf-6014547597b4610dbb5451fb92a80526274a06a4wsavi\"," +
            "\"RealBalance\": 10.0000," +
            "\"CurrencyCode\": \"RMB\"," +
            "\"PlayerCreated\": false," +
            "\"HasBonus\": false," +
            "\"BonusBalance\": 0.0," +
            "\"BonusSpins\": 0," +
            "\"BonusGameId\": 0," +
            "\"BonusPercentage\": 0.0," +
            "\"BonusWagerRemaining\": 0.0," +
            "\"CurrencySymbol\": \"¥\"," +
            "\"PointBalance\": 0.0" +
            "}";

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, apiUrl.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);
        var actual = await sut.LoginOrCreatePlayerAsync(
            apiUrl: apiUrl,
            brandId: "b97e1834-d142-ed11-a27c-0050f23876f7",
            apiKey: "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
            username: "4ucmqplxb029",
            password: "HabaTfP@ssw0rd",
            currencyCode: "RMB",
            useragent: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36",
            playerHostAddress: "127.0.0.1",
            firstName: "GPI",
            lastName: "4ucmqplxb029")
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsFalse(string.IsNullOrEmpty(actual));
    }
}
