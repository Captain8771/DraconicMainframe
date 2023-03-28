using DraconicMainframe.Utils;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DraconicMainframe;


public sealed class DraconicMainframeService : IHostedService
{
    public readonly DiscordClient Client;
    private readonly IHostApplicationLifetime _applicationLifetime;
    public readonly ILogger<DraconicMainframeService> Logger;
    public readonly SlashCommandsExtension Slash;
    public readonly InteractivityExtension Interactivity;
    
    public DraconicMainframeService(ILogger<DraconicMainframeService> logger, IHostApplicationLifetime applicationLifetime)
    {
        Logger = logger;
        _applicationLifetime = applicationLifetime;
        Client = new(new()
        {
            Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN") ?? throw new Exception("No token provided."),
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.GuildMembers,
            LoggerFactory = new LoggerFactory().AddSerilog(),
            MinimumLogLevel = LogLevel.Warning,
            LogUnknownEvents = false
        });
        Slash = Client.UseSlashCommands();
        Interactivity = Client.UseInteractivity();
        
        // register commands
        Slash.RegisterCommands<Commands.Dev>(DraconicMainframe.GuildId);
        Slash.RegisterCommands<Commands.Info>(DraconicMainframe.GuildId);
        Slash.RegisterCommands<Commands.Misc>(DraconicMainframe.GuildId);
        
        // clyde-ai-funnies only
        Slash.RegisterCommands<Commands.ClydeServer>(831542504951251014);
        using (var db = new ClydeAIConfigContext())
        {
            db.Database.EnsureCreated();
        }
        
        // register event handlers
        Client.Ready += async (c, e) =>
        {
            Logger.LogInformation($"Bot {Client.CurrentUser.Username} is running!");
        };
        
        Client.MessageCreated += async (c, e) =>
        {
            if (e.Message.Content == c.CurrentUser.Mention)
            {
                DiscordEmbed embed = new DiscordEmbedBuilder()
                    .WithInvoker(e.Message.Author)
                    .WithTitle(DraconicMainframe.Config.Name)
                    .WithDescription("Run `/help` to get started!")
                    .WithColor(DraconicMainframe.Config.Color);
                    
                await e.Message.RespondAsync(embed);
            }
        };
        
        Slash.SlashCommandErrored += async (c, e) =>
        {
            // check the type of error in a switch
            DiscordEmbed embed;
            switch (e.Exception)
            {
                case SlashExecutionChecksFailedException:
                    embed = new DiscordEmbedBuilder()
                        .WithInvoker(e.Context.User)
                        .WithTitle("You can't use that command.")
                        .WithColor(DraconicMainframe.Config.Color)
                        .WithDraconicMainframeAdvertising(c.Client);
                    await e.Context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().AddEmbed(embed));
                    break;

                default:
                    embed = new DiscordEmbedBuilder()
                        .WithDraconicMainframeAdvertising(c.Client)
                        .WithTitle("An error occurred")
                        .WithDescription(e.Exception.Message)
                        .WithColor(DraconicMainframe.Config.Color)
                        .WithFooter("This is a bug. Bug has been reported, and a fix will be made.");
                
                    await e.Context.Channel.SendMessageAsync(embed: embed);
                    Logger.LogError(e.Exception, "An error occurred while executing a slash command.");
                    DiscordChannel channel = await c.Client.GetChannelAsync(DraconicMainframe.Config.BugReportChannelId);
                    if (channel is null)
                    {
                        Logger.LogError("Bug report channel not found.");
                        break;
                    }

                    await channel.SendMessageAsync(embed: new DiscordEmbedBuilder()
                        .WithTitle("An error occurred")
                        .WithDescription(e.Exception.Message)
                        .WithColor(DiscordColor.Red)
                        .WithFooter("Unhandled exception.")
                        .AddField("Command", e.Context.CommandName)
                        .AddField("User", e.Context.User.Username)
                        .AddField("Guild", e.Context.Guild?.Name ?? "DM"));
                    break;
            }
        };
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // test logging levels
        Logger.LogTrace("Logging test");
        Logger.LogDebug("Logging test");
        Logger.LogInformation("Logging test");
        Logger.LogWarning("Logging test");
        Logger.LogError("Logging test");
        Logger.LogCritical("Logging test");

        await Client.ConnectAsync(
            new DiscordActivity($"{DraconicMainframe.Config.Name} v{DraconicMainframe.version}-{DraconicMainframe.VersionSuffix}", ActivityType.Playing),
            UserStatus.DoNotDisturb);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Client.DisconnectAsync();
    }
}