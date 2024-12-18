using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Arsenals.Infrastructure.Ef;

public class ArsenalDbContextFactory : IDesignTimeDbContextFactory<ArsenalDbContext>
{
    public ArsenalDbContext CreateDbContext(string[] args)
    {
        string environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";
        var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                                .AddEnvironmentVariables()
                                .Build();

        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionBuilder = new DbContextOptionsBuilder<ArsenalDbContext>();
        optionBuilder.UseNpgsql(connectionString);

        return new ArsenalDbContext(optionBuilder.Options);
    }
}
