namespace Arsenals.Domains.Guns.Services;

/// <summary>
/// 銃のカテゴリー名称のドメインサービス
/// </summary>
public class GunCategoryNameService
{
    private readonly IGunCategoryRepository _repository;

    public GunCategoryNameService(IGunCategoryRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        _repository = repository;
    }

    /// <summary>
    /// 銃のカテゴリー名称が既に存在するか確認する
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<bool> ExistsAsync(GunCategoryName name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        IAsyncEnumerable<GunCategory> categories = _repository.FetchAll();

        return await categories
                        .AnyAsync(x => x.Name.Equals(name));
    }
}
