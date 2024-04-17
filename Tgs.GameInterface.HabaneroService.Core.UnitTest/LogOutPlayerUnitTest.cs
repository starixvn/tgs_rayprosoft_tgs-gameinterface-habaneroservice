using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.HabaneroService.Core.UnitTest;

[TestClass]
public class LogOutPlayerUnitTest
{
	[TestMethod]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://ws-test.insvr.com/jsonapi/logoutplayer");

		var response =
			"{" +
			"\"Success\": true," +
			"\"Message\": \"Player Logged out\"" +
			"}";

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, apiUrl.ToString())
			.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		await sut.LogOutPlayerAsync(
			apiUrl: apiUrl,
			brandId: "b97e1834-d142-ed11-a27c-0050f23876f7",
			apiKey: "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
			username: "4ucmqplxb029",
			password: "HabaTfP@ssw0rd")
		   .ConfigureAwait(false);
	}
}
