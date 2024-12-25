using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Arsenals.Infrastructure.Ef;

public class ArsenalDbContextFactory : IDesignTimeDbContextFactory<ArsenalDbContext>
{
    public ArsenalDbContext CreateDbContext(string[] args)
    {
        string environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";
        Console.WriteLine($"DOTNET_ENVIRONMENT:{environment}");

        var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("dbsettings.json", optional: false, reloadOnChange: true)
                                .AddJsonFile($"dbsettings.{environment}.json", optional: false)
                                .AddEnvironmentVariables()
                                .Build();

        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionBuilder = new DbContextOptionsBuilder<ArsenalDbContext>();
        optionBuilder.UseNpgsql(connectionString, x => x.MigrationsHistoryTable("__EFMigrationsHistory", DbConst.SchemaName));

        return new ArsenalDbContext(optionBuilder.Options);
    }
}
