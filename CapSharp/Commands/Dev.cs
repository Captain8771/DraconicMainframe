using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace CapSharp.Commands;

public class Dev : ApplicationCommandModule
{
    [SlashCommand("eval", "Evaluate a C# expression.")]
    [SlashRequireOwner]
    public async Task EvalCS(InteractionContext context, [Option("code", "the code to execute")] string code)
    {
        await context.CreateResponseAsync(embed: new DiscordEmbedBuilder()
            .WithColor(new DiscordColor("#FF007F"))
            .WithDescription("💭 Evaluating...")
            .Build());

        try
        {
            var globals = new TestVariables(context.Interaction, context.Client, context);

            var scriptOptions = ScriptOptions.Default;
            scriptOptions = scriptOptions.WithImports("System", "System.Collections.Generic", "System.Linq",
                "System.Text", "System.Threading.Tasks", "DSharpPlus");
            scriptOptions = scriptOptions.WithReferences(AppDomain.CurrentDomain.GetAssemblies()
                .Where(xa => !xa.IsDynamic && !string.IsNullOrWhiteSpace(xa.Location)));

            var script = CSharpScript.Create(code, scriptOptions, typeof(TestVariables));
            script.Compile();
            var result = await script.RunAsync(globals).ConfigureAwait(false);

            if (result is {ReturnValue: not null} &&
                !string.IsNullOrWhiteSpace(result.ReturnValue.ToString()))
                await context.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(new DiscordEmbedBuilder()
                {
                    Title = "✅ Evaluation Result", Description = result.ReturnValue.ToString(),
                    Color = new DiscordColor("#089FDF")
                }));
            else
                await context.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(new DiscordEmbedBuilder()
                {
                    Title = "✅ Evaluation Result", Description = "No result was returned",
                    Color = new DiscordColor("#089FDF")
                }));
        }
        catch (Exception ex)
        {
            await context.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(new DiscordEmbedBuilder()
            {
                Title = "⚠️ Evaluation Failure",
                Description = string.Concat("**", ex.GetType().ToString(), "**: ", ex.Message),
                Color = new DiscordColor("#FF0000")
            }));
        }
    }
}

public class TestVariables
{
    public DiscordInteraction Interaction { get; }
    public DiscordChannel Channel { get; }
    public DiscordGuild? Guild { get; }
    public DiscordUser User { get; }
    public DiscordMember? Member { get; set; }
    public InteractionContext Context { get; set; }

    public TestVariables(DiscordInteraction interaction, DiscordClient client, InteractionContext ctx)
    {
        Client = client;

        Interaction = interaction;
        Channel = interaction.Channel;
        Guild = Channel.Guild;
        User = Interaction.User;
        if (Guild != null)
            Member = Guild.GetMemberAsync(User.Id).ConfigureAwait(false).GetAwaiter().GetResult();
        Context = ctx;
    }

    public DiscordClient Client;
}