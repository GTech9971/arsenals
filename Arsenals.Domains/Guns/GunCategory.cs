namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃のカテゴリー
/// </summary>
public class GunCategory : IEquatable<GunCategory>
{
    private readonly GunCategoryId _id;
    private GunCategoryName _name;

    public GunCategory(GunCategoryId id,
                        GunCategoryName name)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        _id = id;
        _name = name;
    }

    public GunCategoryId Id => _id;
    public GunCategoryName Name => _name;

    /// <summary>
    /// カテゴリー名を変更する
    /// </summary>
    /// <param name="name"></param>
    public void ChangeName(GunCategoryName name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        _name = name;
    }

    public bool Equals(GunCategory? other)
    {
        if (other == null) { return false; }
        if (object.ReferenceEquals(this, other)) { return true; }
        return _id.Equals(other._id);
    }

    public override bool Equals(object? obj) { return Equals(obj as GunCategory); }
    public override int GetHashCode() { return _id.GetHashCode(); }
}
