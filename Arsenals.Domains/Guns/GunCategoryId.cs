namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃のカテゴリーID
/// </summary>
public class GunCategoryId : IEquatable<GunCategoryId>
{

    public static readonly int MIN = 100;

    private readonly int _value;

    public GunCategoryId(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(MIN, value, nameof(value));
        _value = value;
    }

    /// <summary>
    /// 銃のカテゴリーID
    /// </summary>
    public int Value => _value;


    public bool Equals(GunCategoryId? other)
    {
        if (other == null) { return false; }
        if (object.ReferenceEquals(this, other)) { return true; }
        return _value == other.Value;
    }

    public override bool Equals(object? obj) { return Equals(obj as GunCategoryId); }
    public override int GetHashCode() { return _value.GetHashCode(); }
    public override string ToString() { return _value.ToString(); }
}
