namespace Arsenals.Domains.Guns.Services;

/// <summary>
/// 銃のカテゴリーのドメインサービス
/// </summary>
public class GunCategoryService
{
    private readonly IGunCategoryRepository _repository;
    private readonly IGunRepository _gunRepository;

    public GunCategoryService(IGunCategoryRepository repository,
                                IGunRepository gunRepository)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(gunRepository, nameof(gunRepository));
        _repository = repository;
        _gunRepository = gunRepository;
    }

    /// <summary>
    /// 銃のカテゴリー名称が既に存在するか確認する
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<bool> ExistsAsync(GunCategoryName name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        IAsyncEnumerable<GunCategory> categories = _repository.FetchAllAsync();

        return await categories
                        .AnyAsync(x => x.Name.Equals(name));
    }

    /// <summary>
    /// 銃のカテゴリーを削除可能かどうか確認する
    /// 1件でも銃にカテゴリーが紐づいている場合, false
    /// </summary>
    /// <param name="gunCategoryId"></param>
    /// <returns></returns>
    public async Task<bool> CanDeleteAsync(GunCategoryId gunCategoryId)
    {
        ArgumentNullException.ThrowIfNull(gunCategoryId, nameof(gunCategoryId));

        return await _gunRepository.FetchAllAsync()
                                    .AnyAsync(x => x.Category.Id.Equals(gunCategoryId)) == false;

    }
}
