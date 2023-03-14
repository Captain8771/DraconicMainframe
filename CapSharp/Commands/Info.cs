using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;
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
        await ctx.DeferAsync(true);
        if (commandName != null)
        {
            // user wants help with a specific command
            // so focus on that command
            
            // get the command from the slash command extension
            
            // it gets stuck here for some reason, and it's infuriating.
            // toasty if you see this, tell me if there's a better way to do this.
            // because im going insane
            DiscordApplicationCommand? command = ctx.SlashCommandsExtension.RegisteredCommands
                .First(kv => kv.Key == null)
                // mfw i have to do this
                .Value.FirstOrDefault(cmd => String.Equals(cmd?.Name, commandName, StringComparison.CurrentCultureIgnoreCase), null);
            
            if (command == null)
            {
                // command not found
                await ctx.FollowUpAsync(
                    new DiscordFollowupMessageBuilder().WithContent("Command not found.").AsEphemeral()
                );
                return;
            }
            
            string helpString = $"__**{command.Mention}**__\n{command.Description}\n\n";

            DiscordEmbed embed = new DiscordEmbedBuilder()
                .WithAuthor(ctx.User)
                .WithTitle($"Help for {commandName}")
                .WithDescription(helpString)
                .WithColor(DiscordColor.Green)
                .WithThumbnail(ctx.Client.CurrentUser.AvatarUrl)
                .Build();

            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed).AsEphemeral());

        }
    }
}