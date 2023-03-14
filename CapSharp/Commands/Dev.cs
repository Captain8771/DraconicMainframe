using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Microsoft.Extensions.Logging;

namespace CapSharp.Commands;

[SlashCommandGroup("dev", "Developer commands.")]
[SlashRequireOwner]
public class Dev : ApplicationCommandModule
{
    // lol not yet
}