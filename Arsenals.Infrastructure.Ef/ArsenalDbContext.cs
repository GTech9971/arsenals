using Arsenals.Infrastructure.Ef.Bullets;
using Arsenals.Infrastructure.Ef.Guns;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.Infrastructure.Ef;

public class ArsenalDbContext : DbContext
{
    public ArsenalDbContext() : base() { }

    public ArsenalDbContext(DbContextOptions options) : base(options) { }

    public DbSet<GunData> Guns => Set<GunData>();
    public DbSet<GunCategoryData> GunCategories => Set<GunCategoryData>();
    public DbSet<BulletData> Bullets => Set<BulletData>();
}
