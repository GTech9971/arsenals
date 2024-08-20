using Arsenals.Domains.Bullets;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.Infrastructure.Ef.Bullets;

public class EfBulletRepository : IBulletRepository
{
    private readonly ArsenalDbContext _context;
    private readonly IMapper _mapper;

    public EfBulletRepository(ArsenalDbContext context,
                                IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));

        _context = context;
        _mapper = mapper;
    }

    public async Task DeleteAsync(BulletId id)
    {
        BulletData bulletData = await _context.Bullets
                                    .Where(x => x.Id == id.Value)
                                    .SingleAsync();

        _context.Bullets.Remove(bulletData);
    }

    public IAsyncEnumerable<Bullet> FetchAll()
    {
        return _context.Bullets
                        .AsNoTracking()
                        .Select(x => _mapper.Map<Bullet>(x))
                        .AsAsyncEnumerable();
    }

    public async Task<Bullet?> FetchAsync(BulletId id)
    {
        return await _context.Bullets
                                .AsNoTracking()
                                .Where(x => x.Id == id.Value)
                                .Select(x => _mapper.Map<Bullet>(x))
                                .SingleOrDefaultAsync();
    }

    public async Task SaveAsync(Bullet bullet)
    {
        BulletData? found = await _context.Bullets
                                            .Where(x => x.Id == bullet.Id.Value)
                                            .SingleOrDefaultAsync();
        if (found == null)
        {
            BulletData bulletData = _mapper.Map<Bullet, BulletData>(bullet);
            await _context.Bullets.AddAsync(bulletData);
        }
        else
        {
            _mapper.Map(bullet, found);
            _context.Bullets.Update(found);
        }
    }
}
