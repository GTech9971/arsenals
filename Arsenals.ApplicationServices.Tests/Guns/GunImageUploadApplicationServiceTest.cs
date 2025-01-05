using Arsenals.ApplicationServices.Guns;
using Arsenals.Domains;
using Arsenals.Domains.Guns;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Arsenals.ApplicationServices.Tests.Guns;


/// <summary>
/// 銃画像登録アプリケーションサービスのテスト
/// </summary>
public class GunImageUploadApplicationServiceTest
{

    private readonly DummyGunCategoryBuilder _dummyGunCategoryBuilder;
    private readonly DummyGunBuilder _dummyGunBuilder;


    public GunImageUploadApplicationServiceTest()
    {
        _dummyGunCategoryBuilder = new DummyGunCategoryBuilder();
        _dummyGunBuilder = new DummyGunBuilder(_dummyGunCategoryBuilder);
    }

    [Fact(DisplayName = "保存成功")]
    public async Task save()
    {
        GunId gunId = new GunId("G-1000");
        string extension = ".jpg";

        Mock<IGunRepository> gunRepositoryMock = new Mock<IGunRepository>();
        gunRepositoryMock
            .Setup(x => x.FetchAsync(It.IsAny<GunId>()))
            .ReturnsAsync(_dummyGunBuilder.Build());

        Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();
        configurationMock
            .Setup(x => x[GunImage.DOWNLOAD_KEY])
            .Returns("https://arsenals/assets/guns/images");

        configurationMock
            .Setup(x => x[GunImage.ROOT_KEY])
            .Returns("/usr/var/arsenals/assets/guns/images");

        Mock<IFileManager> fileManagerMock = new Mock<IFileManager>();

        Mock<IGunImageRepository> gunImageRepositoryMock = new Mock<IGunImageRepository>();
        gunImageRepositoryMock
            .Setup(x => x.SaveAsync(gunId, extension, It.IsAny<MemoryStream>()))
            .ReturnsAsync(new GunImage(1, extension));

        GunImageUploadApplicationService sut = new GunImageUploadApplicationService(gunImageRepositoryMock.Object,
                                                                                    gunRepositoryMock.Object,
                                                                                    configurationMock.Object,
                                                                                    fileManagerMock.Object);

        using (MemoryStream stream = new MemoryStream())
        {
            byte data = 0x41;
            stream.WriteByte(data);

            string imageDownloadUrl = await sut.ExecuteAsync(gunId.Value, ".jpg", stream);
            Assert.Equal("https://arsenals/assets/guns/images/G-1000/1.jpg", imageDownloadUrl);
        }
    }
}
