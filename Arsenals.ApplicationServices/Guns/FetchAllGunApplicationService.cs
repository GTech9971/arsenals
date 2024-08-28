using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Domains.Guns;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<FetchAllGunApplicationService> _logger;

    public FetchAllGunApplicationService(IGunRepository repository,
                                            IConfiguration configuration,
                                            IMapper mapper,
                                            ILogger<FetchAllGunApplicationService> logger)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _repository = repository;
        _configuration = configuration;
        _mapper = mapper;
        _logger = logger;

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
        _logger.LogInformation($"Execute(gunCategoryIdVal = {gunCategoryIdVal}) - Start");
        //_logger.LogMethodStart(() => { };

        IAsyncEnumerable<Gun> guns = _repository.FetchAll();

        if (gunCategoryIdVal != null)
        {
            GunCategoryId gunCategoryId = new GunCategoryId((int)gunCategoryIdVal);
            guns = guns
                    .Where(x => x.Category.Id.Equals(gunCategoryId));
        }

        _logger.LogInformation($"Execute(gunCategoryIdVal = {gunCategoryIdVal}) - End");

        return guns
                .Select(x => _mapper.Map<Gun, GunDto>(x));
    }
}