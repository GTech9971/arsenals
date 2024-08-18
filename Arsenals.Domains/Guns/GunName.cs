namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃の名称
/// </summary>
public class GunName : IEquatable<GunName>
{
    private static readonly int MIN_LEN = 1;
    private readonly string _value;

    public GunName(string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, MIN_LEN, nameof(value));

        _value = value;
    }

    /// <summary>
    /// 銃の名称
    /// </summary>
    public string Value => _value;

    public bool Equals(GunName? other)
    {
        if (other == null) { return false; }
        if (object.ReferenceEquals(this, other)) { return true; }
        return this._value == other.Value;
    }

    public override bool Equals(object? obj) { return Equals(obj as GunName); }
    public override int GetHashCode() { return _value.GetHashCode(); }
    public override string ToString() { return _value; }
}
