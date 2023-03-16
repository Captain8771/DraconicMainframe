using Serilog.Templates.Themes;

namespace DraconicMainframe;

// This theme was yoinked from Tirion, which was apparently yoinked from Insanitybot-v3? idk shits confusing to my small brain
public class LoggerTheme
{
    public static Dictionary<TemplateThemeStyle, String> ThemeStyles { get; } = new()
    {
        [TemplateThemeStyle.LevelDebug] = "\u001b[38;5;212m",
        [TemplateThemeStyle.LevelInformation] = "\u001b[38;5;141m",
        [TemplateThemeStyle.LevelError] = "\u001b[38;5;196m",
        [TemplateThemeStyle.LevelFatal] = "\u001b[38;5;88m",

        [TemplateThemeStyle.String] = "\u001b[38;5;159m"
    };

    public static TemplateTheme Theme { get; } = new(TemplateTheme.Literate, ThemeStyles);
}