namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃の永続化操作を行うレポジトリ
/// </summary>
public interface IGunRepository
{
    IAsyncEnumerable<Gun> FetchAll();

    Task<Gun?> FetchAsync(GunId gunId);

    Task SaveAsync(Gun gun);

    Task DeleteAsync(GunId gunId);
}
