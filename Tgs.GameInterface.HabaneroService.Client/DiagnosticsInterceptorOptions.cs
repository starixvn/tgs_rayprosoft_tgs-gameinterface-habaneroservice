using System.Diagnostics;

namespace Tgs.GameInterface.HabaneroService.Client;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.HabaneroService.Client.DiagnosticsInterceptor");
}
