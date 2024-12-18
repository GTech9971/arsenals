using System.Text.Json.Serialization;
using System.Transactions;
using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Domains.Bullets;
using Arsenals.Domains.Bullets.Exceptions;
using Arsenals.Domains.Guns;
using Arsenals.Domains.Guns.Exceptions;
using Arsenals.Domains.Guns.Services;
using AutoMapper;

namespace Arsenals.ApplicationServices.Guns;

/// <summary>
/// 銃更新アプリケーションサービス
/// </summary>
public class UpdateGunApplicationService
{
    private readonly IGunRepository _repository;
    private readonly GunService _gunService;
    private readonly IGunCategoryRepository _gunCategoryRepository;
    private readonly IBulletRepository _bulletRepository;
    private readonly IMapper _mapper;

    public UpdateGunApplicationService(IGunRepository repository,
                                        GunService gunService,
                                        IGunCategoryRepository gunCategoryRepository,
                                        IBulletRepository bulletRepository,
                                        IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(gunService, nameof(gunService));
        ArgumentNullException.ThrowIfNull(gunCategoryRepository, nameof(gunCategoryRepository));
        ArgumentNullException.ThrowIfNull(bulletRepository, nameof(bulletRepository));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));

        _repository = repository;
        _gunService = gunService;
        _gunCategoryRepository = gunCategoryRepository;
        _bulletRepository = bulletRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// 銃更新
    /// </summary>
    /// <param name="gunIdVal"></param>
    /// <param name="fieldMasks"></param>
    /// <param name="request"></param>
    /// <returns>更新後の銃</returns>
    /// <exception cref="GunNotFoundException"></exception>
    /// <exception cref="DuplicateGunNameException"></exception>
    /// <exception cref="GunCategoryNotFoundException"></exception>
    /// <exception cref="BulletNotFoundException"></exception>
    public async Task<GunDto> ExecuteAsync(string gunIdVal, IEnumerable<string> fieldMasks, UpdateGunRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(fieldMasks, nameof(fieldMasks));
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        UpdateGunCommand updateGunCommand = new UpdateGunCommand(fieldMasks, request);

        GunId gunId = new GunId(gunIdVal);

        Gun? gun = await _repository.FetchAsync(gunId);
        if (gun == null)
        {
            throw new GunNotFoundException(gunId);
        }

        //名称
        if (updateGunCommand.Name != null)
        {
            GunName gunName = new GunName(updateGunCommand.Name);
            if (await _gunService.ExistsAsync(gunName))
            {
                throw new DuplicateGunNameException(gunName);
            }
            gun.ChangeName(gunName);
        }

        //カテゴリー
        if (updateGunCommand.Category != null)
        {
            GunCategoryId gunCategoryId = new GunCategoryId(updateGunCommand.Category);
            GunCategory? gunCategory = await _gunCategoryRepository.FetchAsync(gunCategoryId);
            if (gunCategory == null)
            {
                throw new GunCategoryNotFoundException(gunCategoryId);
            }
            gun.ChangeCategory(gunCategory);
        }

        //装弾数
        if (updateGunCommand.Capacity != null)
        {
            Capacity capacity = new Capacity((int)updateGunCommand.Capacity);
            gun.ChangeCapacity(capacity);
        }

        //弾丸
        if (updateGunCommand.Bullets != null)
        {
            List<Bullet> bullets = [];
            IEnumerable<BulletId> bulletIdList = updateGunCommand.Bullets
                                                                    .Select(x => new BulletId(x))
                                                                    .Distinct();
            foreach (BulletId bulletId in bulletIdList)
            {
                Bullet? bullet = await _bulletRepository.FetchAsync(bulletId);
                if (bullet == null)
                {
                    throw new BulletNotFoundException(bulletId);
                }
                bullets.Add(bullet);
            }

            gun.ChangeUseBullets(bullets);
        }


        using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _repository.SaveAsync(gun);
            transaction.Complete();

            return _mapper.Map<Gun, GunDto>(gun);
        }
    }
}

public class UpdateGunCommand
{
    public UpdateGunCommand(IEnumerable<string> fieldMasks, UpdateGunRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(fieldMasks, nameof(fieldMasks));
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        if (fieldMasks.Any() == false)
        {
            throw new ArgumentException("フィールドマスクが空です", nameof(fieldMasks));
        }

        if (fieldMasks.Contains("name"))
        {
            ArgumentNullException.ThrowIfNull(request.Name, nameof(request.Name));
            Name = request.Name;
        }

        if (fieldMasks.Contains("category"))
        {
            ArgumentNullException.ThrowIfNull(request.Category, nameof(request.Category));
            Category = request.Category;
        }

        if (fieldMasks.Contains("capacity"))
        {
            ArgumentNullException.ThrowIfNull(request.Capacity, nameof(request.Capacity));
            Capacity = request.Capacity;
        }

        if (fieldMasks.Contains("bullets"))
        {
            ArgumentNullException.ThrowIfNull(request.Bullets, nameof(request.Bullets));
            Bullets = request.Bullets;
        }
    }

    public string? Name { get; private set; }

    public string? Category { get; private set; }

    public int? Capacity { get; private set; }

    public IEnumerable<string>? Bullets { get; private set; }
}

public class UpdateGunRequestDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("capacity")]
    public int? Capacity { get; set; }

    [JsonPropertyName("bullets")]
    public IEnumerable<string>? Bullets { get; set; }
}
