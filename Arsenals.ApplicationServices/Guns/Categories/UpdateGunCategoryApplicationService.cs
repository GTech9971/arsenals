using System.Text.Json.Serialization;
using System.Transactions;
using Arsenals.Domains.Guns;
using Arsenals.Domains.Guns.Exceptions;
using Arsenals.Domains.Guns.Services;

namespace Arsenals.ApplicationServices.Guns;

/// <summary>
/// 銃のカテゴリーを更新するアプリケーションサービス
/// </summary>
public class UpdateGunCategoryApplicationService
{
    private readonly IGunCategoryRepository _repository;
    private readonly GunCategoryService _gunCategoryService;

    public UpdateGunCategoryApplicationService(IGunCategoryRepository repository,
                                                GunCategoryService gunCategoryService)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(gunCategoryService, nameof(gunCategoryService));

        _repository = repository;
        _gunCategoryService = gunCategoryService;
    }

    /// <summary>
    /// 銃のカテゴリーを更新する
    /// </summary>
    /// <param name="categoryId"></param>
    /// <param name="fieldMasks"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="GunCategoryNotFoundException"></exception>
    public async Task ExecuteAsync(string categoryId, IEnumerable<string> fieldMasks, UpdateGunCategoryRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(fieldMasks, nameof(fieldMasks));
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        if (fieldMasks.Contains("name") == false) { throw new ArgumentException("フィールドマスクに有効な値が存在しません。", nameof(fieldMasks)); }

        GunCategoryId gunCategoryId = new GunCategoryId(categoryId);
        GunCategory? found = await _repository.FetchAsync(gunCategoryId);

        if (found == null) { throw new GunCategoryNotFoundException(gunCategoryId); }

        GunCategoryName gunCategoryName = new GunCategoryName(request.Name);
        if (await _gunCategoryService.ExistsAsync(gunCategoryName))
        {
            throw new ArgumentException($"既にカテゴリー名:{gunCategoryName}が存在します。", nameof(request.Name));
        }

        found.ChangeName(gunCategoryName);

        using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _repository.SaveAsync(found);
            transaction.Complete();
        }
    }

}

public class UpdateGunCategoryRequestDto
{
    [JsonRequired]
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
}
