using System.Text.Json.Serialization;
using Arsenals.Domains.Bullets;
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
    public async Task<List<FetchAllGunResponseDto>> ExecuteAsync(int? gunCategoryIdVal)
    {
        IAsyncEnumerable<Gun> guns = _repository.FetchAll();

        if (gunCategoryIdVal != null)
        {
            GunCategoryId gunCategoryId = new GunCategoryId((int)gunCategoryIdVal);
            guns = guns
                    .Where(x => x.Category.Id.Equals(gunCategoryId));
        }

        return await guns
                        .Select(x => _mapper.Map<Gun, FetchAllGunResponseDto>(x))
                        .ToListAsync();
    }
}

public class FetchAllGunResponseDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("category")]
    public GunCategoryDto Category { get; set; } = null!;

    [JsonPropertyName("capacity")]
    public int Capacity { get; set; }

    [JsonPropertyName("imageUrl")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("bullets")]
    public IEnumerable<BulletDto> Bullets { get; set; } = Enumerable.Empty<BulletDto>();
}

public class GunCategoryDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
}

public class BulletDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    [JsonPropertyName("damage")]
    public int Damage { get; set; }
}

public class FetchAllGunResponseDtoMappingProfile : Profile
{
    public FetchAllGunResponseDtoMappingProfile()
    {
        CreateMap<Bullet, BulletDto>()
        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id.Value))
        .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name.Value))
        .ForMember(dst => dst.Damage, opt => opt.MapFrom(src => src.Damage.Value));

        CreateMap<GunCategory, GunCategoryDto>()
        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id.Value))
        .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name.Value));

        CreateMap<Gun, FetchAllGunResponseDto>()
        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id.Value))
        .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name.Value))
        .ForMember(dst => dst.Capacity, opt => opt.MapFrom(src => src.Capacity.Value))
        .ForMember(dst => dst.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));
    }
}