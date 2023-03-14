using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using static CapSharp.Utils.ExtensionMethods;

namespace CapSharp.Commands;

public class Info : ApplicationCommandModule
{
    [SlashCommand("ping", "Check latency between the bot and discord.")]
    public async Task PingCommand(InteractionContext ctx)
    {
        await ctx.CreateResponseAsync(
            InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent(ctx.Client.Ping.ToString() + "ms").AsEphemeral()
        );
    }

    [SlashCommand("help", "Get help with a command.")]
    public async Task HelpCommand(InteractionContext ctx,
        [Option("command", "The command to get help with.")] string? commandName = null)
    {
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().AsEphemeral());
        if (commandName != null)
        {
            // user wants help with a specific command
            // so focus on that command
            
            // get the command from the slash command extension
            ulong? id = CapSharp.guildId;
            DiscordApplicationCommand? command = ctx.SlashCommandsExtension.RegisteredCommands
                .First(kv => kv.Key == id)
                // mfw i have to do this
                .Value.FirstOrDefault(cmd => String.Equals(cmd?.Name, commandName, StringComparison.CurrentCultureIgnoreCase)
                                             && cmd?.Type == ApplicationCommandType.SlashCommand, null);
            
            if (command is null)
            {
                // command not found
                await ctx.EditResponseAsync(
                    new DiscordWebhookBuilder().WithContent("Command not found.")
                );
                return;
            }
            
            string helpString = $"__**{command.Mention}**__\n{command.Description}\n\n";

            DiscordEmbed embed = new DiscordEmbedBuilder()
                .WithInvoker(ctx.User)
                .WithTitle($"Help for {commandName}")
                .WithDescription(helpString)
                .WithColor(CapSharp.Config.Color)
                .WithThumbnail(ctx.Client.CurrentUser.AvatarUrl)
                .WithCapSharpAdvertising(ctx.Client)
                .Build();

            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder().AddEmbed(embed)
            );
        }
        else
        {
            // show a list of all commands and their descriptions
            string helpString = "";
            IReadOnlyList<DiscordApplicationCommand> commands = ctx.SlashCommandsExtension.RegisteredCommands.First(kv => kv.Key == CapSharp.guildId).Value;
            foreach (DiscordApplicationCommand command in commands)
            {
                // make sure the command is a slash command and not a context menu
                if (command.Type == ApplicationCommandType.SlashCommand)
                {
                    helpString += $"__**{command.Mention}**__ - {command.Description}\n\n";
                }
            }
            
            DiscordEmbed embed = new DiscordEmbedBuilder()
                .WithInvoker(ctx.User)
                .WithTitle("Help")
                .WithDescription(helpString)
                .WithColor(CapSharp.Config.Color)
                .WithThumbnail(ctx.Client.CurrentUser.AvatarUrl)
                .WithCapSharpAdvertising(ctx.Client)
                .Build();

            await ctx.EditResponseAsync(
                new DiscordWebhookBuilder().AddEmbed(embed)
            );
        }
    }
}