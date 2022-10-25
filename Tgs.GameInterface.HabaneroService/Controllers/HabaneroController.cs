using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.HabaneroService.Core.ApplicationServices;
using Tgs.GameInterface.HabaneroService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.HabaneroService.Controllers;

[ApiController]
[Route("habanero")]
public class HabaneroController : ControllerBase
{
    /// <summary>
    /// Funds the in asynchronous.(GP -> Tgs)
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://ws-test.insvr.com",
    ///             "brandId": "b97e1834-d142-ed11-a27c-0050f23876f7",
    ///             "apiKey": "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
    ///             "username": "4ucmqplxb029",
    ///             "password": "HabaTfP@ssw0rd",
    ///             "currencyCode": "RMB",
    ///             "amount": 10,
    ///             "transactionId": "8a1ff03c-8101-0000-0001-00009da1fea9"
    ///         }
    /// </remarks>
    [HttpPost("fund/in", Name = "FundIn")]
    public async ValueTask<FundResultViewModel> FundInAsync(
        [FromServices] IGetService<FundInQuery, FundResult> service,
        FundViewModel source)
    {
        var query = new FundInQuery(
            ApiUrl: source.ApiUrl,
            BrandId: source.BrandId,
            ApiKey: source.ApiKey,
            Username: source.Username,
            Password: source.Password,
            CurrencyCode: source.CurrencyCode,
            Amount: source.Amount,
            TransactionId: source.TransactionId);

        var result = await service
            .GetAsync(query, HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new FundResultViewModel(
            TransactionId: result.TransactionId,
            Balance: result.Balance);
    }

    /// <summary>
    /// Funds the out asynchronous.(Tgs -> GP)
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://ws-test.insvr.com",
    ///             "brandId": "b97e1834-d142-ed11-a27c-0050f23876f7",
    ///             "apiKey": "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
    ///             "username": "4ucmqplxb029",
    ///             "password": "HabaTfP@ssw0rd",
    ///             "currencyCode": "RMB",
    ///             "amount": 40,
    ///             "transactionId": "8454cbdb-8101-0000-0001-00005042f4aa"
    ///         }
    /// </remarks>
    [HttpPost("fund/out", Name = "FundOut")]
    public async ValueTask<FundResultViewModel> FundOutAsync(
            [FromServices] IGetService<FundOutQuery, FundResult> service,
            FundViewModel source)
    {
        var query = new FundOutQuery(
            ApiUrl: source.ApiUrl,
            BrandId: source.BrandId,
            ApiKey: source.ApiKey,
            Username: source.Username,
            Password: source.Password,
            CurrencyCode: source.CurrencyCode,
            Amount: source.Amount,
            TransactionId: source.TransactionId);

        var result = await service
                .GetAsync(query, HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new FundResultViewModel(
            TransactionId: result.TransactionId,
            Balance: result.Balance);
    }

    /// <summary>
    /// Funds the status asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://ws-test.insvr.com",
    ///             "brandId": "b97e1834-d142-ed11-a27c-0050f23876f7",
    ///             "apiKey": "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
    ///             "username": "4ucmqplxb029",
    ///             "transactionId": "8454cbdb-8101-0000-0001-00005042f4aa"
    ///         }
    /// </remarks>
    [HttpPost("fund/Status", Name = "FundStatus")]
    public ValueTask<bool> FundStatusAsync(
        [FromServices] IGetService<FundStatusQuery, bool> service,
        FundStatusViewModel source)
    {
        var query = new FundStatusQuery(
            ApiUrl: source.ApiUrl,
            BrandId: source.BrandId,
            ApiKey: source.ApiKey,
            Username: source.Username,
            TransactionId: source.TransactionId);

        return service.GetAsync(query, HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the create asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://ws-test.insvr.com",
    ///             "brandId": "b97e1834-d142-ed11-a27c-0050f23876f7",
    ///             "apiKey": "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
    ///             "playerHostAddress": "127.0.0.1",
    ///             "userAgent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36",
    ///             "username": "4ucmqplxb029",
    ///             "password": "HabaTfP@ssw0rd",
    ///             "currencyCode": "RMB"
    ///         }
    /// </remarks>
    [HttpPost("player/create", Name = "PlayerCreate")]
    public ValueTask PlayerCreateAsync(
        [FromServices] ICommandService<PlayerCreateCommand> service,
        PlayerCreateViewModel source)
    {
        var command = new PlayerCreateCommand(
            ApiUrl: source.ApiUrl,
            BrandId: source.BrandId,
            ApiKey: source.ApiKey,
            PlayerHostAddress: source.PlayerHostAddress,
            UserAgent: source.UserAgent,
            Username: source.Username,
            Password: source.Password,
            CurrencyCode: source.CurrencyCode);

        return service.ExecuteAsync(
                command: command,
                cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the balance asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://ws-test.insvr.com",
    ///             "brandId": "b97e1834-d142-ed11-a27c-0050f23876f7",
    ///             "apiKey": "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
    ///             "username": "4ucmqplxb029",
    ///             "password": "HabaTfP@ssw0rd"
    ///         }
    /// </remarks>
    [HttpPost("player/balance", Name = "PlayerBalance")]
    public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
        [FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
        PlayerBalanceViewModel source)
    {
        var query = new PlayerBalanceQuery(
            ApiUrl: source.ApiUrl,
            BrandId: source.BrandId,
            ApiKey: source.ApiKey,
            Username: source.Username,
            Password: source.Password);

        var result = await service
            .GetAsync(query, HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new PlayerBalanceResultViewModel(
            result.IsPlayerExist,
            result.Balance);
    }

    /// <summary>
    /// Gets the token asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://ws-test.insvr.com",
    ///             "brandId": "b97e1834-d142-ed11-a27c-0050f23876f7",
    ///             "apiKey": "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
    ///             "playerHostAddress": "127.0.0.1",
    ///             "userAgent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36",
    ///             "username": "4ucmqplxb029",
    ///             "password": "HabaTfP@ssw0rd",
    ///             "currencyCode": "RMB"
    ///         }
    /// </remarks>
    [HttpPost("token", Name = "GetToken")]
    public async ValueTask<GetTokenResultViewModel> GetTokenAsync(
            [FromServices] IGetService<GetTokenQuery, GetTokenResult> service,
            GetTokenViewModel source)
    {
        var query = new GetTokenQuery(
            ApiUrl: source.ApiUrl,
            BrandId: source.BrandId,
            ApiKey: source.ApiKey,
            PlayerHostAddress: source.PlayerHostAddress,
            UserAgent: source.UserAgent,
            Username: source.Username,
            Password: source.Password,
            CurrencyCode: source.CurrencyCode);

        var result = await service
            .GetAsync(query, HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new GetTokenResultViewModel(result.Token);
    }

    /// <summary>
    /// Kicks the player asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://ws-test.insvr.com",
    ///             "brandId": "b97e1834-d142-ed11-a27c-0050f23876f7",
    ///             "apiKey": "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
    ///             "username": "4ucmqplxb029",
    ///             "password": "HabaTfP@ssw0rd"
    ///         }
    /// </remarks>
    [HttpPost("kick")]
    public ValueTask<bool> KickPlayerAsync(
            [FromServices] IGetService<LogoutCommand, bool> service,
            KickPlayerViewModel model)
        => service.GetAsync(new LogoutCommand(
            model.ApiUrl,
            model.BrandId,
            model.ApiKey,
            model.Username,
            model.Password));

    /// <summary>
    /// Games the history settled asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://ws-test.insvr.com",
    ///             "brandId": "b97e1834-d142-ed11-a27c-0050f23876f7",
    ///             "apiKey": "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
    ///             "startTime": "2022-10-20T09:10:00.000Z",
    ///             "endTime": "2022-10-20T09:25:00.000Z"
    ///         }
    /// </remarks>
    [HttpPost("game/history/settled", Name = "GameHistorySettled")]
    public async IAsyncEnumerable<GameHistorySettledResultViewModel> GameHistorySettledAsync(
            [FromServices] IQueryService<GameHistorySettledQuery, GameHistorySettledResult> service,
            GameHistorySettledViewModel model)
    {
        var query = new GameHistorySettledQuery(
            ApiUrl: model.ApiUrl,
            BrandId: model.BrandId,
            ApiKey: model.ApiKey,
            StartTime: model.StartTime,
            EndTime: model.EndTime);

        var gameRecords = await service
            .QueryAsync(query, HttpContext.RequestAborted)
            .ToArrayAsync(HttpContext.RequestAborted)
            .ConfigureAwait(false);

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords)
        {
            yield return new GameHistorySettledResultViewModel(
                PlayerCode: gameRecord.PlayerCode,
                ExternalRoundId: gameRecord.ExternalRoundId,
                ExternalGameId: gameRecord.ExternalGameId,
                ExternalCreatedOn: gameRecord.ExternalCreatedOn,
                ExternalClosedOn: gameRecord.ExternalClosedOn,
                Bet: gameRecord.Bet,
                ValidBet: gameRecord.ValidBet,
                PlayerWinLoss: gameRecord.PlayerWinLoss,
                Turnover: gameRecord.Turnover,
                Settle: gameRecord.Settle);
        }
    }
}
