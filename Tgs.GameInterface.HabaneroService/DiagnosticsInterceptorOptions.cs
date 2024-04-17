using System.Diagnostics;

namespace Tgs.GameInterface.HabaneroService;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameInterface.HabaneroService.DiagnosticsInterceptor");
}
