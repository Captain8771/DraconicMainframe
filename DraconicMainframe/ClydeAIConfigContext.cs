using Microsoft.EntityFrameworkCore;

namespace DraconicMainframe;

// postgres microsoft EFCore
public class ClydeAIConfigContext : DbContext 
{
    public DbSet<ClydeAIConfig> Configs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
        string port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
        string user = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
        string password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "example"; // this is my password lmao :trolley:
        
        // check if host is "NO_DB" and if so, don't use a database"
        if (host == "NO_DB")
        {
            return;
        }

        // connect to the database
        optionsBuilder.UseNpgsql($"Host={host};Port={port};Username={user};Password={password};Database=clydeai");
    }

}

// postgres microsoft EFCore
[PrimaryKey(nameof(userId))]
public class ClydeAIConfig
{
    public ulong userId { get; set; }
    public ulong privateChannelId { get; set; }
}
