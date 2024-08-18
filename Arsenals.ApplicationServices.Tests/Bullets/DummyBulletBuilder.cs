using Arsenals.Domains.Bullets;

namespace Arsenals.ApplicationServices.Tests.Bullets;

public class DummyBulletBuilder
{
    public Bullet Build()
    {
        return new Bullet(new BulletId(100), new BulletName("9mm"), new Damage(3));
    }
    public Bullet Build(int bulletIdVal)
    {
        return new Bullet(new BulletId(bulletIdVal), new BulletName("45ACP"), new Damage(4));
    }
}
