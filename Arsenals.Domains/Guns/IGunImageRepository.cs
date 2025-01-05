namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃の画像の永続化を行うレポジトリ
/// </summary>
public interface IGunImageRepository
{
    Task<GunImage?> FetchAsync(GunId id);

    Task DeleteAsync(GunId id);

    Task SaveAsync(GunId id, MemoryStream data);
}
