using Arsenals.Domains.Bullets;

namespace Arsenals.Domains.Tests.Bullets;

public class BulletIdTest
{
    [Theory(DisplayName = "正常ケース")]
    [InlineData("B-1290")]
    [InlineData("B-0001")]
    [InlineData("B-9999")]
    [InlineData("B-1234")]
    public void valid(string value)
    {
        BulletId sut = new BulletId(value);
        Assert.Equal(value, sut.Value);
    }

    [Theory(DisplayName = "indexのテスト")]
    [InlineData("B-1290", 1290)]
    [InlineData("B-0004", 4)]
    [InlineData("B-9999", 9999)]
    [InlineData("B-0981", 981)]
    public void index(string value, int expected)
    {
        BulletId sut = new BulletId(value);
        Assert.Equal(expected, sut.Index);
    }
}
