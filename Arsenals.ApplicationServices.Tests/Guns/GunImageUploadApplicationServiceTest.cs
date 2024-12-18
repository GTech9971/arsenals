using Arsenals.ApplicationServices.Guns;
using Arsenals.Domains.Guns;
using Arsenals.Infrastructure.FileStorage;
using Arsenals.Infrastructure.FileStorage.Guns;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Arsenals.ApplicationServices.Tests.Guns;

public class GunImageUploadApplicationServiceTest
{

    private readonly DummyGunCategoryBuilder _dummyGunCategoryBuilder;
    private readonly DummyGunBuilder _dummyGunBuilder;


    public GunImageUploadApplicationServiceTest()
    {
        _dummyGunCategoryBuilder = new DummyGunCategoryBuilder();
        _dummyGunBuilder = new DummyGunBuilder(_dummyGunCategoryBuilder);
    }

    [Fact]
    public async void save()
    {
        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock.Setup(x => x.FetchAsync(It.IsAny<GunId>())).ReturnsAsync(_dummyGunBuilder.Build());

        Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x["images:root"]).Returns("/Users/george/Downloads");

        IGunImageRepository gunImageRepository = new FSGunImageRepository(configurationMock.Object);

        GunImageUploadApplicationService sut = new GunImageUploadApplicationService(gunImageRepository,
                                                                                    gunRepositoryMock.Object,
                                                                                     configurationMock.Object,
                                                                                     new FSFileManager());

        using (FileStream stream = new FileStream("/Users/george/Documents/GitHub/arsenals/Arsenals.ApplicationServices.Tests/Assets/glock22.jpg", FileMode.Open))
        {
            byte[] binary = new byte[stream.Length];
            await stream.ReadAsync(binary, 0, binary.Length);

            using (MemoryStream data = new MemoryStream(binary))
            {
                GunId gunId = new GunId("G-1000");
                Uri image = await sut.ExecuteAsync(gunId.Value, data);
                Assert.Equal("/Users/george/Downloads/100", image.AbsolutePath);
            }
        }
    }
}
