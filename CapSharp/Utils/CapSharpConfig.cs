using DSharpPlus.Entities;

namespace CapSharp.Utils;

public class CapSharpConfig
{
    public string Name { get; set; } = "CapSharp";
    public DiscordColor Color { get; set; } = DiscordColor.Green;
    public ulong FeedbackChannelId { get; set; } = 0;
    public ulong BugReportChannelId { get; set; } = 0;

    public CapSharpConfig() { }
    public CapSharpConfig(string name, DiscordColor color, ulong feedbackChannelId, ulong bugReportChannelId)
    {
        Name = name;
        Color = color;
        FeedbackChannelId = feedbackChannelId;
        BugReportChannelId = bugReportChannelId;
    }
}