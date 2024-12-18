using Arsenals.ApplicationServices.Guns;
using Arsenals.Domains.Guns;
using Arsenals.Domains.Guns.Exceptions;
using Arsenals.Domains.Guns.Services;
using Arsenals.Models;
using Moq;

namespace Arsenals.ApplicationServices.Tests.Guns.Categories;

public class RegistryGunCategoryApplicationServiceTest
{

    private readonly DummyGunCategoryBuilder _dummyGunCategoryBuilder;

    public RegistryGunCategoryApplicationServiceTest()
    {
        _dummyGunCategoryBuilder = new DummyGunCategoryBuilder();
    }

    [Fact(DisplayName = "カテゴリー名が被った際に例外が発生")]
    public async void duplicate_category_name()
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        Mock<IGunCategoryRepository> gunCategoryRepositoryMock = new Mock<IGunCategoryRepository>();
        gunCategoryRepositoryMock
            .Setup(x => x.FetchAsync(new GunCategoryId("C-1000")))
            .ReturnsAsync(_dummyGunCategoryBuilder.BuildWithName("ハンドガン"));
        gunCategoryRepositoryMock
            .Setup(x => x.FetchAllAsync())
            .Returns(new List<GunCategory>() { _dummyGunCategoryBuilder.BuildWithName("ハンドガン") }.ToAsyncEnumerable());

        Mock<IGunCategoryIdFactory> gunCategoryIdFactoryMock = new Mock<IGunCategoryIdFactory>();
        GunCategoryService gunCategoryService = new GunCategoryService(gunCategoryRepositoryMock.Object, gunRepositoryMock.Object);

        RegistryGunCategoryApplicationService sut = new RegistryGunCategoryApplicationService(gunCategoryRepositoryMock.Object,
                                                                                                gunCategoryIdFactoryMock.Object,
                                                                                                gunCategoryService);

        RegistryGunCategoryRequestModel request = new RegistryGunCategoryRequestModel()
        {
            Name = "ハンドガン"
        };

        await Assert.ThrowsAsync<DuplicateGunCategoryNameException>(() => sut.ExecuteAsync(request));
    }
}
