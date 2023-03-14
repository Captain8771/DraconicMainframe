using DSharpPlus;
using DSharpPlus.Entities;

namespace CapSharp.Utils;

public static class ExtensionMethods
{
    public static DiscordEmbedBuilder WithInvoker(this DiscordEmbedBuilder builder, DiscordUser user)
    {
        return builder.WithFooter($"Invoked by {user.Username}#{user.Discriminator}", user.AvatarUrl);
    }

    public static DiscordEmbedBuilder WithCapSharpAdvertising(this DiscordEmbedBuilder builder, DiscordClient? client = null)
    {
        return builder.WithAuthor($"{CapSharp.Config.Name} v{CapSharp.version}-{CapSharp.VersionSuffix}", null, client?.CurrentUser.AvatarUrl);
    }
}