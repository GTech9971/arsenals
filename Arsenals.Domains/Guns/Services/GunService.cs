namespace Arsenals.Domains.Guns.Services;

/// <summary>
/// 銃名称のドメインサービス
/// </summary>
public class GunService
{
    private readonly IGunRepository _repository;

    public GunService(IGunRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        _repository = repository;
    }

    /// <summary>
    /// 銃の名称が既に存在するか確認
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<bool> Exists(GunName name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        IAsyncEnumerable<Gun> guns = _repository.FetchAll();
        return await guns
                        .AnyAsync(x => x.Name.Equals(name));
    }
}
