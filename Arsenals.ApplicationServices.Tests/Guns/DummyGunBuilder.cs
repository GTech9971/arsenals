using Arsenals.Domains.Guns;

namespace Arsenals.ApplicationServices.Tests.Guns;

public class DummyGunBuilder
{
    private readonly DummyGunCategoryBuilder _dummyGunCategoryBuilder;

    public DummyGunBuilder(DummyGunCategoryBuilder dummyGunCategoryBuilder)
    {
        ArgumentNullException.ThrowIfNull(dummyGunCategoryBuilder, nameof(dummyGunCategoryBuilder));
        _dummyGunCategoryBuilder = dummyGunCategoryBuilder;
    }

    public DummyGunBuilder() { _dummyGunCategoryBuilder = new DummyGunCategoryBuilder(); }

    public Gun Build()
    {
        return new Gun(new GunId("G-1000"), new GunName("Glock22"), _dummyGunCategoryBuilder.Build(), new Capacity(17));
    }

    public Gun Build(string categoryIdVal)
    {
        return new Gun(new GunId("G-1000"), new GunName("Glock22"), _dummyGunCategoryBuilder.Build(categoryIdVal), new Capacity(17));
    }
}
