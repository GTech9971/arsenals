using Arsenals.Domains.Guns;

namespace Arsenals.Domains.Tests.Guns;

public class GunIdTest
{

    [Theory(DisplayName = "正常ケース")]
    [InlineData("G-1290")]
    [InlineData("G-0001")]
    [InlineData("G-9999")]
    [InlineData("G-1234")]
    public void valid(string value)
    {
        GunId sut = new GunId(value);
        Assert.Equal(value, sut.Value);
    }

    [Theory(DisplayName = "indexのテスト")]
    [InlineData("G-1290", 1290)]
    [InlineData("G-0004", 4)]
    [InlineData("G-9999", 9999)]
    [InlineData("G-0981", 981)]
    public void index(string value, int expected)
    {
        GunId sut = new GunId(value);
        Assert.Equal(expected, sut.Index);
    }
}
