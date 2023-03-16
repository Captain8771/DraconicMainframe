using DSharpPlus.Entities;

namespace DraconicMainframe.Utils;

public class DraconicMainframeConfig
{
    public string Name { get; set; } = "DraconicMainframe";
    public DiscordColor Color { get; set; } = DiscordColor.Green;
    public ulong FeedbackChannelId { get; set; } = 0;
    public ulong BugReportChannelId { get; set; } = 0;
    public ulong[] OwnerIds { get; set; } = { };

    public DraconicMainframeConfig() { }
}