namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃のカテゴリー
/// </summary>
public class GunCategory : IEquatable<GunCategory>
{
    private readonly GunCategoryId _id;

    //TODO 名前

    public GunCategory(GunCategoryId id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        _id = id;
    }

    public GunCategoryId Id => _id;

    public bool Equals(GunCategory? other)
    {
        if (other == null) { return false; }
        if (object.ReferenceEquals(this, other)) { return true; }
        return _id.Equals(other._id);
    }

    public override bool Equals(object? obj) { return Equals(obj as GunCategory); }
    public override int GetHashCode() { return _id.GetHashCode(); }
}
