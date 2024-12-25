using Arsenals.Infrastructure.Ef;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.Infrastructure.Migrations.Batch;

public class Program
{
    public static int Main(string[] args)
    {
        ArsenalDbContextFactory factory = new ArsenalDbContextFactory();
        using ArsenalDbContext context = factory.CreateDbContext(args);

        Console.WriteLine($"Connect Status:{context.Database.CanConnect()}");
        context.Database.Migrate();
        Console.WriteLine("Migration done.");

        // 0
        return Environment.ExitCode;
    }
}