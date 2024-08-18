using System.Transactions;
using Arsenals.Domains.Guns;
using Arsenals.Domains.Guns.Exceptions;

namespace Arsenals.ApplicationServices.Guns;

/// <summary>
/// 銃削除アプリケーションサービス
/// </summary>
public class DeleteGunApplicationService
{
    private readonly IGunRepository _repository;

    public DeleteGunApplicationService(IGunRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));

        _repository = repository;
    }

    /// <summary>
    /// 銃削除
    /// </summary>
    /// <param name="gunIdVal"></param>
    /// <returns></returns>
    /// <exception cref="GunNotFoundException"></exception>
    public async Task ExecuteAsync(int gunIdVal)
    {
        GunId gunId = new GunId(gunIdVal);

        Gun? found = await _repository.FetchAsync(gunId);
        if (found == null)
        {
            throw new GunNotFoundException(gunId);
        }

        using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _repository.DeleteAsync(gunId);
            transaction.Complete();
        }
    }
}
