using Arsenals.Domains.Guns;

namespace Arsenals.ApplicationServices.Tests.Guns;

public class DummyGunCategoryBuilder
{
    public GunCategory Build()
    {
        return new GunCategory(new GunCategoryId(100), new GunCategoryName("ハンドガン"));
    }

    public GunCategory Build(int categoryIdVal)
    {
        return new GunCategory(new GunCategoryId(categoryIdVal), new GunCategoryName("ライフル"));
    }
}
