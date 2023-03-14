using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json.Linq;

namespace CapSharp
{
    class CapSharp
    {
        private static readonly Version version = new Version(0, 0, 1);
        private const string VersionSuffix = "testing";
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
            slash.RegisterCommands<Commands.Dev>(841890589640359946);
            slash.RegisterCommands<Commands.Info>(841890589640359946);
            slash.RegisterCommands<Commands.Misc>(841890589640359946);

            await client.ConnectAsync(
                new DiscordActivity($"CapSharp v{version}-{VersionSuffix}", ActivityType.Playing),
                UserStatus.DoNotDisturb);
            await Task.Delay(-1); // what the fuck is this 💀
        }
    }
}