using DSharpPlus.Entities;

namespace CapSharp.Utils;

public static class ExtensionMethods
{
    public static DiscordEmbedBuilder WithAuthor(this DiscordEmbedBuilder builder, DiscordUser user)
    {
        return builder.WithFooter($"Invoked by {user.Username}#{user.Discriminator}", user.AvatarUrl);
    }
}