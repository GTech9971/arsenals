using System.Transactions;
using Arsenals.Domains.Bullets;
using Arsenals.Domains.Bullets.Exceptions;
using Arsenals.Domains.Guns;
using Arsenals.Domains.Guns.Exceptions;
using Arsenals.Domains.Guns.Services;
using Arsenals.Models;

namespace Arsenals.ApplicationServices.Guns;

/// <summary>
/// 銃登録アプリケーションサービス
/// </summary>
public class RegistryGunApplicationService
{
    private readonly IGunRepository _repository;
    private readonly GunService _gunService;
    private readonly IGunIdFactory _gunIdFactory;
    private readonly IGunCategoryRepository _gunCategoryRepository;
    private readonly IBulletRepository _bulletRepository;

    public RegistryGunApplicationService(IGunRepository repository,
                                            GunService gunService,
                                            IGunIdFactory gunIdFactory,
                                            IGunCategoryRepository gunCategoryRepository,
                                            IBulletRepository bulletRepository)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(gunService, nameof(gunService));
        ArgumentNullException.ThrowIfNull(gunIdFactory, nameof(gunIdFactory));
        ArgumentNullException.ThrowIfNull(gunCategoryRepository, nameof(gunCategoryRepository));
        ArgumentNullException.ThrowIfNull(bulletRepository, nameof(bulletRepository));

        _repository = repository;
        _gunService = gunService;
        _gunIdFactory = gunIdFactory;
        _gunCategoryRepository = gunCategoryRepository;
        _bulletRepository = bulletRepository;
    }

    /// <summary>
    /// 銃を登録する
    /// </summary>
    /// <param name="request"></param>
    /// <returns>新規生成した銃ID</returns>
    /// <exception cref="DuplicateGunNameException"></exception>
    /// <exception cref="GunCategoryNotFoundException"></exception>
    /// <exception cref="BulletNotFoundException"></exception>
    public async Task<RegistryGunResponseModel> ExecuteAsync(RegistryGunRequestModel request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        GunName gunName = new GunName(request.Name);
        GunCategoryId gunCategoryId = new GunCategoryId(request.CategoryId);
        Capacity capacity = new Capacity(request.Capacity);

        //銃の名前が被っていたら例外
        if (await _gunService.ExistsAsync(gunName)) { throw new DuplicateGunNameException(gunName); }

        GunCategory? gunCategory = await _gunCategoryRepository.FetchAsync(gunCategoryId);
        //カテゴリーが存在しなければ例外
        if (gunCategory == null) { throw new GunCategoryNotFoundException(gunCategoryId); }

        GunId gunId = await _gunIdFactory.BuildAsync();

        Gun.Builder builder = new Gun.Builder(gunId, gunName, gunCategory, capacity);

        //弾丸がリクエストに存在する場合、登録する
        if (request.UseBullets.Any())
        {
            ICollection<Bullet> bullets = [];
            IEnumerable<BulletId> bulletIdList = request.UseBullets
                                                            .Distinct()
                                                            .Select(x => new BulletId(x));
            foreach (BulletId bulletId in bulletIdList)
            {
                Domains.Bullets.Bullet? bullet = await _bulletRepository.FetchAsync(bulletId);
                //弾丸が存在しなければ例外
                if (bullet == null) { throw new BulletNotFoundException(bulletId); }
                bullets.Add(bullet);
            }

            builder.WithBullets(bullets);
        }

        Gun gun = builder.Build();

        using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _repository.SaveAsync(gun);
            transaction.Complete();

            return new RegistryGunResponseModel()
            {
                Data = new RegistryGunResponseAllOfDataModel()
                {
                    Id = gunId.Value
                }
            };
        }
    }
}

