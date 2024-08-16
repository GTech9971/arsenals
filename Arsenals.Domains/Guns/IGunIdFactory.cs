namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃IDを生成するファクトリー
/// </summary>
public interface IGunIdFactory
{
    Task<GunId> BuildAsync();
}
