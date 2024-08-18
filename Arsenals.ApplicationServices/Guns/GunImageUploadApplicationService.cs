using Arsenals.Domains;
using Arsenals.Domains.Guns;
using Arsenals.Domains.Guns.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Arsenals.ApplicationServices.Guns;

/// <summary>
/// 銃の画像をアップロードするアプリケーションサービス
/// </summary>
public class GunImageUploadApplicationService
{
    private static readonly string KEY = "images:root";

    private readonly IGunImageRepository _repository;
    private readonly IGunRepository _gunRepository;
    private readonly IFileManager _fileManager;

    private readonly string _rootVal;

    public GunImageUploadApplicationService(IGunImageRepository repository,
                                            IGunRepository gunRepository,
                                            IConfiguration configuration,
                                            IFileManager fileManager)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(gunRepository, nameof(gunRepository));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNull(fileManager, nameof(fileManager));

        _rootVal = configuration[KEY]!;
        ArgumentNullException.ThrowIfNull(_rootVal);

        _repository = repository;
        _gunRepository = gunRepository;
        _fileManager = fileManager;
    }

    /// <summary>
    /// 銃の画像を登録する
    /// </summary>
    /// <param name="gunIdVal"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="GunNotFoundException"></exception>
    public async Task<Uri> ExecuteAsync(int gunIdVal, MemoryStream data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));

        if (_fileManager.ExistsDirectory(_rootVal)) { _fileManager.CreateDirectory(_rootVal); }

        GunId gunId = new GunId(gunIdVal);

        Gun? found = await _gunRepository.FetchAsync(gunId);
        if (found == null) { throw new GunNotFoundException(gunId); }

        using (data)
        {
            await _repository.DeleteAsync(gunId);
            await _repository.SaveAsync(gunId, data);
            return gunId.ImageUrl(_rootVal);
        }
    }
}
