using DraconicMainframe.Utils;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Templates;

namespace DraconicMainframe
{
    class DraconicMainframe
    {
        [DontInject] public static readonly Version version = new Version(0, 0, 1);
        [DontInject] public static readonly string VersionSuffix = "testing";
        [DontInject] public static ulong? GuildId = 820452671273172992;
        [DontInject] public static DraconicMainframeConfig Config = new()
        {
            Name = "CapSharp",
            Color = DiscordColor.Green,
            FeedbackChannelId = 841890589640359949,
            BugReportChannelId = 841890589640359950,
            OwnerIds = new ulong[]
            {
                347366054806159360, // Captain#3175
                813770420758511636  // Niko Oneshot#0584
            }
        };
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(formatter: new ExpressionTemplate("[{@t:yyyy-MM-dd HH:mm:ss.fff}] [{@l:u3}] {@m}\n{@x}", theme: LoggerTheme.Theme))
                .WriteTo.File("logs/.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            
            await Host.CreateDefaultBuilder()
                .UseConsoleLifetime()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(logging => logging.ClearProviders().AddSerilog());
                    services.AddHostedService<DraconicMainframeService>();
                })
                .RunConsoleAsync();

            await Log.CloseAndFlushAsync();
        }
    }
}