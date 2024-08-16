namespace Arsenals.Domains.Bullets;

/// <summary>
/// 弾丸IDを生成する
/// </summary>
public interface IBulletIdFactory
{
    Task<BulletId> BuildAsync();
}
