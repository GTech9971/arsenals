using Arsenals.Domains.Guns;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.Infrastructure.Ef.Guns;

public class EfGunRepository : IGunRepository
{
    private readonly ArsenalDbContext _context;
    private readonly IMapper _mapper;

    public EfGunRepository(ArsenalDbContext context,
                            IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));

        _context = context;
        _mapper = mapper;
    }

    public async Task DeleteAsync(GunId gunId)
    {
        GunData gunData = await _context.Guns
                                            .Where(x => x.Id == gunId.Value)
                                            .SingleAsync();
        _context.Guns.Remove(gunData);
    }

    public IAsyncEnumerable<Gun> FetchAllAsync()
    {
        return _context.Guns
                        .AsNoTracking()
                        .Include(x => x.GunCategoryData)
                        .Include(x => x.BulletDataList)
                        .AsSplitQuery()
                        .Select(x => _mapper.Map<Gun>(x))
                        .AsAsyncEnumerable();
    }

    public async Task<Gun?> FetchAsync(GunId gunId)
    {
        return await _context.Guns
                                .AsNoTracking()
                                .Where(x => x.Id == gunId.Value)
                                .Include(x => x.GunCategoryData)
                                .Include(x => x.BulletDataList)
                                .AsSplitQuery()
                                .Select(x => _mapper.Map<Gun>(x))
                                .SingleOrDefaultAsync();
    }

    public async Task SaveAsync(Gun gun)
    {
        GunData? found = await _context.Guns
                                    .Where(x => x.Id == gun.Id.Value)
                                    .Include(x => x.GunCategoryData)
                                    .Include(x => x.BulletDataList)
                                    .AsSplitQuery()
                                    .SingleOrDefaultAsync();

        if (found == null)
        {
            GunData gunData = _mapper.Map<Gun, GunData>(gun);
            //Mapperでmapするとインスタンスを新規作成してしまうため、新しいデータとして認識されてしまう
            //既存データからのリレーションと認識させるため、以下の代入を行う
            if (gun.UseableBullets.Any())
            {
                IEnumerable<string> gunIdList = gun.UseableBullets
                                                    .Select(x => x.Id.Value);

                //再割り当てをさせないためにAsNoTrackingを記載してはいけない
                gunData.BulletDataList = await _context.Bullets
                                                            .Where(x => gunIdList.Contains(x.Id))
                                                            .ToListAsync();
            }
            await _context.Guns.AddAsync(gunData);
        }
        else
        {
            _mapper.Map(gun, found);
            _context.Guns.Update(found);
        }

        await _context.SaveChangesAsync();
    }
}
