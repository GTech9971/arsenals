using System.Transactions;
using Arsenals.Domains.Guns;
using Arsenals.Domains.Guns.Exceptions;
using Arsenals.Domains.Guns.Services;

namespace Arsenals.ApplicationServices.Guns;

/// <summary>
/// 銃のカテゴリー削除アプリケーションサービス
/// </summary>
public class DeleteGunCategoryApplicationService
{
    private readonly IGunCategoryRepository _repository;
    private readonly GunCategoryService _gunCategoryService;

    public DeleteGunCategoryApplicationService(IGunCategoryRepository repository,
                                                GunCategoryService gunCategoryService)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(gunCategoryService, nameof(gunCategoryService));

        _repository = repository;
        _gunCategoryService = gunCategoryService;
    }

    /// <summary>
    /// 銃のカテゴリーを削除する
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    /// <exception cref="GunCategoryNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task ExecuteAsync(string categoryId)
    {
        GunCategoryId gunCategoryId = new GunCategoryId(categoryId);

        GunCategory? found = await _repository.FetchAsync(gunCategoryId);
        if (found == null) { throw new GunCategoryNotFoundException(gunCategoryId); }

        if (await _gunCategoryService.CanDeleteAsync(gunCategoryId) == false)
        {
            throw new InvalidOperationException($"カテゴリーID:{gunCategoryId}は銃に紐づいているため削除できません");
        }

        using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _repository.DeleteAsync(gunCategoryId);
            transaction.Complete();
        }
    }
}
