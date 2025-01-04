using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Domains.Guns;
using Arsenals.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Arsenals.ApplicationServices.Tests.Guns;

/// <summary>
/// 全銃取得アプリケーションサービスのテスト
/// </summary>
public class FetchAllGunApplicationServiceTest
{
    private readonly DummyGunCategoryBuilder _dummyGunCategoryBuilder;
    private readonly DummyGunBuilder _dummyGunBuilder;
    private readonly IMapper _mapper;

    public FetchAllGunApplicationServiceTest()
    {
        _dummyGunCategoryBuilder = new DummyGunCategoryBuilder();
        _dummyGunBuilder = new DummyGunBuilder(_dummyGunCategoryBuilder);

        var config = new MapperConfiguration(c => c.AddProfile<DtoMappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact(DisplayName = "取得結果なし")]
    public async void fetch_empty()
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock
            .Setup(x => x.FetchAllAsync())
            .Returns(AsyncEnumerable.Empty<Gun>());

        Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();
        configurationMock
            .Setup(x => x["images:root"])
            .Returns("dummy");

        FetchAllGunApplicationService sut = new FetchAllGunApplicationService(gunRepositoryMock.Object,
                                                                                configurationMock.Object,
                                                                                _mapper);

        FetchGunsResponseModel response = await sut.ExecuteAsync(null);

        Assert.Empty(response.Data);
    }

    [Fact(DisplayName = "1件以上取得")]
    public async void fetch_any()
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock
            .Setup(x => x.FetchAllAsync())
            .Returns(new List<Gun>() { _dummyGunBuilder.Build(), _dummyGunBuilder.Build() }
            .ToAsyncEnumerable());

        Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();
        configurationMock
            .Setup(x => x["images:root"])
            .Returns("dummy");

        FetchAllGunApplicationService sut = new FetchAllGunApplicationService(gunRepositoryMock.Object,
                                                                                configurationMock.Object,
                                                                                _mapper);

        FetchGunsResponseModel actual = await sut.ExecuteAsync(null);

        Assert.NotEmpty(actual.Data);
        Assert.Equal(2, actual.Data.Count);
    }

    [Theory(DisplayName = "カテゴリー検索")]
    [InlineData("C-1000", 2)]
    [InlineData("C-2000", 0)]
    public async void fetch_filter(string gunCategoryIdVal, int expected)
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock
            .Setup(x => x.FetchAllAsync())
            .Returns(new List<Gun>() { _dummyGunBuilder.Build(), _dummyGunBuilder.Build() }
            .ToAsyncEnumerable());

        Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();
        configurationMock
            .Setup(x => x["images:root"])
            .Returns("dummy");

        FetchAllGunApplicationService sut = new FetchAllGunApplicationService(gunRepositoryMock.Object,
                                                                                configurationMock.Object,
                                                                                _mapper);

        FetchGunsResponseModel actual = await sut.ExecuteAsync(gunCategoryIdVal);

        Assert.Equal(expected, actual.Data.Count);
    }
}
