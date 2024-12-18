using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Domains.Guns;
using AutoMapper;

namespace Arsenals.ApplicationServices.Guns;

/// <summary>
/// 銃のカテゴリー取得アプリケーションサービス
/// </summary>
public class FetchGunCategoryApplicationService
{
    private readonly IGunCategoryRepository _repository;
    private readonly IMapper _mapper;

    public FetchGunCategoryApplicationService(IGunCategoryRepository repository,
                                                IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        _repository = repository;
        _mapper = mapper;
    }

    public IAsyncEnumerable<GunCategoryDto> ExecuteAsync()
    {
        IAsyncEnumerable<GunCategory> categories = _repository.FetchAllAsync();
        return categories
                .Select(x => _mapper.Map<GunCategoryDto>(x));
    }
}