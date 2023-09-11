using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.HabaneroService.Core.ApplicationServices;
using Tgs.GameInterface.HabaneroService.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.HabaneroService.GrpcServices;

internal class GameInterfaceHabaneroGrpcService : GameInterfaceHabaneroService.GameInterfaceHabaneroServiceBase
{
    private readonly IGetService<FundInQuery, FundResult> _fundInService;
    private readonly IGetService<FundOutQuery, FundResult> _fundOutService;
    private readonly IGetService<FundStatusQuery, bool> _fundStatusService;
    private readonly IQueryService<GameHistorySettledQuery, GameHistorySettledResult> _gameHistorySettledService;
    private readonly IGetService<GetTokenQuery, GetTokenResult> _getTokenService;
    private readonly ILogger<GameInterfaceHabaneroGrpcService> _logger;
    private readonly IGetService<PlayerBalanceQuery, PlayerBalanceResult> _playerBalanceService;
    private readonly ICommandService<PlayerCreateCommand> _playerCreateService;

    public GameInterfaceHabaneroGrpcService(
        ILogger<GameInterfaceHabaneroGrpcService> logger,
        IGetService<FundInQuery, FundResult> fundInService,
        IGetService<FundOutQuery, FundResult> fundOutService,
        IGetService<FundStatusQuery, bool> fundStatusService,
        ICommandService<PlayerCreateCommand> playerCreateService,
        IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
        IGetService<GetTokenQuery, GetTokenResult> getTokenService,
        IQueryService<GameHistorySettledQuery, GameHistorySettledResult> gameHistorySettledService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fundInService = fundInService ?? throw new ArgumentNullException(nameof(fundInService));
        _fundOutService = fundOutService ?? throw new ArgumentNullException(nameof(fundOutService));
        _fundStatusService = fundStatusService ?? throw new ArgumentNullException(nameof(fundStatusService));
        _playerCreateService = playerCreateService ?? throw new ArgumentNullException(nameof(playerCreateService));
        _playerBalanceService = playerBalanceService ?? throw new ArgumentNullException(nameof(playerBalanceService));
        _getTokenService = getTokenService ?? throw new ArgumentNullException(nameof(getTokenService));
        _gameHistorySettledService = gameHistorySettledService ?? throw new ArgumentNullException(nameof(gameHistorySettledService));
    }

    public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
    {
        _logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

        return Task.FromResult(new CheckingServiceReply
        {
            MachineName = Environment.MachineName
        });
    }

    public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
    {
        var query = new FundInQuery(
            ApiUrl: new(request.ApiUrl),
            BrandId: request.BrandId,
            ApiKey: request.ApiKey,
            Username: request.Username,
            Password: request.Password,
            CurrencyCode: request.CurrencyCode,
            Amount: request.Amount.ToDecimal().GetValueOrDefault(0),
            TransactionId: request.TransactionId);

        var result = await _fundInService
            .GetAsync(query, context.CancellationToken)
            .ConfigureAwait(false);

        return new FundReply()
        {
            TransactionId = result.TransactionId,
            Balance = result.Balance
        };
    }

    public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
    {
        var query = new FundOutQuery(
            ApiUrl: new(request.ApiUrl),
            BrandId: request.BrandId,
            ApiKey: request.ApiKey,
            Username: request.Username,
            Password: request.Password,
            CurrencyCode: request.CurrencyCode,
            Amount: request.Amount.ToDecimal().GetValueOrDefault(0),
            TransactionId: request.TransactionId);

        var result = await _fundOutService
            .GetAsync(query, context.CancellationToken)
            .ConfigureAwait(false);

        return new FundReply()
        {
            TransactionId = result.TransactionId,
            Balance = result.Balance
        };
    }

