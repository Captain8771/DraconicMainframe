using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Microsoft.Extensions.Logging;

namespace CapSharp.Commands;

[SlashCommandGroup("dev", "Developer commands.")]
[SlashRequireOwner]
public class Dev : ApplicationCommandModule
{
    [SlashCommand("sync", "Syncs slash commands with discord.")]
    public async Task SyncCommand(InteractionContext ctx)
    {
        ctx.Client.Logger.LogInformation("Syncing slash commands...");
        ctx.SlashCommandsExtension.RegisterCommands<Info>();
        ctx.SlashCommandsExtension.RegisterCommands<Dev>();
        
        await ctx.CreateResponseAsync(
            InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent("Synced slash commands.")
        );
    }
}