using System;
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

    public Gun Build()
    {
        return new Gun(new GunId(100), new GunName("Glock22"), _dummyGunCategoryBuilder.Build(), new Capacity(17));
    }
}
