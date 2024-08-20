using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Domains.Guns;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace Arsenals.ApplicationServices.Guns;

/// <summary>
/// 全ての銃を取得する
/// </summary>
public class FetchAllGunApplicationService
{

    private static readonly string KEY = "images:root";
    private readonly string _root;

    private readonly IGunRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public FetchAllGunApplicationService(IGunRepository repository,
                                            IConfiguration configuration,
                                            IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        _repository = repository;
        _configuration = configuration;
        _mapper = mapper;

        _root = _configuration[KEY]!;
        ArgumentNullException.ThrowIfNullOrWhiteSpace(_root, nameof(_root));
    }

    /// <summary>
    /// 全ての銃を取得する
    /// </summary>
    /// <param name="gunCategoryIdVal"></param>
    /// <returns></returns>
    public IAsyncEnumerable<GunDto> Execute(int? gunCategoryIdVal)
    {
        IAsyncEnumerable<Gun> guns = _repository.FetchAll();

        if (gunCategoryIdVal != null)
        {
            GunCategoryId gunCategoryId = new GunCategoryId((int)gunCategoryIdVal);
            guns = guns
                    .Where(x => x.Category.Id.Equals(gunCategoryId));
        }

        return guns
                .Select(x => _mapper.Map<Gun, GunDto>(x));
    }
}