using DSharpPlus;
using DSharpPlus.Entities;

namespace DraconicMainframe.Utils;

public static class ExtensionMethods
{
    public static DiscordEmbedBuilder WithInvoker(this DiscordEmbedBuilder builder, DiscordUser user)
    {
        return builder.WithFooter($"Invoked by {user.Username}#{user.Discriminator}", user.AvatarUrl);
    }

    public static DiscordEmbedBuilder WithDraconicMainframeAdvertising(this DiscordEmbedBuilder builder, DiscordClient? client = null)
    {
        return builder.WithAuthor($"{DraconicMainframe.Config.Name} v{DraconicMainframe.version}-{DraconicMainframe.VersionSuffix}", null, client?.CurrentUser.AvatarUrl);
    }
}