    public override async Task<FundStatusReply> FundStatus(FundStatusRequest request, ServerCallContext context)
    {
        var query = new FundStatusQuery(
            ApiUrl: new(request.ApiUrl),
            BrandId: request.BrandId,
            ApiKey: request.ApiKey,
            Username: request.Username,
            TransactionId: request.TransactionId);

        var result = await _fundStatusService
            .GetAsync(query, context.CancellationToken)
            .ConfigureAwait(false);

        return new FundStatusReply()
        {
            IsOrderExist = result
        };
    }

    public override async Task<GameHistorySettledReply> GameHistorySettled(GameHistorySettledRequest request, ServerCallContext context)
    {
        var query = new GameHistorySettledQuery(
            ApiUrl: new(request.ApiUrl),
            BrandId: request.BrandId,
            ApiKey: request.ApiKey,
            StartTime: request.StartTime.ToDateTime(),
            EndTime: request.EndTime.ToDateTime());

        var gameHistorySettleds = await _gameHistorySettledService
            .QueryAsync(query, context.CancellationToken)
            .ToArrayAsync(context.CancellationToken)
            .ConfigureAwait(false);

        var reply = new GameHistorySettledReply();

        if (gameHistorySettleds.Any())
        {
            foreach (var gameHistorySettled in gameHistorySettleds)
            {
                reply.Items.Add(new GameHistorySettled()
                {
                    PlayerCode = gameHistorySettled.PlayerCode,
                    ExternalRoundId = gameHistorySettled.ExternalRoundId,
                    ExternalGameId = gameHistorySettled.ExternalGameId,
                    ExternalCreatedOn = gameHistorySettled.ExternalCreatedOn.ToTimestamp(),
                    ExternalClosedOn = gameHistorySettled.ExternalClosedOn.ToTimestamp(),
                    Bet = gameHistorySettled.Bet,
                    ValidBet = gameHistorySettled.ValidBet,
                    PlayerWinLoss = gameHistorySettled.PlayerWinLoss,
                    Turnover = gameHistorySettled.Turnover,
                    Settle = gameHistorySettled.Settle,
                    AdditionalData=gameHistorySettled.AdditionalData
                });
            }
        }

        return reply;
    }

    public override async Task<GameTokenReply> GameToken(GameTokenRequest request, ServerCallContext context)
    {
        var query = new GetTokenQuery(
            ApiUrl: new(request.ApiUrl),
            BrandId: request.BrandId,
            ApiKey: request.ApiKey,
            PlayerHostAddress: request.PlayerHostAddress,
            UserAgent: request.UserAgent,
            Username: request.Username,
            Password: request.Password,
            CurrencyCode: request.CurrencyCode,
            FirstName: request.FirstName,
            LastName: request.LastName);

        var result = await _getTokenService
            .GetAsync(query, context.CancellationToken)
            .ConfigureAwait(false);

        return new GameTokenReply()
        {
            AuthToken = result.Token
        };
    }

    public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
    {
        var query = new PlayerBalanceQuery(
            ApiUrl: new(request.ApiUrl),
            BrandId: request.BrandId,
            ApiKey: request.ApiKey,
            Username: request.Username,
            Password: request.Password);

        var result = await _playerBalanceService
            .GetAsync(query, context.CancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceReply()
        {
            IsPlayerExist = result.IsPlayerExist,
            Balance = result.Balance
        };
    }

    public override async Task<BaseReply> PlayerCreate(PlayerCreateRequest request, ServerCallContext context)
    {
        var command = new PlayerCreateCommand(
            ApiUrl: new(request.ApiUrl),
            BrandId: request.BrandId,
            ApiKey: request.ApiKey,
            PlayerHostAddress: request.PlayerHostAddress,
            UserAgent: request.UserAgent,
            Username: request.Username,
            Password: request.Password,
            CurrencyCode: request.CurrencyCode,
            FirstName: request.FirstName,
            LastName: request.LastName);

        await _playerCreateService
            .ExecuteAsync(command, context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply()
        {
            Status = true
        };
    }
}
