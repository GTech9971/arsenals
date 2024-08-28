using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Domains.Guns;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Arsenals.ApplicationServices.Tests.Guns;

public class FetchAllGunApplicationServiceTest
{

    private readonly DummyGunCategoryBuilder _dummyGunCategoryBuilder;
    private readonly DummyGunBuilder _dummyGunBuilder;
    private readonly IMapper _mapper;
    private readonly ILogger<FetchAllGunApplicationService> _logger;

    public FetchAllGunApplicationServiceTest()
    {
        _dummyGunCategoryBuilder = new DummyGunCategoryBuilder();
        _dummyGunBuilder = new DummyGunBuilder(_dummyGunCategoryBuilder);

        var config = new MapperConfiguration(c =>
        {
            c.AddMaps(typeof(GunDto).Assembly);
        });
        _mapper = config.CreateMapper();
        _logger = new Mock<ILogger<FetchAllGunApplicationService>>().Object;
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
                                                                                _mapper,
                                                                                _logger);

        List<GunDto> empty = await sut.Execute(null).ToListAsync();

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
                                                                                _mapper,
                                                                                _logger);

        List<GunDto> actual = await sut.Execute(null).ToListAsync();

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
                                                                                _mapper,
                                                                                _logger);

        List<GunDto> actual = await sut.Execute(gunCategoryIdVal).ToListAsync();

        Assert.Equal(expected, actual.Count);
    }
}
