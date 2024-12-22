namespace Arsenals.Domains.Bullets;

/// <summary>
/// 弾丸の永続化を行うレポジトリ
/// </summary>
public interface IBulletRepository
{
    IAsyncEnumerable<Bullet> FetchAllAsync();

    Task<Bullet?> FetchAsync(BulletId id);

    Task SaveAsync(Bullet bullet);

    Task DeleteAsync(BulletId id);
}
