namespace Arsenals.Domains.Bullets;

/// <summary>
/// 弾丸名称
/// </summary>
public class BulletName : IEquatable<BulletName>
{
    private static readonly int MIN = 1;
    private static readonly int MAX = 50;

    private readonly string _value;

    public BulletName(string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, MIN, nameof(value));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, MAX, nameof(value));
        _value = value;
    }

    /// <summary>
    /// 弾丸名称
    /// </summary>
    public string Value => _value;


    public bool Equals(BulletName? other)
    {
        if (other == null) { return false; }
        if (object.ReferenceEquals(this, other)) { return true; }
        return _value == other._value;
    }

    public override bool Equals(object? obj) { return Equals(obj as BulletName); }
    public override int GetHashCode() { return _value.GetHashCode(); }
    public override string ToString() { return _value; }
}
