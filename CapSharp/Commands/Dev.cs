using CapSharp.Utils;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace CapSharp.Commands;

public class Dev : ApplicationCommandModule
{
    [SlashCommand("eval", "Evaluate a C# expression.")]
    [SlashRequireOwner]
    public async Task EvalCommand(InteractionContext ctx,
        [Option("expression", "The expression to evaluate.")] string expression)
    {
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
        DiscordEmbedBuilder embed = new DiscordEmbedBuilder()
            .WithCapSharpAdvertising(ctx.Client)
            .WithTitle("Evaluation")
            .WithColor(CapSharp.Config.Color)
            .WithInvoker(ctx.User);

        try
        {
            var result = CSharpScript.EvaluateAsync(expression, null, new { ctx = ctx, Client = ctx.Client} ).Result;
            embed.WithDescription($"```cs\n{result}```");
        } 
        catch (Exception e)
        {
            embed.WithDescription($"```cs\n{e}```").WithColor(DiscordColor.Red);
        }
        
        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));
    }
}