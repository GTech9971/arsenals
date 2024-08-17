using System.Text.Json.Serialization;
using Arsenals.Domains.Guns;
using AutoMapper;

namespace Arsenals.ApplicationServices.Guns;

/// <summary>
/// 銃のカテゴリー取得アプリケーションサービス
/// </summary>
public class FetchGunCategoryApplicationService
{
    private readonly IGunCategoryRepository _repository;
    private readonly IMapper _mapper;

    public FetchGunCategoryApplicationService(IGunCategoryRepository repository,
                                                IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        _repository = repository;
        _mapper = mapper;
    }

    public IAsyncEnumerable<FetchGunCategoryResponseDto> Execute()
    {
        IAsyncEnumerable<GunCategory> categories = _repository.FetchAll();
        return categories
                .Select(x => _mapper.Map<FetchGunCategoryResponseDto>(x));
    }
}


public class FetchGunCategoryResponseDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
}

public class FetchGunCategoryResponseDtoMappingProfile : Profile
{
    public FetchGunCategoryResponseDtoMappingProfile()
    {
        CreateMap<GunCategory, FetchGunCategoryResponseDto>()
        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id.Value))
        .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name.Value));
    }
}