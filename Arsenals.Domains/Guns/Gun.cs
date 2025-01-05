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
    private GunImage? _image;
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

    public Gun(GunId id,
        GunName name,
        GunCategory category,
        Capacity capacity,
        IEnumerable<Bullet> useableBullets,
        GunImage? gunImage) : this(id, name, category, capacity, useableBullets)
    {
        _image = gunImage;
    }

    public GunId Id => _id;
    public GunName Name => _name;
    public GunCategory Category => _category;
    public Capacity Capacity => _capacity;
    public GunImage? Image => _image;
    public ImmutableList<Bullet> UseableBullets => _useableBullets.ToImmutableList();


    /// <summary>
    /// 銃の名称を更新する
    /// </summary>
    /// <param name="gunName"></param>
    public void ChangeName(GunName gunName)
    {
        ArgumentNullException.ThrowIfNull(gunName, nameof(gunName));
        _name = gunName;
    }

    /// <summary>
    /// 銃のカテゴリーを変更する
    /// </summary>
    /// <param name="gunCategory"></param>
    public void ChangeCategory(GunCategory gunCategory)
    {
        ArgumentNullException.ThrowIfNull(gunCategory, nameof(gunCategory));
        _category = gunCategory;
    }

    /// <summary>
    /// 装弾数を変更する
    /// </summary>
    /// <param name="capacity"></param>
    public void ChangeCapacity(Capacity capacity)
    {
        ArgumentNullException.ThrowIfNull(capacity, nameof(capacity));
        _capacity = capacity;
    }

    /// <summary>
    /// 使用する弾丸を変更する
    /// </summary>
    /// <param name="bullets"></param>
    public void ChangeUseBullets(IEnumerable<Bullet> bullets)
    {
        ArgumentNullException.ThrowIfNull(bullets, nameof(bullets));
        _useableBullets = bullets
                            .Distinct()
                            .ToList();
    }

    /// <summary>
    /// 銃の画像を変更する
    /// </summary>
    /// <param name="gunImage"></param>
    public void ChangeGunImage(GunImage gunImage)
    {
        _image = gunImage;
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

        public Builder WithImageUrl(GunImage? image)
        {
            ArgumentNullException.ThrowIfNull(_target, nameof(_target));
            _target._image = image;
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
