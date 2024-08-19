using Arsenals.Domains.Bullets;
using Arsenals.Domains.Guns;
using Arsenals.Infrastructure.Ef.Bullets;
using Arsenals.Infrastructure.Ef.Guns;
using AutoMapper;

namespace Arsenals.Infrastructure.Ef;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<GunCategoryData, GunCategory>()
        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => new GunCategoryId(src.Id)))
        .ForMember(dst => dst.Name, opt => opt.MapFrom(src => new GunCategoryName(src.Name)));

        CreateMap<BulletData, Bullet>()
        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => new BulletId(src.Id)))
        .ForMember(dst => dst.Name, opt => opt.MapFrom(src => new BulletName(src.Name)))
        .ForMember(dst => dst.Damage, opt => opt.MapFrom(src => new Damage(src.Damage)));

        CreateMap<GunData, Gun>()
        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => new GunId(src.Id)))
        .ForMember(dst => dst.Name, opt => opt.MapFrom(src => new GunName(src.Name)))
        .ForMember(dst => dst.Capacity, opt => opt.MapFrom(src => new Capacity(src.Capacity)))
        .ForMember(dst => dst.UseableBullets, opt => opt.MapFrom(src => src.BulletDataList));
    }
}
