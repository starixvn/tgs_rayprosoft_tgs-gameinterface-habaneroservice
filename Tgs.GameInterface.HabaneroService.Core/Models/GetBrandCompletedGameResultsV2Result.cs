namespace Tgs.GameInterface.HabaneroService.Core.Models;

public record GetBrandCompletedGameResultsV2Result
{
    public int GameStateId { get; set; }
    public int FeatureCount { get; set; }
    public int BuyFeatureId { get; set; }
    public string PlayerId { get; set; } = default!;
    public string BrandId { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string BrandGameId { get; set; } = default!;
    public string GameKeyName { get; set; } = default!;
    public int GameTypeId { get; set; }
    public DateTime DtStarted { get; set; }
    public DateTime DtCompleted { get; set; }
    public long FriendlyGameInstanceId { get; set; }
    public Guid GameInstanceId { get; set; } = default!;
    public decimal Stake { get; set; }
    public decimal Payout { get; set; }
    public decimal JackpotWin { get; set; }
    public decimal JackpotContribution { get; set; }
    public string CurrencyCode { get; set; } = default!;
    public int ChannelTypeId { get; set; }
    public decimal BalanceAfter { get; set; }
}
