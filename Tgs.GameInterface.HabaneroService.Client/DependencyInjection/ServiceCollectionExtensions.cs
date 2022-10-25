using Microsoft.Extensions.Options;
using Tgs.GameInterface.HabaneroService.Client;
using Tgs.GameInterface.HabaneroService.Protos;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameInterfaceHabaneroServiceClient(
        this IServiceCollection services,
        Action<GameInterfaceHabaneroClientOptions, IServiceProvider> configure)
        => services
            .AddOptions<GameInterfaceHabaneroClientOptions>()
            .Configure(configure)
            .Services
            .AddGrpcClient<GameInterfaceHabaneroService.GameInterfaceHabaneroServiceClient>((sp, configureClient) =>
            {
                var options = sp.GetRequiredService<IOptions<GameInterfaceHabaneroClientOptions>>().Value;
                configureClient.Address = options.BaseUrl;
            })
            .ConfigureChannel((sp, configureChannel) =>
            {
                var handler = new SocketsHttpHandler
                {
                    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                    EnableMultipleHttp2Connections = true
                };
                configureChannel.HttpHandler = handler;
            })
            .AddInterceptor<LoggingInterceptor>()
            .Services
            .AddSingleton<DiagnosticsInterceptorOptions>()
            .AddSingleton<LoggingInterceptor>()
            .AddTransient<IGameInterfaceHabaneroClient, GameInterfaceHabaneroClient>();
}
