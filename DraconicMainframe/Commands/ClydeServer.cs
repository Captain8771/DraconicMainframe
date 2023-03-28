using DraconicMainframe.Utils;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;

namespace DraconicMainframe.Commands;

public class ClydeServer : ApplicationCommandModule
{

    // these commands are only available to the clyde ai server
    // so restrict them to the server

    [SlashCommand("create-channel", "Create a channel in the clyde ai server.")]
    [Checks.ClydeServerOnlyAttribute]
    public async Task CreateChannel(InteractionContext ctx, [Option("name", "the name of the channel")] string name)
    {
        // get the 'private-channels' category
        var category = ctx.Guild.GetChannel(1086391329182994592);
        // check the config to see if the channel already exists
        ClydeAIConfigContext config = new ClydeAIConfigContext();
        var lastIAmHorribleAtVariableNaming = config.Configs.Where(c => c.userId == ctx.User.Id).ToList();
        // if the user doesn't have a config entry, create one
        if (lastIAmHorribleAtVariableNaming.Count == 0)
        {
            var x = new ClydeAIConfig
            {
                userId = ctx.User.Id,
                privateChannelId = 0
            };
            config.Configs.Add(x);
            await config.SaveChangesAsync();
            lastIAmHorribleAtVariableNaming.Add(x);
        }
        
        // get the channel id from the config
        var id = lastIAmHorribleAtVariableNaming[0].privateChannelId;
        
        if (ctx.Guild.GetChannel(id) is not null)
        {
            // channel already exists
            await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
                .WithInvoker(ctx.User)
                .WithDraconicMainframeAdvertising(ctx.Client)
                .WithTitle("You already have a private channel!")
                .WithDescription($"Go check out <#{id}>!"));
            return;
        }
        
        // set up permissions
        DiscordOverwriteBuilder deniedOverwrites = new DiscordOverwriteBuilder(ctx.Guild.EveryoneRole)
            .Deny(Permissions.AccessChannels);

        DiscordOverwriteBuilder allowedOverwrites = new DiscordOverwriteBuilder(ctx.Member)
            .Allow(Permissions.AccessChannels)
            .Allow(Permissions.ManageMessages)
            // we also only want them to be able to edit permissions of the channel
            .Allow(Permissions.ManageRoles);

        var overwrites = new List<DiscordOverwriteBuilder> {deniedOverwrites, allowedOverwrites};

        DiscordChannel channel = null!;
        
        // create the channel
        try
        {
            channel =
                await ctx.Guild.CreateChannelAsync(name, ChannelType.Text, category, overwrites: overwrites);
        }
        catch (UnauthorizedException e)
        {
            await ctx.CreateResponseAsync(e.JsonMessage);
            return;
        }

        // update the config
        config.Configs.Where(c => c.userId == ctx.User.Id).ToList()[0].privateChannelId = channel.Id;
        await config.SaveChangesAsync();
        
        DiscordEmbedBuilder embed = new DiscordEmbedBuilder()
            .WithInvoker(ctx.User)
            .WithDraconicMainframeAdvertising(ctx.Client)
            .WithTitle("Channel created!")
            .WithDescription($"Go check out <#{channel.Id}>!");
        
        await ctx.CreateResponseAsync(embed);
    }

    [SlashCommand("announcement-ping", "Toggle pings for announcements.")]
    [Checks.ClydeServerOnlyAttribute]
    public async Task AnnouncementPing(InteractionContext ctx)
    {
        DiscordRole role = ctx.Guild.GetRole(1090208131952414830);
        
        // check if the user has the role
        if (ctx.Member.Roles.Contains(role))
        {
            // remove the role
            await ctx.Member.RevokeRoleAsync(role);
            await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
                .WithInvoker(ctx.User)
                .WithDraconicMainframeAdvertising(ctx.Client)
                .WithTitle("Pings disabled!")
                .WithDescription("You will no longer be pinged for announcements."));
        }
        else
        {
            // add the role
            await ctx.Member.GrantRoleAsync(role);
            await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
                .WithInvoker(ctx.User)
                .WithDraconicMainframeAdvertising(ctx.Client)
                .WithTitle("Pings enabled!")
                .WithDescription("You will now be pinged for announcements."));
        }
    }

    [SlashCommand("clear-threads", "removes you from every thread in the channel.")]
    [Checks.ClydeServerOnlyAttribute]
    public async Task ClearThreads(InteractionContext ctx)
    {
        ctx.Guild.Threads.Values
            .Where(x => x.ParentId == ctx.Channel.Id && x.GetThreadMemberAsync(ctx.Member) is not null)
            .ToList()
            .ForEach(async x =>
            {
                await x.RemoveThreadMemberAsync(ctx.Member);
            });
        
        await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
            .WithInvoker(ctx.User)
            .WithDraconicMainframeAdvertising(ctx.Client)
            .WithTitle("Threads cleared!")
            .WithDescription("You have been removed from every thread in this channel."));
    }
}