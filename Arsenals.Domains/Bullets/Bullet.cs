namespace Arsenals.Domains.Bullets;

/// <summary>
/// 弾丸
/// </summary>
public class Bullet : IEquatable<Bullet>
{
    private readonly BulletId _id;
    private readonly BulletName _name;
    private readonly Damage _damage;

    public Bullet(BulletId id,
                    BulletName name,
                    Damage damage)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentNullException.ThrowIfNull(damage, nameof(damage));

        _id = id;
        _name = name;
        _damage = damage;
    }

    public BulletId Id => _id;
    public BulletName Name => _name;
    public Damage Damage => _damage;

    public bool Equals(Bullet? other)
    {
        if (other == null) { return false; }
        if (object.ReferenceEquals(this, other)) { return true; }
        return _id.Equals(other._id);
    }

    public override bool Equals(object? obj) { return Equals(obj as Bullet); }
    public override int GetHashCode() { return _id.GetHashCode(); }
}
