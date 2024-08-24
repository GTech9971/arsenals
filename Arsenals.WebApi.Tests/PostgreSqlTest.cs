using Arsenals.Infrastructure.Ef;
using Arsenals.Infrastructure.Ef.Bullets;
using Arsenals.Infrastructure.Ef.Guns;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Arsenals.WebApi.Tests;

public class PostgreSqlTest : IAsyncLifetime
{

    private readonly PostgreSqlContainer _container;
    private readonly ArsenalDbContext _context;

    public readonly string _connectString;

    public PostgreSqlTest()
    {
        _container = new PostgreSqlBuilder()
                            .WithImage("postgres")
                            .WithDatabase("arsenals")
                            .WithUsername("test")
                            .WithPassword("test")
                            .WithPortBinding(5431, 5432)
                            .WithCleanUp(true)
                            .Build();

        _container.StartAsync().GetAwaiter().GetResult();
        _connectString = _container.GetConnectionString();
        var options = new DbContextOptionsBuilder<ArsenalDbContext>()
                            .UseNpgsql(_connectString)
                            .Options;

        _context = new ArsenalDbContext(options);
    }

    public Task DisposeAsync()
    {
        _context.Dispose();
        return _container.DisposeAsync().AsTask();
    }

    public async Task InitializeAsync()
    {
        await _context.Database.MigrateAsync();

        //テストデータ作成
        await _context.GunCategories.AddAsync(new GunCategoryData() { Id = 100, Name = "ハンドガン" });
        await _context.Guns.AddAsync(new GunData() { Id = 100, Name = "M1911A1", Capacity = 6, GunCategoryDataId = 100 });
        await _context.Bullets.AddAsync(new BulletData() { Id = 100, Name = "45ACP", Damage = 12 });
        _context.SaveChanges();
    }
}
