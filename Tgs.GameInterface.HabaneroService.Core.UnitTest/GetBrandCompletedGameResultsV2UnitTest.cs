using System.Net;
using System.Text.Json.Nodes;
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

        var response = new JsonArray {
            new JsonObject {
                ["FeatureCount"]= 0,
                ["BuyFeatureId"]= 0,
                ["PlayerId"]= "89dc452b-4637-ee11-b8f0-6045bd569110",
                ["BrandId"]= "a452e523-8d40-ed11-a27c-00155db8f23b",
                ["Username"]= "4ukop6pr8zj6",
                ["BrandGameId"]= "01acc930-4978-4d9d-9b7d-7e961f6894ec",
                ["GameKeyName"]= "SGMountMazuma",
                ["GameTypeId"]= 11,
                ["GameStateId"]= 3,
                ["DtStarted"]= "2023-08-10T06:26:08.01",
                ["DtCompleted"]= "2023-08-10T06:26:08.037",
                ["FriendlyGameInstanceId"]= 79003316977,
                ["GameInstanceId"]= "fcd07cc6-4637-ee11-b8f0-6045bd569110",
                ["Stake"]= 1.4,
                ["Payout"]= 14.0,
                ["JackpotWin"]= 0.0,
                ["JackpotContribution"]= 0.0,
                ["CurrencyCode"]= "HKD",
                ["ChannelTypeId"]= 4,
                ["BalanceAfter"]= 100.0
            }
        };

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, apiUrl.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString()));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);
        var actual = await sut.GetBrandCompletedGameResultsV2Async(
                apiUrl: apiUrl,
                brandId: "b97e1834-d142-ed11-a27c-0050f23876f7",
                apiKey: "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
                startTime: new DateTime(2023, 08, 10, 06, 26, 0),
                endTime: new DateTime(2023, 08, 10, 06, 26, 0))
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Any());
        Assert.AreEqual("4ukop6pr8zj6", actual.First().PlayerCode);
        Assert.AreEqual("79003316977", actual.First().ExternalRoundId);
        Assert.AreEqual("SGMountMazuma", actual.First().ExternalGameId);
        Assert.AreEqual(3, actual.First().GameStateId);
        Assert.AreEqual(DateTimeOffset.Parse("2023-08-10T06:26:08.01"), actual.First().ExternalCreatedOn);
        Assert.AreEqual(DateTimeOffset.Parse("2023-08-10T06:26:08.037"), actual.First().ExternalClosedOn);
        Assert.AreEqual(1.4m, actual.First().Bet);
        Assert.AreEqual(14.0m, actual.First().Settle);
        Assert.AreEqual(response.ElementAt(0)!.ToJsonString(), actual.First().AdditionalData);
    }
}
