namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃のカテゴリーIDを生成する
/// </summary>
public interface IGunCategoryIdFactory
{
    Task<GunCategoryId> BuildAsync();
}
