namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃
/// </summary>
public class Gun : IEquatable<Gun>
{
    private readonly GunId _id;
    private GunName _name;
    private Capacity _capacity;
    private GunCategory _category;

    //以下任意項目
    private readonly Uri? _imageUri;

    public Gun(GunId id,
                GunName name,
                GunCategory category,
                Capacity capacity)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentNullException.ThrowIfNull(category, nameof(category));
        ArgumentNullException.ThrowIfNull(capacity, nameof(capacity));

        _id = id;
        _name = name;
        _capacity = capacity;
        _category = category;
    }

    public GunId Id => _id;
    public GunName Name => _name;
    public GunCategory Category => _category;
    public Capacity Capacity => _capacity;
    public Uri? ImageUrl => _imageUri;
    //TODO 弾丸


    /// <summary>
    /// 銃の内容を更新する
    /// </summary>
    /// <param name="updateCommand"></param>
    public void Update(GunUpdateCommand updateCommand)
    {
        ArgumentNullException.ThrowIfNull(updateCommand, nameof(updateCommand));
        //TODO
    }

    public bool Equals(Gun? other)
    {
        if (other == null) { return false; }
        if (object.ReferenceEquals(this, other)) { return true; }
        return _id.Equals(other._id);
    }

    public override bool Equals(object? obj) { return Equals(obj as Gun); }
    public override int GetHashCode() { return _id.GetHashCode(); }
}
