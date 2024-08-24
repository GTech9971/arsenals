using Arsenals.Infrastructure.Ef;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Arsenals.WebApi.Tests;

public class BaseControllerTest : IClassFixture<PostgreSqlTest>, IDisposable
{
    protected readonly WebApplicationFactory<Program> _factory;
    protected readonly HttpClient _client;

    protected readonly ArsenalDbContext _context;

    public BaseControllerTest(PostgreSqlTest fixture)
    {
        var options = new WebApplicationFactoryClientOptions()
        {
            AllowAutoRedirect = true
        };
        _factory = new CustomWebApplicationFactory(fixture);
        _client = _factory.CreateClient(options);
        _context = fixture.ArsenalDbContext;
    }

    public void Dispose()
    {
        _factory.Dispose();
    }
}
