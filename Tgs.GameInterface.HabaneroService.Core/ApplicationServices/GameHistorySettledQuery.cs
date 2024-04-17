namespace Tgs.GameInterface.HabaneroService.Core.ApplicationServices;

public record GameHistorySettledQuery(
	Uri ApiUrl,
	string BrandId,
	string ApiKey,
	DateTime StartTime,
	DateTime EndTime);
