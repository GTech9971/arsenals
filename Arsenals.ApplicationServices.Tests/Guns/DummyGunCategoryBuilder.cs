using Arsenals.Domains.Guns;

namespace Arsenals.ApplicationServices.Tests.Guns;

public class DummyGunCategoryBuilder
{
    public GunCategory Build()
    {
        return new GunCategory(new GunCategoryId("C-1000"), new GunCategoryName("ハンドガン"));
    }

    public GunCategory BuildWithName(string categoryName)
    {
        return new GunCategory(new GunCategoryId("C-1000"), new GunCategoryName(categoryName));
    }

    public GunCategory Build(string categoryIdVal)
    {
        return new GunCategory(new GunCategoryId(categoryIdVal), new GunCategoryName("ライフル"));
    }
}
