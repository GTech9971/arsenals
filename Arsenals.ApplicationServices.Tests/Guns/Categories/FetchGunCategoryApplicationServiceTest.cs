using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Domains.Guns;
using Arsenals.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Arsenals.ApplicationServices.Tests.Guns.Categories;

/// <summary>
/// 全銃カテゴリー取得アプリケーションサービスのテスト
/// </summary>
public class FetchGunCategoryApplicationServiceTest
{
    private readonly DummyGunCategoryBuilder _dummyGunCategoryBuilder;
    private readonly IMapper _mapper;

    public FetchGunCategoryApplicationServiceTest()
    {
        _dummyGunCategoryBuilder = new DummyGunCategoryBuilder();

        Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();

        MapperConfiguration mapperConfiguration = new MapperConfiguration(c =>
        {
            c.AddProfile(new DtoMappingProfile(configurationMock.Object));
        });
        _mapper = mapperConfiguration.CreateMapper();
    }

    [Fact(DisplayName = "取得結果0")]
    public async Task fetch_empty()
    {
        Mock<IGunCategoryRepository> repositoryMock = new Mock<IGunCategoryRepository>();
        repositoryMock
            .Setup(x => x.FetchAllAsync())
            .Returns(AsyncEnumerable.Empty<GunCategory>());

        FetchGunCategoryApplicationService sut = new FetchGunCategoryApplicationService(repositoryMock.Object,
                                                                                        _mapper);

        FetchGunCategoryResponseModel responseModel = await sut.ExecuteAsync();
        Assert.NotNull(responseModel?.Data);
        Assert.Empty(responseModel.Data);
        Assert.Null(responseModel.Error);
    }

    [Fact(DisplayName = "1件以上取得")]
    public async Task fetch_any()
    {
        Mock<IGunCategoryRepository> repositoryMock = new Mock<IGunCategoryRepository>();
        repositoryMock
            .Setup(x => x.FetchAllAsync())
            .Returns(new List<GunCategory>() { _dummyGunCategoryBuilder.Build() }.ToAsyncEnumerable());

        FetchGunCategoryApplicationService sut = new FetchGunCategoryApplicationService(repositoryMock.Object,
                                                                                        _mapper);

        FetchGunCategoryResponseModel responseModel = await sut.ExecuteAsync();
        Assert.NotNull(responseModel?.Data);
        Assert.Single(responseModel.Data);
        Assert.Null(responseModel.Error);
    }
}
