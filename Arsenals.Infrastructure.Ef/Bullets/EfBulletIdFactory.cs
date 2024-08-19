using Arsenals.Domains.Bullets;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.Infrastructure.Ef.Bullets;

public class EfBulletIdFactory : IBulletIdFactory
{
    private readonly ArsenalDbContext _context;

    public EfBulletIdFactory(ArsenalDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        _context = context;
    }

    public async Task<BulletId> BuildAsync()
    {
        BulletData? bulletData = await _context.Bullets
                                        .AsNoTracking()
                                        .OrderBy(x => x.Id)
                                        .FirstOrDefaultAsync();

        if (bulletData == null)
        {
            return BulletId.FirstId();
        }
        else
        {
            BulletId bulletId = new BulletId(bulletData.Id);
            return bulletId.Next();
        }
    }
}
