namespace Arsenals.Domains.Bullets;

/// <summary>
/// 弾丸ID
/// </summary>
public class BulletId : IEquatable<BulletId>
{
    private static readonly int MIN = 100;

    private readonly int _value;

    public BulletId(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, MIN, nameof(value));
        _value = value;
    }
    /// <summary>
    /// 弾丸ID
    /// </summary>
    public int Value => _value;


    public bool Equals(BulletId? other)
    {
        if (other == null) { return false; }
        if (object.ReferenceEquals(this, other)) { return true; }
        return _value == other._value;
    }

    public override bool Equals(object? obj) { return Equals(obj as BulletId); }
    public override int GetHashCode() { return _value.GetHashCode(); }
    public override string ToString() { return _value.ToString(); }
}
