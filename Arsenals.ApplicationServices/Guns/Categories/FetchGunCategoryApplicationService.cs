using Arsenals.Domains.Guns;
using Arsenals.Models;
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

    /// <summary>
    /// 全銃カテゴリー取得
    /// </summary>
    /// <returns></returns>
    public async Task<FetchGunCategoryResponseModel> ExecuteAsync()
    {
        List<GunCategoryModel> categories = await _repository
                                                    .FetchAllAsync()
                                                    .Select(_mapper.Map<GunCategoryModel>)
                                                    .ToListAsync();

        return new FetchGunCategoryResponseModel()
        {
            Error = null,
            Data = categories
        };
    }
}