using System.Transactions;
using Arsenals.Domains.Bullets;
using Arsenals.Domains.Bullets.Exceptions;
using Arsenals.Domains.Bullets.Services;
using Arsenals.Models;

namespace Arsenals.ApplicationServices.Bullets;

/// <summary>
/// 弾丸登録アプリケーションサービス
/// </summary>
public class RegistryBulletApplicationService
{
    private readonly IBulletRepository _repository;
    private readonly BulletService _bulletService;
    private readonly IBulletIdFactory _bulletIdFactory;

    public RegistryBulletApplicationService(IBulletRepository repository,
                                            BulletService bulletService,
                                            IBulletIdFactory bulletIdFactory)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(bulletService, nameof(bulletService));
        ArgumentNullException.ThrowIfNull(bulletIdFactory, nameof(bulletIdFactory));

        _repository = repository;
        _bulletService = bulletService;
        _bulletIdFactory = bulletIdFactory;
    }

    /// <summary>
    /// 弾丸登録
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="DuplicateBulletNameException"></exception>
    public async Task<RegistryBulletResponseModel> ExecuteAsync(RegistryBulletRequestModel request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        BulletName bulletName = new BulletName(request.Name);
        Damage damage = new Damage(request.Damage);

        // 名前が既に登録されている
        if (await _bulletService.ExistsNameAsync(bulletName)) { throw new DuplicateBulletNameException(bulletName); }

        BulletId bulletId = await _bulletIdFactory.BuildAsync();

        Bullet bullet = new Bullet(bulletId, bulletName, damage);

        using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _repository.SaveAsync(bullet);
            transaction.Complete();

            return new RegistryBulletResponseModel()
            {
                Data = new RegistryBulletResponseAllOfDataModel()
                {
                    Id = bulletId.Value
                }
            };
        }
    }

}
