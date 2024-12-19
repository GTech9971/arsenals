using Arsenals.Domains.Guns;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.Infrastructure.Ef.Guns;

public class EfGunCategoryRepository : IGunCategoryRepository
{

    private readonly ArsenalDbContext _context;
    private readonly IMapper _mapper;

    public EfGunCategoryRepository(ArsenalDbContext context,
                                    IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));

        _context = context;
        _mapper = mapper;
    }

    public async Task DeleteAsync(GunCategoryId id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));

        GunCategoryData data = await _context.GunCategories
                                                .Where(x => x.Id == id.Value)
                                                .SingleAsync();
        _context.GunCategories.Remove(data);

        await _context.SaveChangesAsync();
    }

    public IAsyncEnumerable<GunCategory> FetchAllAsync()
    {
        return _context.GunCategories
                        .AsNoTracking()
                        .Select(x => _mapper.Map<GunCategory>(x))
                        .AsAsyncEnumerable();
    }

    public async Task<GunCategory?> FetchAsync(GunCategoryId id)
    {
        return await _context.GunCategories
                                .AsNoTracking()
                                .Where(x => x.Id == id.Value)
                                .Select(x => _mapper.Map<GunCategory>(x))
                                .SingleOrDefaultAsync();
    }

    public async Task SaveAsync(GunCategory gunCategory)
    {
        ArgumentNullException.ThrowIfNull(gunCategory, nameof(gunCategory));

        GunCategoryData? found = await _context.GunCategories
                                                    .Where(x => x.Id == gunCategory.Id.Value)
                                                    .SingleOrDefaultAsync();

        if (found == null)
        {
            GunCategoryData gunCategoryData = _mapper.Map<GunCategory, GunCategoryData>(gunCategory);
            await _context.GunCategories.AddAsync(gunCategoryData);
        }
        else
        {
            _mapper.Map(gunCategory, found);
            _context.GunCategories.Update(found);
        }

        await _context.SaveChangesAsync();
    }
}
