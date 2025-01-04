using Arsenals.Domains.Guns;
using Arsenals.Models;
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
    public async Task<FetchGunsResponseModel> ExecuteAsync(string? gunCategoryIdVal)
    {
        IAsyncEnumerable<Gun> guns = _repository.FetchAllAsync();

        if (gunCategoryIdVal != null)
        {
            GunCategoryId gunCategoryId = new GunCategoryId(gunCategoryIdVal);
            guns = guns
                    .Where(x => x.Category.Id.Equals(gunCategoryId));
        }

        List<GunModel> gunModels = await guns
                                            .Select(_mapper.Map<GunModel>)
                                            .ToListAsync();

        return new FetchGunsResponseModel()
        {
            Error = null,
            Data = gunModels
        };
    }
}