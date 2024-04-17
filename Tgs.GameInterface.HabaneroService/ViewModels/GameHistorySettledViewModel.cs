namespace Tgs.GameInterface.HabaneroService.ViewModels;

public record GameHistorySettledViewModel(
	Uri ApiUrl,
	string BrandId,
	string ApiKey,
	DateTime StartTime,
	DateTime EndTime);
