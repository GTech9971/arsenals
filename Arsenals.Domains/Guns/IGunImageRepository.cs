namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃の画像の永続化を行うレポジトリ
/// </summary>
public interface IGunImageRepository
{
    Task<GunImage?> FetchAsync(GunImage gunImage);

    Task DeleteAsync(GunImage gunImage);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gunId"></param>
    /// <param name="extension"></param>
    /// <param name="data"></param>
    /// <returns>銃画像ID</returns>
    Task<GunImage> SaveAsync(GunId gunId, string extension, MemoryStream data);
}
