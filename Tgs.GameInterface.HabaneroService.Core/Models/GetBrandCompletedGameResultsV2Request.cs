namespace Tgs.GameInterface.HabaneroService.Core.Models;

public record GetBrandCompletedGameResultsV2Request
{
    public string BrandId { get; set; } = default!;
    public string APIKey { get; set; } = default!;
    public string DtStartUTC { get; set; } = default!;
    public string DtEndUTC { get; set; } = default!;
}
