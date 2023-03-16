using DSharpPlus.SlashCommands;

namespace DraconicMainframe.Utils;

public class Checks
{
    public sealed class OwnerOnlyAttribute : SlashCheckBaseAttribute
    {
        public override async Task<bool> ExecuteChecksAsync(InteractionContext ctx)
        {
            return DraconicMainframe.Config.OwnerIds.Contains(ctx.User.Id);
        }
    }
}