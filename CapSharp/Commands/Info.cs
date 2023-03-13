using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;

namespace CapSharp.Commands;

public class Info : ApplicationCommandModule
{
    [SlashCommand("ping", "Check latency between the bot and discord.")]
    public async Task PingCommand(InteractionContext ctx)
    {
        await ctx.CreateResponseAsync(
            InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent(ctx.Client.Ping.ToString() + "ms").AsEphemeral(true)
        );
    }
    
    [SlashCommand("help", "Get help with a command.")]
    public async Task HelpCommand(InteractionContext ctx, [Option("command", "The command to get help with.")] string? commandName = null)
    {
        if (commandName == null)
        {
            string commandString = "";
            // set the help command string to the following format:
            /*
             * __**ModuleName**__
             * CommandName,CommandName,CommandName
             */
            for (int i = 0; i < ctx.SlashCommandsExtension.RegisteredCommands.Count; i++)
            {
                var _command = ctx.SlashCommandsExtension.RegisteredCommands[i];
                IReadOnlyList<DiscordApplicationCommand> commands = _command.Value;
                for (int j = 0; j < commands.Count; j++)
                {
                    var command = commands[j];
                    string cmdOrGroup;
                    string[] groupNames = new string[] { "dev" };
                    if (groupNames.Contains(command.Name))
                    {
                        cmdOrGroup = "g";
                    }
                    else
                    {
                        cmdOrGroup = "c";
                    }

                    string cmd;
                    if (cmdOrGroup == "g")
                    {
                        cmd = command.Name;
                    }
                    else
                    {
                        cmd = command.Mention;
                    }
                    commandString += $"[{cmdOrGroup}] {cmd} - {command.Description}\n";
                    commandString += "\n\n";
                }
                
            }

            DiscordEmbed embed = new DiscordEmbedBuilder()
                .WithTitle("Help")
                .WithDescription(
                    "Use `/help [command]` for more info on a command.\nYou can also use `/help [category]` for more info on a category." +
                    "\n\n" + commandString);

            await ctx.CreateResponseAsync(
                InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(embed).AsEphemeral(true)
            );

        }
        else
        {
            // check if 
        }
    }
}