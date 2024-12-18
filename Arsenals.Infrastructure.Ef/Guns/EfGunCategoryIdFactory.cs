using Arsenals.Domains.Guns;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.Infrastructure.Ef.Guns;

public class EfGunCategoryIdFactory : IGunCategoryIdFactory
{
    private readonly ArsenalDbContext _context;

    public EfGunCategoryIdFactory(ArsenalDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        _context = context;
    }

    public async Task<GunCategoryId> BuildAsync()
    {
        GunCategoryData? gunCategoryData = await _context.GunCategories
                                                            .AsNoTracking()
                                                            .OrderByDescending(x => x.Id)
                                                            .FirstOrDefaultAsync();

        if (gunCategoryData == null)
        {
            return new GunCategoryId("C-0001");
        }
        else
        {
            GunCategoryId gunCategoryId = new GunCategoryId(gunCategoryData.Id);
            return gunCategoryId.Next();
        }
    }
}
