using Arsenals.ApplicationServices.Guns;
using Arsenals.Domains.Guns;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Arsenals.ApplicationServices.Tests.Guns;

public class FetchAllGunApplicationServiceTest
{

    private readonly DummyGunCategoryBuilder _dummyGunCategoryBuilder;
    private readonly DummyGunBuilder _dummyGunBuilder;
    private readonly IMapper _mapper;

    public FetchAllGunApplicationServiceTest()
    {
        _dummyGunCategoryBuilder = new DummyGunCategoryBuilder();
        _dummyGunBuilder = new DummyGunBuilder(_dummyGunCategoryBuilder);

        var config = new MapperConfiguration(c =>
        {
            c.AddMaps(typeof(FetchAllGunResponseDto).Assembly);
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async void fetch_empty()
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock.Setup(x => x.FetchAll()).Returns(AsyncEnumerable.Empty<Gun>());

        Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x["images:root"]).Returns("dummy");

        FetchAllGunApplicationService sut = new FetchAllGunApplicationService(gunRepositoryMock.Object,
                                                                                configurationMock.Object,
                                                                                _mapper);

        List<FetchAllGunResponseDto> empty = await sut.ExecuteAsync(null);

        Assert.False(empty.Any());
    }

    [Fact]
    public async void fetch_any()
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock.Setup(x => x.FetchAll()).Returns(new List<Gun>() { _dummyGunBuilder.Build(), _dummyGunBuilder.Build() }.ToAsyncEnumerable());

        Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x["images:root"]).Returns("dummy");

        FetchAllGunApplicationService sut = new FetchAllGunApplicationService(gunRepositoryMock.Object,
                                                                                configurationMock.Object,
                                                                                _mapper);

        List<FetchAllGunResponseDto> actual = await sut.ExecuteAsync(null);

        Assert.True(actual.Any());
        Assert.Equal(2, actual.Count);
    }

    [Theory]
    [InlineData(100, 2)]
    [InlineData(200, 0)]
    public async void fetch_filter(int gunCategoryIdVal, int expected)
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock.Setup(x => x.FetchAll()).Returns(new List<Gun>() { _dummyGunBuilder.Build(), _dummyGunBuilder.Build() }.ToAsyncEnumerable());

        Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x["images:root"]).Returns("dummy");

        FetchAllGunApplicationService sut = new FetchAllGunApplicationService(gunRepositoryMock.Object,
                                                                                configurationMock.Object,
                                                                                _mapper);

        List<FetchAllGunResponseDto> actual = await sut.ExecuteAsync(gunCategoryIdVal);

        Assert.Equal(expected, actual.Count);
    }
}
