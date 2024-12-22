using Arsenals.ApplicationServices.Bullets;
using Arsenals.Domains.Bullets;
using Arsenals.Domains.Bullets.Exceptions;
using Arsenals.Domains.Bullets.Services;
using Arsenals.Domains.Guns;
using Arsenals.Models;
using Moq;

namespace Arsenals.ApplicationServices.Tests.Bullets;

public class RegistryBulletApplicationServiceTest
{

    private readonly DummyBulletBuilder _dummyBulletBuilder;

    public RegistryBulletApplicationServiceTest()
    {
        _dummyBulletBuilder = new DummyBulletBuilder();
    }

    [Fact(DisplayName = "名前が被っている")]
    public async void duplicate_name()
    {
        Mock<IBulletRepository> bulletRepositoryMock = new Mock<IBulletRepository>();
        IEnumerable<Bullet> samples = [_dummyBulletBuilder.BuildWithName(name: "9mm")];
        bulletRepositoryMock
            .Setup(x => x.FetchAllAsync())
            .Returns(samples.ToAsyncEnumerable());

        Mock<IBulletIdFactory> bulletIdFactoryMock = new Mock<IBulletIdFactory>();
        bulletIdFactoryMock
            .Setup(x => x.BuildAsync())
            .ReturnsAsync(new BulletId("B-1001"));

        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();

        BulletService bulletService = new BulletService(bulletRepositoryMock.Object, gunRepositoryMock.Object);
        RegistryBulletApplicationService sut = new RegistryBulletApplicationService(bulletRepositoryMock.Object,
                                                                                        bulletService,
                                                                                        bulletIdFactoryMock.Object);

        RegistryBulletRequestModel request = new RegistryBulletRequestModel()
        {
            Name = "9mm",
            Damage = 3
        };

        await Assert.ThrowsAsync<DuplicateBulletNameException>(() => sut.ExecuteAsync(request));

        bulletRepositoryMock.Verify(x => x.FetchAllAsync(), Times.Once());
        bulletIdFactoryMock.Verify(x => x.BuildAsync(), Times.Never());
    }


    [Fact(DisplayName = "弾丸登録")]
    public async void success()
    {
        Mock<IBulletRepository> bulletRepositoryMock = new Mock<IBulletRepository>();
        IEnumerable<Bullet> samples = [_dummyBulletBuilder.BuildWithName(name: "9mm")];
        bulletRepositoryMock
            .Setup(x => x.FetchAllAsync())
            .Returns(samples.ToAsyncEnumerable());

        Mock<IBulletIdFactory> bulletIdFactoryMock = new Mock<IBulletIdFactory>();
        bulletIdFactoryMock
            .Setup(x => x.BuildAsync())
            .ReturnsAsync(new BulletId("B-1001"));

        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();

        BulletService bulletService = new BulletService(bulletRepositoryMock.Object, gunRepositoryMock.Object);
        RegistryBulletApplicationService sut = new RegistryBulletApplicationService(bulletRepositoryMock.Object,
                                                                                        bulletService,
                                                                                        bulletIdFactoryMock.Object);

        RegistryBulletRequestModel request = new RegistryBulletRequestModel()
        {
            Name = "45ACP",
            Damage = 3
        };

        var response = await sut.ExecuteAsync(request);
        Assert.NotNull(response.Data);

        Assert.Equal("B-1001", response.Data.Id);

        bulletRepositoryMock.Verify(x => x.FetchAllAsync(), Times.Once());
        bulletIdFactoryMock.Verify(x => x.BuildAsync(), Times.Once());
    }
}
