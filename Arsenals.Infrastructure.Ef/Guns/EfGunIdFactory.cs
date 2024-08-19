using Arsenals.Domains.Guns;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.Infrastructure.Ef.Guns;

public class EfGunIdFactory : IGunIdFactory
{
    private readonly ArsenalDbContext _context;

    public EfGunIdFactory(ArsenalDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        _context = context;
    }

    public async Task<GunId> BuildAsync()
    {
        GunData? gunData = await _context.Guns
                                            .AsNoTracking()
                                            .OrderBy(x => x.Id)
                                            .FirstOrDefaultAsync();

        if (gunData == null)
        {
            return GunId.FirstId();
        }
        else
        {
            GunId gunId = new GunId(gunData.Id);
            return gunId.Next();
        }
    }
}
