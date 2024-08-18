using System.Collections.Immutable;
using Arsenals.Domains.Bullets;

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
    private Uri? _imageUri;
    private IEnumerable<Bullet> _useableBullets;

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
        _useableBullets = Enumerable.Empty<Bullet>();
    }

    public Gun(GunId id,
            GunName name,
            GunCategory category,
            Capacity capacity,
            IEnumerable<Bullet> useableBullets) : this(id, name, category, capacity)
    {
        ArgumentNullException.ThrowIfNull(useableBullets, nameof(useableBullets));
        _useableBullets = useableBullets.Distinct();
    }

    public GunId Id => _id;
    public GunName Name => _name;
    public GunCategory Category => _category;
    public Capacity Capacity => _capacity;
    public Uri? ImageUrl => _imageUri;
    public ImmutableArray<Bullet> UseableBullets => _useableBullets.ToImmutableArray();


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

    /// <summary>
    /// 銃ビルダー
    /// </summary>
    public class Builder
    {
        private Gun? _target;

        public Builder(GunId id,
                GunName name,
                GunCategory category,
                Capacity capacity)
        {
            _target = new Gun(id, name, category, capacity);
        }

        public Builder WithImageUrl(Uri? imageUrl)
        {
            ArgumentNullException.ThrowIfNull(_target, nameof(_target));
            _target._imageUri = imageUrl;
            return this;
        }

        public Builder WithBullets(IEnumerable<Bullet>? bullets)
        {
            ArgumentNullException.ThrowIfNull(_target, nameof(_target));

            _target._useableBullets = bullets == null
                                        ? Enumerable.Empty<Bullet>()
                                        : bullets.Distinct();
            return this;
        }

        public Gun Build()
        {
            ArgumentNullException.ThrowIfNull(_target, nameof(_target));

            Gun result = _target;
            _target = null;
            return result;
        }
    }
}
