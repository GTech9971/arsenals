using System.Transactions;
using Arsenals.Domains;
using Arsenals.Domains.Guns;
using Arsenals.Domains.Guns.Exceptions;
using Arsenals.Models;
using Microsoft.Extensions.Configuration;

namespace Arsenals.ApplicationServices.Guns;

/// <summary>
/// 銃の画像をアップロードするアプリケーションサービス
/// </summary>
public class GunImageUploadApplicationService
{
    private readonly IGunImageRepository _repository;
    private readonly IGunRepository _gunRepository;
    private readonly IFileManager _fileManager;
    private readonly IConfiguration _configuration;

    private readonly string _root;

    public GunImageUploadApplicationService(IGunImageRepository repository,
                                            IGunRepository gunRepository,
                                            IConfiguration configuration,
                                            IFileManager fileManager)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(gunRepository, nameof(gunRepository));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNull(fileManager, nameof(fileManager));

        _repository = repository;
        _gunRepository = gunRepository;
        _fileManager = fileManager;
        _configuration = configuration;

        string? rootVal = _configuration.GetGunImageRoot();
        ArgumentNullException.ThrowIfNullOrWhiteSpace(rootVal, nameof(rootVal));
        _root = rootVal;

        ArgumentNullException.ThrowIfNullOrWhiteSpace(_configuration.GetGunImageDownloadUrl(), nameof(_configuration));
    }

    /// <summary>
    /// 銃の画像を登録する
    /// </summary>
    /// <param name="gunIdVal"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="GunNotFoundException"></exception>
    public async Task<UploadGunImageResponseModel> ExecuteAsync(string gunIdVal, string extension, MemoryStream data)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(gunIdVal, nameof(gunIdVal));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(extension, nameof(extension));
        ArgumentNullException.ThrowIfNull(data, nameof(data));

        GunId gunId = new GunId(gunIdVal);
        string path = Path.Combine(_root, gunId.Value);

        if (_fileManager.ExistsDirectory(path) == false)
        {
            _fileManager.CreateDirectory(path);
        }


        Gun? found = await _gunRepository.FetchAsync(gunId);
        if (found == null) { throw new GunNotFoundException(gunId); }


        using (data)
        using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            // ファイル保存・銃画像DB登録
            GunImage gunImage = await _repository.SaveAsync(gunId, extension, data);
            found.ChangeGunImage(gunImage);
            // 銃テーブルに画像IDを紐付け
            await _gunRepository.SaveAsync(found);

            transaction.Complete();

            return new UploadGunImageResponseModel()
            {
                Error = null,
                Data = new UploadGunImageResponseAllOfDataModel()
                {
                    Url = gunImage.DownloadUrl(gunId, _configuration.GetGunImageDownloadUrl()!).ToString()
                }
            };
        }
    }
}
