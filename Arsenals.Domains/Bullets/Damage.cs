namespace Arsenals.Domains.Bullets;

/// <summary>
/// 弾丸のダメージ
/// </summary>
public class Damage : IEquatable<Damage>
{
    private static readonly int MIN = 1;
    private static readonly int MAX = 50;

    private readonly int _value;

    public Damage(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(MIN, value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(MAX, value, nameof(value));
        _value = value;
    }


    /// <summary>
    /// 弾丸のダメージ
    /// </summary>
    public int Value => _value;

    public bool Equals(Damage? other)
    {
        if (other == null) { return false; }
        if (object.ReferenceEquals(this, other)) { return true; }
        return _value == other._value;
    }

    public override bool Equals(object? obj) { return Equals(obj as Damage); }
    public override int GetHashCode() { return _value.GetHashCode(); }
    public override string ToString() { return _value.ToString(); }
}
