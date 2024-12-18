using Arsenals.ApplicationServices.Guns;
using Arsenals.Domains.Bullets;
using Arsenals.Domains.Bullets.Exceptions;
using Arsenals.Domains.Guns;
using Arsenals.Domains.Guns.Exceptions;
using Arsenals.Domains.Guns.Services;
using Arsenals.Models;
using Moq;

namespace Arsenals.ApplicationServices.Tests.Guns;

public class RegistryGunApplicationServiceTest
{
    private readonly DummyGunCategoryBuilder _dummyGunCategoryBuilder;
    private readonly DummyGunBuilder _dummyGunBuilder;

    public RegistryGunApplicationServiceTest()
    {
        _dummyGunCategoryBuilder = new DummyGunCategoryBuilder();
        _dummyGunBuilder = new DummyGunBuilder(_dummyGunCategoryBuilder);
    }

    [Fact]
    public async void registry()
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock
            .Setup(x => x.FetchAllAsync())
            .Returns(AsyncEnumerable.Empty<Domains.Guns.Gun>());

        GunService gunService = new GunService(gunRepositoryMock.Object);

        Mock<IGunIdFactory> gunIdFactoryMock = new Mock<IGunIdFactory>();
        gunIdFactoryMock
            .Setup(x => x.BuildAsync())
            .ReturnsAsync(new GunId("G-2000"));

        Mock<IGunCategoryRepository> gunCategoryRepositoryMock = new Mock<IGunCategoryRepository>();
        gunCategoryRepositoryMock
            .Setup(x => x.FetchAsync(It.IsAny<GunCategoryId>()))
            .ReturnsAsync(_dummyGunCategoryBuilder.Build());

        Mock<IBulletRepository> bulletRepositoryMock = new Mock<IBulletRepository>();

        RegistryGunApplicationService sut = new RegistryGunApplicationService(gunRepositoryMock.Object,
                                                                                gunService,
                                                                                gunIdFactoryMock.Object,
                                                                                gunCategoryRepositoryMock.Object,
                                                                                bulletRepositoryMock.Object);

        RegistryGunRequestModel request = new RegistryGunRequestModel()
        {
            Name = "M567",
            CategoryId = "C-1000",
            Capacity = 6
        };

        RegistryGunResponseModel newId = await sut.ExecuteAsync(request);
        Assert.NotNull(newId.Data);
        Assert.Equal("G-2000", newId.Data.Id);
    }

    [Fact]
    public async void exists_gun_name()
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock
            .Setup(x => x.FetchAllAsync())
            .Returns(new List<Domains.Guns.Gun>() { _dummyGunBuilder.Build() }.ToAsyncEnumerable());

        GunService gunService = new GunService(gunRepositoryMock.Object);

        Mock<IGunIdFactory> gunIdFactoryMock = new Mock<IGunIdFactory>();
        gunIdFactoryMock
            .Setup(x => x.BuildAsync())
            .ReturnsAsync(new GunId("G-2000"));

        Mock<IGunCategoryRepository> gunCategoryRepositoryMock = new Mock<IGunCategoryRepository>();
        gunCategoryRepositoryMock
            .Setup(x => x.FetchAsync(It.IsAny<GunCategoryId>()))
            .ReturnsAsync(_dummyGunCategoryBuilder.Build());

        Mock<IBulletRepository> bulletRepositoryMock = new Mock<IBulletRepository>();

        RegistryGunApplicationService sut = new RegistryGunApplicationService(gunRepositoryMock.Object,
                                                                                gunService,
                                                                                gunIdFactoryMock.Object,
                                                                                gunCategoryRepositoryMock.Object,
                                                                                bulletRepositoryMock.Object);

        RegistryGunRequestModel request = new RegistryGunRequestModel()
        {
            Name = "Glock22",
            CategoryId = "C-1000",
            Capacity = 6
        };

        await Assert.ThrowsAsync<DuplicateGunNameException>(async () =>
        {
            await sut.ExecuteAsync(request);
        });
    }

    [Fact]
    public async void not_found_gun_category()
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock
            .Setup(x => x.FetchAllAsync())
            .Returns(new List<Domains.Guns.Gun>() { _dummyGunBuilder.Build() }.ToAsyncEnumerable());

        GunService gunService = new GunService(gunRepositoryMock.Object);

        Mock<IGunIdFactory> gunIdFactoryMock = new Mock<IGunIdFactory>();
        gunIdFactoryMock
            .Setup(x => x.BuildAsync())
            .ReturnsAsync(new GunId("G-2000"));

        Mock<IGunCategoryRepository> gunCategoryRepositoryMock = new Mock<IGunCategoryRepository>();
        gunCategoryRepositoryMock
            .Setup(x => x.FetchAsync(new GunCategoryId("C-1000")))
            .ReturnsAsync(_dummyGunCategoryBuilder.Build());

        Mock<IBulletRepository> bulletRepositoryMock = new Mock<IBulletRepository>();

        RegistryGunApplicationService sut = new RegistryGunApplicationService(gunRepositoryMock.Object,
                                                                                gunService,
                                                                                gunIdFactoryMock.Object,
                                                                                gunCategoryRepositoryMock.Object,
                                                                                bulletRepositoryMock.Object);

        RegistryGunRequestModel request = new RegistryGunRequestModel()
        {
            Name = "M586",
            CategoryId = "C-9999",
            Capacity = 6
        };

        await Assert.ThrowsAsync<GunCategoryNotFoundException>(async () =>
        {
            await sut.ExecuteAsync(request);
        });
    }

    [Fact]
    public async void not_found_bullet()
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock
            .Setup(x => x.FetchAllAsync())
            .Returns(new List<Domains.Guns.Gun>() { _dummyGunBuilder.Build() }.ToAsyncEnumerable());

        GunService gunService = new GunService(gunRepositoryMock.Object);

        Mock<IGunIdFactory> gunIdFactoryMock = new Mock<IGunIdFactory>();
        gunIdFactoryMock
            .Setup(x => x.BuildAsync())
            .ReturnsAsync(new GunId("G-2000"));

        Mock<IGunCategoryRepository> gunCategoryRepositoryMock = new Mock<IGunCategoryRepository>();
        gunCategoryRepositoryMock
            .Setup(x => x.FetchAsync(It.IsAny<GunCategoryId>()))
            .ReturnsAsync(_dummyGunCategoryBuilder.Build());

        Mock<IBulletRepository> bulletRepositoryMock = new Mock<IBulletRepository>();
        bulletRepositoryMock
            .Setup(x => x.FetchAsync(It.IsAny<BulletId>()))
            .ReturnsAsync(() =>
        {
            return null;
        });

        RegistryGunApplicationService sut = new RegistryGunApplicationService(gunRepositoryMock.Object,
                                                                                gunService,
                                                                                gunIdFactoryMock.Object,
                                                                                gunCategoryRepositoryMock.Object,
                                                                                bulletRepositoryMock.Object);

        RegistryGunRequestModel request = new RegistryGunRequestModel()
        {
            Name = "M586",
            CategoryId = "C-1000",
            Capacity = 6,
            UseBullets = ["B-1000"]
        };

        await Assert.ThrowsAsync<BulletNotFoundException>(async () =>
        {
            await sut.ExecuteAsync(request);
        });
    }

}
