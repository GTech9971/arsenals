using Arsenals.ApplicationServices.Guns;
using Arsenals.Domains.Guns;
using Arsenals.Domains.Guns.Exceptions;
using Moq;

namespace Arsenals.ApplicationServices.Tests.Guns;

public class DeleteGunApplicationServiceTest
{

    private readonly DummyGunCategoryBuilder _dummyGunCategoryBuilder;
    private readonly DummyGunBuilder _dummyGunBuilder;

    public DeleteGunApplicationServiceTest()
    {
        _dummyGunCategoryBuilder = new DummyGunCategoryBuilder();
        _dummyGunBuilder = new DummyGunBuilder(_dummyGunCategoryBuilder);
    }

    [Fact]
    public async void delete()
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock.Setup(x => x.FetchAsync(It.IsAny<GunId>())).ReturnsAsync(_dummyGunBuilder.Build());

        DeleteGunApplicationService sut = new DeleteGunApplicationService(gunRepositoryMock.Object);

        await sut.ExecuteAsync(100);
    }

    [Fact]
    public async void not_found()
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock.Setup(x => x.FetchAsync(It.IsAny<GunId>())).ReturnsAsync(() =>
        {
            return null;
        });


        DeleteGunApplicationService sut = new DeleteGunApplicationService(gunRepositoryMock.Object);

        await Assert.ThrowsAsync<GunNotFoundException>(async () =>
        {
            await sut.ExecuteAsync(100);
        });
    }
}
