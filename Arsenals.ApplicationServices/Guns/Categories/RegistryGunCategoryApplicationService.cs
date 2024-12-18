using System.Transactions;
using Arsenals.Domains.Guns;
using Arsenals.Domains.Guns.Exceptions;
using Arsenals.Domains.Guns.Services;
using Arsenals.Models;

namespace Arsenals.ApplicationServices.Guns;

/// <summary>
/// 銃のカテゴリー登録アプリケーションサービス
/// </summary>
public class RegistryGunCategoryApplicationService
{
    private readonly IGunCategoryRepository _repository;
    private readonly IGunCategoryIdFactory _gunCategoryIdFactory;
    private readonly GunCategoryService _gunCategoryService;

    public RegistryGunCategoryApplicationService(IGunCategoryRepository repository,
                                                IGunCategoryIdFactory gunCategoryIdFactory,
                                                GunCategoryService gunCategoryService)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(gunCategoryIdFactory, nameof(gunCategoryIdFactory));
        ArgumentNullException.ThrowIfNull(gunCategoryService, nameof(gunCategoryService));

        _repository = repository;
        _gunCategoryIdFactory = gunCategoryIdFactory;
        _gunCategoryService = gunCategoryService;
    }

    /// <summary>
    /// 銃のカテゴリーを登録する
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="DuplicateGunCategoryNameException"></exception>
    public async Task<RegistryGunCategoryResponseModel> ExecuteAsync(RegistryGunCategoryRequestModel request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        GunCategoryName gunCategoryName = new GunCategoryName(request.Name);

        bool exists = await _gunCategoryService.ExistsAsync(gunCategoryName);
        if (exists) { throw new DuplicateGunCategoryNameException(gunCategoryName); }

        using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            GunCategoryId gunCategoryId = await _gunCategoryIdFactory.BuildAsync();
            GunCategory gunCategory = new GunCategory(gunCategoryId, gunCategoryName);

            await _repository.SaveAsync(gunCategory);
            transaction.Complete();

            return new RegistryGunCategoryResponseModel()
            {
                Data = new RegistryGunCategoryResponseAllOfDataModel()
                {
                    Id = gunCategoryId.Value
                }
            };
        }
    }
}
