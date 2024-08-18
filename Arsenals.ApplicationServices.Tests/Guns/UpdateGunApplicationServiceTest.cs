using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.ApplicationServices.Tests.Bullets;
using Arsenals.Domains.Bullets;
using Arsenals.Domains.Guns;
using Arsenals.Domains.Guns.Services;
using AutoMapper;
using Moq;

namespace Arsenals.ApplicationServices.Tests.Guns;

public class UpdateGunApplicationServiceTest
{
    private readonly DummyGunCategoryBuilder _dummyGunCategoryBuilder;
    private readonly DummyGunBuilder _dummyGunBuilder;
    private readonly DummyBulletBuilder _dummyBulletBuilder;
    private readonly IMapper _mapper;

    public UpdateGunApplicationServiceTest()
    {
        _dummyGunCategoryBuilder = new DummyGunCategoryBuilder();
        _dummyGunBuilder = new DummyGunBuilder(_dummyGunCategoryBuilder);
        _dummyBulletBuilder = new DummyBulletBuilder();

        var config = new MapperConfiguration(c =>
        {
            c.AddMaps(typeof(GunDto).Assembly);
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async void update_name()
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock.Setup(x => x.FetchAsync(It.IsAny<GunId>())).ReturnsAsync(_dummyGunBuilder.Build());
        gunRepositoryMock.Setup(x => x.FetchAll()).Returns(new List<Gun>() { _dummyGunBuilder.Build() }.ToAsyncEnumerable());

        GunService gunService = new GunService(gunRepositoryMock.Object);

        Mock<IGunCategoryRepository> gunCategoryRepositoryMock = new Mock<IGunCategoryRepository>();

        Mock<IBulletRepository> bulletRepositoryMock = new Mock<IBulletRepository>();


        UpdateGunApplicationService sut = new UpdateGunApplicationService(gunRepositoryMock.Object,
                                                                            gunService,
                                                                            gunCategoryRepositoryMock.Object,
                                                                            bulletRepositoryMock.Object,
                                                                            _mapper);

        IEnumerable<string> filedMasks = new List<string>() { "name" };
        UpdateGunRequestDto request = new UpdateGunRequestDto() { Name = "Glock30" };

        GunDto gunDto = await sut.ExecuteAsync(100, filedMasks, request);

        Assert.Equal("Glock30", gunDto.Name);
    }

    [Fact]
    public async void update_category()
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock.Setup(x => x.FetchAsync(It.IsAny<GunId>())).ReturnsAsync(_dummyGunBuilder.Build());
        gunRepositoryMock.Setup(x => x.FetchAll()).Returns(new List<Gun>() { _dummyGunBuilder.Build() }.ToAsyncEnumerable());

        GunService gunService = new GunService(gunRepositoryMock.Object);

        Mock<IGunCategoryRepository> gunCategoryRepositoryMock = new Mock<IGunCategoryRepository>();
        gunCategoryRepositoryMock.Setup(x => x.FetchAsync(new GunCategoryId(200))).ReturnsAsync(_dummyGunCategoryBuilder.Build(200));
        gunCategoryRepositoryMock.Setup(x => x.FetchAll()).Returns(() =>
        {
            return new List<GunCategory>() { _dummyGunCategoryBuilder.Build(), _dummyGunCategoryBuilder.Build(200) }.ToAsyncEnumerable();
        });

        Mock<IBulletRepository> bulletRepositoryMock = new Mock<IBulletRepository>();


        UpdateGunApplicationService sut = new UpdateGunApplicationService(gunRepositoryMock.Object,
                                                                            gunService,
                                                                            gunCategoryRepositoryMock.Object,
                                                                            bulletRepositoryMock.Object,
                                                                            _mapper);

        IEnumerable<string> filedMasks = new List<string>() { "category" };
        UpdateGunRequestDto request = new UpdateGunRequestDto() { Category = 200 };

        GunDto gunDto = await sut.ExecuteAsync(100, filedMasks, request);

        Assert.Equal(200, gunDto.Category.Id);
    }

    [Fact]
    public async void update_capacity()
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock.Setup(x => x.FetchAsync(It.IsAny<GunId>())).ReturnsAsync(_dummyGunBuilder.Build());
        gunRepositoryMock.Setup(x => x.FetchAll()).Returns(new List<Gun>() { _dummyGunBuilder.Build() }.ToAsyncEnumerable());

        GunService gunService = new GunService(gunRepositoryMock.Object);

        Mock<IGunCategoryRepository> gunCategoryRepositoryMock = new Mock<IGunCategoryRepository>();

        Mock<IBulletRepository> bulletRepositoryMock = new Mock<IBulletRepository>();


        UpdateGunApplicationService sut = new UpdateGunApplicationService(gunRepositoryMock.Object,
                                                                            gunService,
                                                                            gunCategoryRepositoryMock.Object,
                                                                            bulletRepositoryMock.Object,
                                                                            _mapper);

        IEnumerable<string> filedMasks = new List<string>() { "capacity" };
        UpdateGunRequestDto request = new UpdateGunRequestDto() { Capacity = 20 };

        GunDto gunDto = await sut.ExecuteAsync(100, filedMasks, request);

        Assert.Equal(20, gunDto.Capacity);
    }

    [Fact]
    public async void update_bullets()
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock.Setup(x => x.FetchAsync(It.IsAny<GunId>())).ReturnsAsync(_dummyGunBuilder.Build());
        gunRepositoryMock.Setup(x => x.FetchAll()).Returns(new List<Gun>() { _dummyGunBuilder.Build() }.ToAsyncEnumerable());

        GunService gunService = new GunService(gunRepositoryMock.Object);

        Mock<IGunCategoryRepository> gunCategoryRepositoryMock = new Mock<IGunCategoryRepository>();

        Mock<IBulletRepository> bulletRepositoryMock = new Mock<IBulletRepository>();
        bulletRepositoryMock.Setup(x => x.FetchAll()).Returns(() =>
        {
            return new List<Bullet>() { _dummyBulletBuilder.Build(), _dummyBulletBuilder.Build(200) }.ToAsyncEnumerable();
        });
        bulletRepositoryMock.Setup(x => x.FetchAsync(new BulletId(200))).ReturnsAsync(_dummyBulletBuilder.Build(200));


        UpdateGunApplicationService sut = new UpdateGunApplicationService(gunRepositoryMock.Object,
                                                                            gunService,
                                                                            gunCategoryRepositoryMock.Object,
                                                                            bulletRepositoryMock.Object,
                                                                            _mapper);

        IEnumerable<string> filedMasks = new List<string>() { "bullets" };
        UpdateGunRequestDto request = new UpdateGunRequestDto() { Bullets = [200] };

        GunDto gunDto = await sut.ExecuteAsync(100, filedMasks, request);

        Assert.Equal(200, gunDto.Bullets.First().Id);
    }
}
