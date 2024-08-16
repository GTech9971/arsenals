using Arsenals.Domains.Guns;

namespace Arsenals.Domains.Bullets.Services;

/// <summary>
/// 弾丸のドメインサービス
/// </summary>
public class BulletService
{
    private readonly IBulletRepository _repository;
    private readonly IGunRepository _gunRepository;

    public BulletService(IBulletRepository repository,
                            IGunRepository gunRepository)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(gunRepository, nameof(gunRepository));
        _repository = repository;
        _gunRepository = gunRepository;
    }

    /// <summary>
    /// 弾丸名称が既に存在するか確認する
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<bool> ExistsNameAsync(BulletName name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        IAsyncEnumerable<Bullet> bullets = _repository.FetchAll();
        return await bullets
                        .AnyAsync(x => x.Name.Equals(name)) == false;
    }

    /// <summary>
    /// 弾丸を削除可能か調べる
    /// </summary>
    /// <param name="bulletId"></param>
    /// <returns></returns>
    public async Task<bool> CanDeleteAsync(BulletId bulletId)
    {
        ArgumentNullException.ThrowIfNull(bulletId, nameof(bulletId));

        IAsyncEnumerable<Gun> guns = _gunRepository.FetchAll();
        //弾丸を使用している
        //削除対象の弾丸のみ紐づいている場合、削除不可
        return await guns
                        .Where(x => x.UseableBullets.Any(b => b.Id.Equals(bulletId)))
                        .Where(x => x.UseableBullets.Length == 1)
                        .AnyAsync() == false;

    }
}
