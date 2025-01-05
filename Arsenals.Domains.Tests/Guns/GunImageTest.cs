using Arsenals.Domains.Guns;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Arsenals.Domains.Tests.Guns;

/// <summary>
/// 銃画像のテスト　
/// </summary>
public class GunImageTest
{
    private readonly IConfiguration _configuration;

    public GunImageTest()
    {
        Mock<IConfiguration> mock = new Mock<IConfiguration>();
        mock
            .Setup(x => x[GunImage.DOWNLOAD_KEY])
            .Returns("https://arsenals/assets/guns/images");

        mock
            .Setup(x => x[GunImage.ROOT_KEY])
            .Returns("/usr/var/arsenals/guns/images/");

        _configuration = mock.Object;
    }

    [Theory(DisplayName = "ダウンロードURL")]
    [InlineData("G-0001", ".png", 1, "https://arsenals/assets/guns/images/G-0001/1.png")]
    [InlineData("G-1000", ".jpeg", 24, "https://arsenals/assets/guns/images/G-1000/24.jpeg")]
    public void download_url(string gunIdVal, string extension, int id, string expected)
    {
        GunId gunId = new GunId(gunIdVal);

        GunImage sut = new GunImage(id, extension, _configuration);

        string actual = sut.DownloadUrl(gunId).ToString();

        Assert.Equal(expected, actual);
    }

    [Theory(DisplayName = "ルートパス")]
    [InlineData("G-0001", ".png", 1, "/usr/var/arsenals/guns/images/G-0001/1.png")]
    [InlineData("G-1000", ".jpeg", 24, "/usr/var/arsenals/guns/images/G-1000/24.jpeg")]
    public void root_path(string gunIdVal, string extension, int id, string expected)
    {
        GunId gunId = new GunId(gunIdVal);

        GunImage sut = new GunImage(id, extension, _configuration);

        string actual = sut.Path(gunId);

        Assert.Equal(expected, actual);
    }
}
