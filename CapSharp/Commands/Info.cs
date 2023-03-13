using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;

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
    public async Task HelpCommand(InteractionContext ctx, [Option("command", "The command to get help with.")] string? commandName = null)
    {
        // TODO: help command
        //  bother toasty about it
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, 
            new DiscordInteractionResponseBuilder().AsEphemeral().WithContent("This command is not yet implemented."));
    }
}