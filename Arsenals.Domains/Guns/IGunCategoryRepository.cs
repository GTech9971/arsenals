namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃のカテゴリーの永続化を行うレポジトリ
/// </summary>
public interface IGunCategoryRepository
{
    IAsyncEnumerable<GunCategory> FetchAll();

    Task<GunCategory?> FetchAsync(GunCategoryId id);

    Task SaveAsync(GunCategory gunCategory);

    Task DeleteAsync(GunCategoryId id);
}
