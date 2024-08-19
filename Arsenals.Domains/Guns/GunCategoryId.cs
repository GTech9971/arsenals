namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃のカテゴリーID
/// </summary>
public class GunCategoryId : IEquatable<GunCategoryId>
{

    /// <summary>
    /// 最初のIDを生成する
    /// </summary>
    /// <returns></returns>
    public static GunCategoryId FirstId()
    {
        return new GunCategoryId(MIN);
    }

    public static readonly int MIN = 100;

    private readonly int _value;

    public GunCategoryId(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, MIN, nameof(value));
        _value = value;
    }

    /// <summary>
    /// 銃のカテゴリーID
    /// </summary>
    public int Value => _value;


    /// <summary>
    /// 次のIDを生成する
    /// </summary>
    /// <returns></returns>
    public GunCategoryId Next()
    {
        return new GunCategoryId(_value + MIN);
    }

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
