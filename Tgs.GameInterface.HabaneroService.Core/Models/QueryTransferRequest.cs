namespace Tgs.GameInterface.HabaneroService.Core.Models;

public record QueryTransferRequest
{
	public string BrandId { get; set; } = default!;
	public string APIKey { get; set; } = default!;
	public string Username { get; set; } = default!;

	public string RequestId { get; set; } = default!;
}
