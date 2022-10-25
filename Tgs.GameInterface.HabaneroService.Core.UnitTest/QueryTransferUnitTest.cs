using System.Net;
using System.Transactions;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.HabaneroService.Core.UnitTest;

[TestClass]
public class QueryTransferUnitTest
{
    [TestMethod]
    public async Task Fund_Out_Normal_Process()
    {
        var apiUrl = new Uri("https://ws-test.insvr.com/jsonapi/QueryTransfer");

        var response =
            "{" +
            "\"Success\":true," +
            "\"PlayerId\":\"d69ba962-3b50-ed11-ade6-0050f238e028\"," +
            "\"Username\":\"4ucmqplxb029\"," +
            "\"Amount\":40.0000," +
            "\"BalanceAfter\":155.1000," +
            "\"DtAdded\":\"2022-10-24T05:35:34.93\"," +
            "\"TransactionId\":\"076346ac-5d53-ed11-ade6-0050f238e028\"," +
            "\"CurrencyCode\":\"RMB\"" +
            "}";

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, apiUrl.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);
        var actual = await sut.QueryTransferAsync(
                apiUrl: apiUrl,
                brandId: "b97e1834-d142-ed11-a27c-0050f23876f7",
                apiKey: "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
                username: "4ucmqplxb029",
                requestId: "8454cbdb-8101-0000-0001-00005042f4aa")
            .ConfigureAwait(false);

        Assert.IsTrue(actual);        
    }

    [TestMethod]
    public async Task Fund_In_Normal_Process()
    {
        var apiUrl = new Uri("https://ws-test.insvr.com/jsonapi/QueryTransfer");
        var response =
            "{" +
            "\"Success\":true," +
            "\"PlayerId\":\"d69ba962-3b50-ed11-ade6-0050f238e028\"," +
            "\"Username\":\"4ucmqplxb029\"," +
            "\"Amount\":-10.0000," +
            "\"BalanceAfter\":145.1000," +
            "\"DtAdded\":\"2022-10-24T05:40:37.163\"," +
            "\"TransactionId\":\"de200b60-5e53-ed11-ade6-0050f238e028\"," +
            "\"CurrencyCode\":\"RMB\"" +
            "}";

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, apiUrl.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);
        var actual = await sut.QueryTransferAsync(
                apiUrl: apiUrl,
                brandId: "b97e1834-d142-ed11-a27c-0050f23876f7",
                apiKey: "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
                username: "4ucmqplxb029",
                requestId: "8a1ff03c-8101-0000-0001-00009da1fea9")
            .ConfigureAwait(false);

        Assert.IsTrue(actual);
    }
}
