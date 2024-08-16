namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃のカテゴリー名
/// </summary>
public class GunCategoryName : IEquatable<GunCategoryName>
{
    private static readonly int MAX = 50;

    private readonly string _value;

    public GunCategoryName(string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(MAX, value.Length, nameof(value));

        _value = value;
    }

    /// <summary>
    /// 銃のカテゴリー名
    /// </summary>
    public string Value => _value;

    public bool Equals(GunCategoryName? other)
    {
        if (other == null) { return false; }
        if (object.ReferenceEquals(this, other)) { return true; }
        return _value == other._value;
    }

    public override bool Equals(object? obj) { return Equals(obj as GunCategoryName); }
    public override int GetHashCode() { return _value.GetHashCode(); }
    public override string ToString() { return _value; }
}
