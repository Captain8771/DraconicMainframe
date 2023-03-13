using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json.Linq;

namespace CapSharp
{
    class CapSharp
    {
        static readonly Version version = new Version(0, 0, 1);
        const string VersionSuffix = "unstable";
        static async Task Main(string[] args)
        {
            // TODO: unhardcode this
            string token = "";
            
            DiscordClient client = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });

            SlashCommandsExtension slash = client.UseSlashCommands();
            slash.RegisterCommands<Commands.Dev>(841890589640359946);
            slash.RegisterCommands<Commands.Info>(841890589640359946);

            await client.ConnectAsync(
                new DiscordActivity($"CapSharp v{version}-{VersionSuffix}", ActivityType.Playing),
                UserStatus.DoNotDisturb);
            await Task.Delay(-1); // what the fuck is this 💀
        }
    }
}