namespace Tgs.GameInterface.HabaneroService.Core.Models;

public record WithdrawPlayerMoneyRequest
{
	public string BrandId { get; set; } = default!;

	public string APIKey { get; set; } = default!;

	public string Username { get; set; } = default!;

	public string Password { get; set; } = default!;

	public string CurrencyCode { get; set; } = default!;

	public decimal Amount { get; set; }

	public bool WithdrawAll { get; set; }

	public string RequestId { get; set; } = default!;
}
