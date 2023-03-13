using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;
using PronounDBLib;

namespace CapSharp.Commands;

public class Misc : ApplicationCommandModule
{
    private readonly PronounDBClient _client = new();
    
    [ContextMenu(ApplicationCommandType.UserContextMenu, "PronounDB: Pronouns")]
    public async Task PronounsContextMenu(ContextMenuContext ctx)
    {
        DiscordUser user = ctx.TargetUser;
        string pronouns = await _client.GetDiscordPronounsAsync(user.Id.ToString());
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent($"{user.Mention}'s preferred pronouns are `{pronouns}`.").AsEphemeral());
    }
    
}