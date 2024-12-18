using System;
using Arsenals.Domains.Guns;

namespace Arsenals.Domains.Tests.Guns;

public class GunCategoryIdTest
{
    [Theory(DisplayName = "正常ケース")]
    [InlineData("C-1290")]
    [InlineData("C-4567")]
    [InlineData("C-0000")]
    [InlineData("C-0001")]
    public void valid(string value)
    {
        GunCategoryId sut = new GunCategoryId(value);
        Assert.Equal(value, sut.Value);
    }

    [Theory(DisplayName = "indexのテスト")]
    [InlineData("C-1234", 1234)]
    [InlineData("C-0000", 0)]
    [InlineData("C-9999", 9999)]
    [InlineData("C-0001", 1)]
    public void index(string value, int expected)
    {
        GunCategoryId sut = new GunCategoryId(value);
        Assert.Equal(expected, sut.Index);
    }
}
