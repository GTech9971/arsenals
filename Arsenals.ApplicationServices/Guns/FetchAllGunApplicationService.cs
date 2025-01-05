using Arsenals.Domains.Guns;
using Arsenals.Models;
using AutoMapper;

namespace Arsenals.ApplicationServices.Guns;

/// <summary>
/// 全ての銃を取得する
/// </summary>
public class FetchAllGunApplicationService
{
    private readonly IGunRepository _repository;
    private readonly IMapper _mapper;

    public FetchAllGunApplicationService(IGunRepository repository,
                                            IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));

        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// 全ての銃を取得する
    /// </summary>
    /// <param name="gunCategoryIdVal"></param>
    /// <returns></returns>
    public async Task<FetchGunsResponseModel> ExecuteAsync(string? gunCategoryIdVal)
    {
        IAsyncEnumerable<Gun> guns = _repository.FetchAllAsync();

        if (gunCategoryIdVal != null)
        {
            GunCategoryId gunCategoryId = new GunCategoryId(gunCategoryIdVal);
            guns = guns
                    .Where(x => x.Category.Id.Equals(gunCategoryId));
        }

        List<GunModel> gunModels = await guns
                                            .Select(_mapper.Map<GunModel>)
                                            .ToListAsync();

        return new FetchGunsResponseModel()
        {
            Error = null,
            Data = gunModels
        };
    }
}