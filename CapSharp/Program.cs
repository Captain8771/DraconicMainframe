using CapSharp.Utils;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json.Linq;

namespace CapSharp
{
    class CapSharp
    {
        [DontInject] public static readonly Version version = new Version(0, 0, 1);
        [DontInject] public static readonly string VersionSuffix = "testing";
        [DontInject] public static ulong? guildId = 841890589640359946;
        // make a config property that i can add properties to
        [DontInject] public static CapSharpConfig Config = new()
        {
            Name = "CapSharp",
            Color = DiscordColor.Green,
            FeedbackChannelId = 841890589640359949,
            BugReportChannelId = 841890589640359950
        };
        static async Task Main(string[] args)
        {
            // read the token from the environment variable "DISCORD_TOKEN"
            string token = Environment.GetEnvironmentVariable("DISCORD_TOKEN") ?? throw new Exception("No token provided.");

            DiscordClient client = new(new()
            {
                Token = token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });

            SlashCommandsExtension slash = client.UseSlashCommands();
            // TODO: global commands
            
            client.MessageCreated += async (c, e) =>
            {
                if (e.Message.Content == c.CurrentUser.Mention)
                {
                    DiscordEmbed embed = new DiscordEmbedBuilder()
                        .WithInvoker(e.Message.Author)
                        .WithTitle(Config.Name)
                        .WithDescription("Run `/help` to get started!")
                        .WithColor(Config.Color);
                    
                    await e.Message.RespondAsync(embed);
                }
            };
            
            slash.RegisterCommands<Commands.Dev>(guildId);
            slash.RegisterCommands<Commands.Info>(guildId);
            slash.RegisterCommands<Commands.Misc>(guildId);

            await client.ConnectAsync(
                new DiscordActivity($"{Config.Name} v{version}-{VersionSuffix}", ActivityType.Playing),
                UserStatus.DoNotDisturb);
            await Task.Delay(-1); // what the fuck is this 💀
        }
    }
}