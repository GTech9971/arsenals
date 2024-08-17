using System.Text.Json.Serialization;
using System.Transactions;
using Arsenals.Domains.Guns;
using Arsenals.Domains.Guns.Exceptions;
using Arsenals.Domains.Guns.Services;
using AutoMapper;

namespace Arsenals.ApplicationServices.Guns;

/// <summary>
/// 銃のカテゴリー登録アプリケーションサービス
/// </summary>
public class RegistryGunCategoryApplicationService
{
    private readonly IGunCategoryRepository _repository;
    private readonly IGunCategoryIdFactory _gunCategoryIdFactory;
    private readonly IMapper _mapper;
    private readonly GunCategoryNameService _gunCategoryNameService;

    public RegistryGunCategoryApplicationService(IGunCategoryRepository repository,
                                                IGunCategoryIdFactory gunCategoryIdFactory,
                                                IMapper mapper,
                                                GunCategoryNameService gunCategoryNameService)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(gunCategoryIdFactory, nameof(gunCategoryIdFactory));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        ArgumentNullException.ThrowIfNull(gunCategoryNameService, nameof(gunCategoryNameService));

        _repository = repository;
        _gunCategoryIdFactory = gunCategoryIdFactory;
        _mapper = mapper;
        _gunCategoryNameService = gunCategoryNameService;
    }

    /// <summary>
    /// 銃のカテゴリーを登録する
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="DuplicateGunCategoryNameException"></exception>
    public async Task<RegistryGunCategoryResponseDto> ExecuteAsync(RegistryGunCategoryRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        GunCategoryName gunCategoryName = new GunCategoryName(request.Name);

        using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            bool exists = await _gunCategoryNameService.ExistsAsync(gunCategoryName);
            if (exists) { throw new DuplicateGunCategoryNameException(gunCategoryName); }

            GunCategoryId gunCategoryId = await _gunCategoryIdFactory.BuildAsync();
            GunCategory gunCategory = new GunCategory(gunCategoryId, gunCategoryName);

            await _repository.SaveAsync(gunCategory);
            transaction.Complete();

            return _mapper.Map<GunCategoryId, RegistryGunCategoryResponseDto>(gunCategoryId);
        }
    }
}

public class RegistryGunCategoryRequestDto
{
    [JsonPropertyName("name")]
    [JsonRequired]
    public string Name { get; set; } = null!;
}


public class RegistryGunCategoryResponseDto
{
    [JsonPropertyName("id")]
    [JsonRequired]
    public int Id { get; set; }
}

public class RegistryGunCategoryResponseDtoMappingProfile : Profile
{
    public RegistryGunCategoryResponseDtoMappingProfile()
    {
        CreateMap<GunCategoryId, RegistryGunCategoryResponseDto>()
        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Value));
    }
}