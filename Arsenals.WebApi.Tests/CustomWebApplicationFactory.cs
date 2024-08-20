using Arsenals.Infrastructure.Ef;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Arsenals.WebApi.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _connectString;

    public CustomWebApplicationFactory(PostgreSqlTest fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture, nameof(fixture));
        _connectString = fixture._connectString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var context = services.Single(x => typeof(DbContextOptions<ArsenalDbContext>) == x.ServiceType);
            services.Remove(context);

            services.AddDbContext<ArsenalDbContext>((_, options) => options.UseNpgsql(_connectString));
        });
    }
}
