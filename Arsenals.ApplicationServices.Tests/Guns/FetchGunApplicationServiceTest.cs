using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Domains.Guns;
using Arsenals.Domains.Guns.Exceptions;
using AutoMapper;
using Moq;

namespace Arsenals.ApplicationServices.Tests.Guns;

public class FetchGunApplicationServiceTest
{
    private readonly DummyGunCategoryBuilder _dummyGunCategoryBuilder;
    private readonly DummyGunBuilder _dummyGunBuilder;
    private readonly IMapper _mapper;

    public FetchGunApplicationServiceTest()
    {
        _dummyGunCategoryBuilder = new DummyGunCategoryBuilder();
        _dummyGunBuilder = new DummyGunBuilder(_dummyGunCategoryBuilder);

        var config = new MapperConfiguration(c =>
        {
            c.AddMaps(typeof(GunDto).Assembly);
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async void not_found()
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        //gunRepositoryMock.Setup(x => x.FetchAll()).Returns(AsyncEnumerable.Empty<Gun>());

        FetchGunApplicationService sut = new FetchGunApplicationService(gunRepositoryMock.Object,
                                                                        _mapper);

        await Assert.ThrowsAsync<GunNotFoundException>(async () =>
        {
            await sut.ExecuteAsync("G-1000");
        });
    }

    [Fact]
    public async void fetch()
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock.Setup(x => x.FetchAsync(new GunId("G-1000"))).ReturnsAsync(_dummyGunBuilder.Build());

        FetchGunApplicationService sut = new FetchGunApplicationService(gunRepositoryMock.Object,
                                                                        _mapper);


        GunDto result = await sut.ExecuteAsync("G-1000");

        Assert.Equal(100, result.Id);

    }
}
