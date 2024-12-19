using Arsenals.ApplicationServices.Guns;
using Arsenals.Domains.Guns;
using Arsenals.Domains.Guns.Exceptions;
using Arsenals.Domains.Guns.Services;
using Moq;

namespace Arsenals.ApplicationServices.Tests.Guns.Categories;

/// <summary>
/// 銃のカテゴリー削除アプリケーションサービス
/// </summary>
public class DeleteGunCategoryApplicationServiceTest
{

    private readonly DummyGunBuilder _dummyGunBuilder;
    private readonly DummyGunCategoryBuilder _dummyGunCategoryBuilder;

    public DeleteGunCategoryApplicationServiceTest()
    {
        _dummyGunCategoryBuilder = new DummyGunCategoryBuilder();
        _dummyGunBuilder = new DummyGunBuilder(_dummyGunCategoryBuilder);
    }

    [Fact(DisplayName = "使用済みのため削除不可")]
    public async void invalid_delete()
    {
        Mock<IGunCategoryRepository> categoryMock = new Mock<IGunCategoryRepository>();
        categoryMock
            .Setup(x => x.FetchAsync(new GunCategoryId("C-1000")))
            .ReturnsAsync(_dummyGunCategoryBuilder.Build("C-1000"));

        Mock<IGunRepository> gunMock = new Mock<IGunRepository>();
        gunMock
            .Setup(x => x.FetchAllAsync())
            .Returns(new Gun[] { _dummyGunBuilder.Build(categoryIdVal: "C-1000") }.ToAsyncEnumerable());

        GunCategoryService gunCategoryService = new GunCategoryService(categoryMock.Object, gunMock.Object);

        DeleteGunCategoryApplicationService sut = new DeleteGunCategoryApplicationService(categoryMock.Object, gunCategoryService);

        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.ExecuteAsync("C-1000"));
    }

    [Fact(DisplayName = "カテゴリーが存在しない")]
    public async void not_found()
    {
        Mock<IGunCategoryRepository> categoryMock = new Mock<IGunCategoryRepository>();
        Mock<IGunRepository> gunMock = new Mock<IGunRepository>();

        GunCategoryService gunCategoryService = new GunCategoryService(categoryMock.Object, gunMock.Object);

        DeleteGunCategoryApplicationService sut = new DeleteGunCategoryApplicationService(categoryMock.Object, gunCategoryService);

        await Assert.ThrowsAsync<GunCategoryNotFoundException>(() => sut.ExecuteAsync("C-1000"));
    }

    [Fact(DisplayName = "削除")]
    public async void delete()
    {
        Mock<IGunCategoryRepository> categoryMock = new Mock<IGunCategoryRepository>();
        categoryMock
            .Setup(x => x.FetchAsync(new GunCategoryId("C-1000")))
            .ReturnsAsync(_dummyGunCategoryBuilder.Build("C-1000"));

        Mock<IGunRepository> gunMock = new Mock<IGunRepository>();
        gunMock
            .Setup(x => x.FetchAllAsync())
            .Returns(AsyncEnumerable.Empty<Gun>());

        GunCategoryService gunCategoryService = new GunCategoryService(categoryMock.Object, gunMock.Object);

        DeleteGunCategoryApplicationService sut = new DeleteGunCategoryApplicationService(categoryMock.Object, gunCategoryService);

        await sut.ExecuteAsync("C-1000");

        categoryMock.Verify(x => x.DeleteAsync(new GunCategoryId("C-1000")), Times.Once());
    }
}
