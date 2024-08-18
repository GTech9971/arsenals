using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Domains.Guns;
using Arsenals.Domains.Guns.Exceptions;
using AutoMapper;

namespace Arsenals.ApplicationServices.Guns;

/// <summary>
/// 銃取得アプリケーションサービス
/// </summary>
public class FetchGunApplicationService
{
    private readonly IGunRepository _repository;
    private readonly IMapper _mapper;

    public FetchGunApplicationService(IGunRepository repository,
                                        IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));

        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// 銃を取得する
    /// </summary>
    /// <param name="gunIdVal"></param>
    /// <returns></returns>
    /// <exception cref="GunNotFoundException"></exception>
    public async Task<GunDto> ExecuteAsync(int gunIdVal)
    {
        GunId gunId = new GunId(gunIdVal);

        Gun? found = await _repository.FetchAsync(gunId);
        if (found == null)
        {
            throw new GunNotFoundException(gunId);
        }

        return _mapper.Map<Gun, GunDto>(found);
    }

}