using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Domains.Bullets;
using Arsenals.Domains.Guns;
using AutoMapper;

namespace Arsenals.ApplicationServices.Tests;

public class AutoMapperTest
{

    private readonly IMapper _mapper;

    public AutoMapperTest()
    {
        var config = new MapperConfiguration(c =>
        {
            c.AddMaps(typeof(GunDto).Assembly);
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void category_dto()
    {
        GunCategory category = new GunCategory(new GunCategoryId(100), new GunCategoryName("ハンドガン"));

        FetchGunCategoryResponseDto dto = _mapper.Map<FetchGunCategoryResponseDto>(category);

        Assert.Equal(100, dto.Id);
        Assert.Equal("ハンドガン", dto.Name);
    }

    [Fact]
    public void bullet_dto()
    {
        Bullet bullet = new Bullet(new BulletId(100), new BulletName("9mm"), new Damage(3));

        BulletDto dto = _mapper.Map<Bullet, BulletDto>(bullet);

        Assert.Equal(100, dto.Id);
        Assert.Equal("9mm", dto.Name);
        Assert.Equal(3, dto.Damage);
    }

    [Fact]
    public void gun_dto()
    {
        Gun.Builder builder = new Gun.Builder(new GunId(100),
                                                new GunName("Glock22"),
                                                new GunCategory(new GunCategoryId(100), new GunCategoryName("ハンドガン")),
                                                new Capacity(17));
        IEnumerable<Bullet> bullets = new List<Bullet>()
        {
            new Bullet(new BulletId(100), new BulletName("9mm"), new Damage(3)),
            new Bullet(new BulletId(200), new BulletName("45ACP"), new Damage(3)),
        };
        builder.WithBullets(bullets);

        Gun gun = builder.Build();

        GunDto dto = _mapper.Map<Gun, GunDto>(gun);

        Assert.Equal(100, dto.Id);
        Assert.Equal("Glock22", dto.Name);
        Assert.Equal(17, dto.Capacity);
        Assert.Equal("9mm", dto.Bullets.First().Name);
        Assert.Equal("45ACP", dto.Bullets.Last().Name);
    }
}
