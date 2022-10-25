using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.HabaneroService.Core.UnitTest;

[TestClass]
public class GetBrandCompletedGameResultsV2UnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://ws-test.insvr.com/jsonapi/GetBrandCompletedGameResultsV2");

        var response = "[{\"GameStateId\":3,\"FeatureCount\":0,\"BuyFeatureId\":0,\"PlayerId\":\"d69ba962-3b50-ed11-ade6-0050f238e028\",\"BrandId\":\"b97e1834-d142-ed11-a27c-0050f23876f7\",\"Username\":\"4ucmqplxb029\",\"BrandGameId\":\"ed07001f-ae28-44c7-9a57-833fb1065260\",\"GameKeyName\":\"SG12Zodiacs\",\"GameTypeId\":11,\"DtStarted\":\"2022-10-20T09:10:04.787\",\"DtCompleted\":\"2022-10-20T09:10:04.89\",\"FriendlyGameInstanceId\":2545686209,\"GameInstanceId\":\"43e18df9-5650-ed11-ade6-0050f238e028\",\"Stake\":0.3600,\"Payout\":0.0000,\"JackpotWin\":0.0000,\"JackpotContribution\":0.00000000,\"CurrencyCode\":\"RMB\",\"ChannelTypeId\":1,\"BalanceAfter\":9.4400},{\"GameStateId\":3,\"FeatureCount\":6,\"BuyFeatureId\":0,\"PlayerId\":\"d69ba962-3b50-ed11-ade6-0050f238e028\",\"BrandId\":\"b97e1834-d142-ed11-a27c-0050f23876f7\",\"Username\":\"4ucmqplxb029\",\"BrandGameId\":\"ed07001f-ae28-44c7-9a57-833fb1065260\",\"GameKeyName\":\"SG12Zodiacs\",\"GameTypeId\":11,\"DtStarted\":\"2022-10-20T09:10:10.32\",\"DtCompleted\":\"2022-10-20T09:10:59.727\",\"FriendlyGameInstanceId\":2545686313,\"GameInstanceId\":\"afe18df9-5650-ed11-ade6-0050f238e028\",\"Stake\":0.3600,\"Payout\":7.0200,\"JackpotWin\":0.0000,\"JackpotContribution\":0.00000000,\"CurrencyCode\":\"RMB\",\"ChannelTypeId\":1,\"BalanceAfter\":16.1000},{\"GameStateId\":3,\"FeatureCount\":0,\"BuyFeatureId\":0,\"PlayerId\":\"d69ba962-3b50-ed11-ade6-0050f238e028\",\"BrandId\":\"b97e1834-d142-ed11-a27c-0050f23876f7\",\"Username\":\"4ucmqplxb029\",\"BrandGameId\":\"5221ebe5-31d1-48d8-ab04-3b4b502d37f1\",\"GameKeyName\":\"SG5LuckyLions\",\"GameTypeId\":11,\"DtStarted\":\"2022-10-20T09:18:07.813\",\"DtCompleted\":\"2022-10-20T09:18:07.917\",\"FriendlyGameInstanceId\":2545696575,\"GameInstanceId\":\"a0a31a19-5850-ed11-ade6-0050f238e028\",\"Stake\":0.8800,\"Payout\":0.0000,\"JackpotWin\":0.0000,\"JackpotContribution\":0.00000000,\"CurrencyCode\":\"RMB\",\"ChannelTypeId\":1,\"BalanceAfter\":15.2200},{\"GameStateId\":3,\"FeatureCount\":0,\"BuyFeatureId\":0,\"PlayerId\":\"d69ba962-3b50-ed11-ade6-0050f238e028\",\"BrandId\":\"b97e1834-d142-ed11-a27c-0050f23876f7\",\"Username\":\"4ucmqplxb029\",\"BrandGameId\":\"5221ebe5-31d1-48d8-ab04-3b4b502d37f1\",\"GameKeyName\":\"SG5LuckyLions\",\"GameTypeId\":11,\"DtStarted\":\"2022-10-20T09:20:05.24\",\"DtCompleted\":\"2022-10-20T09:20:05.36\",\"FriendlyGameInstanceId\":2545699545,\"GameInstanceId\":\"b66df860-5850-ed11-ade6-0050f238e028\",\"Stake\":0.8800,\"Payout\":0.0000,\"JackpotWin\":0.0000,\"JackpotContribution\":0.00000000,\"CurrencyCode\":\"RMB\",\"ChannelTypeId\":1,\"BalanceAfter\":14.3400},{\"GameStateId\":3,\"FeatureCount\":9,\"BuyFeatureId\":0,\"PlayerId\":\"d69ba962-3b50-ed11-ade6-0050f238e028\",\"BrandId\":\"b97e1834-d142-ed11-a27c-0050f23876f7\",\"Username\":\"4ucmqplxb029\",\"BrandGameId\":\"5221ebe5-31d1-48d8-ab04-3b4b502d37f1\",\"GameKeyName\":\"SG5LuckyLions\",\"GameTypeId\":11,\"DtStarted\":\"2022-10-20T09:20:10.483\",\"DtCompleted\":\"2022-10-20T09:22:31.54\",\"FriendlyGameInstanceId\":2545699678,\"GameInstanceId\":\"3b6ef860-5850-ed11-ade6-0050f238e028\",\"Stake\":0.8800,\"Payout\":45.4400,\"JackpotWin\":0.0000,\"JackpotContribution\":0.00000000,\"CurrencyCode\":\"RMB\",\"ChannelTypeId\":1,\"BalanceAfter\":58.9000}]";

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, apiUrl.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);
        var actual = await sut.GetBrandCompletedGameResultsV2Async(
                apiUrl: apiUrl,
                brandId: "b97e1834-d142-ed11-a27c-0050f23876f7",
                apiKey: "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
                startTime: new DateTime(2022, 10, 20, 15, 10, 0),
                endTime: new DateTime(2022, 10, 20, 15, 25, 0))
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Length > 0);
    }
}
