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
            .ConstructUsing(src => new GunCategory(new GunCategoryId(src.Id), new GunCategoryName(src.Name)))
            .ReverseMap()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name.Value));

        CreateMap<GunImageData, GunImage>()
            .ConstructUsing(src => new GunImage(src.Id, src.Extension))
            .ReverseMap()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.Extension, opt => opt.MapFrom(src => src.Extension));

        CreateMap<BulletData, Bullet>()
            .ConstructUsing(src => new Bullet(new BulletId(src.Id), new BulletName(src.Name), new Damage(src.Damage)))
            .ReverseMap()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dst => dst.Damage, opt => opt.MapFrom(src => src.Damage.Value));

        CreateMap<GunData, Gun>()
            .ConstructUsing(src => new Gun(new GunId(src.Id),
                                            new GunName(src.Name),
                                            new GunCategory(new GunCategoryId(src.GunCategoryDataId),
                                                            new GunCategoryName(src.GunCategoryData.Name)),
                                            new Capacity(src.Capacity),
                                            src.BulletDataList == null
                                                ? Enumerable.Empty<Bullet>()
                                                : src.BulletDataList.Select(x => new Bullet(new BulletId(x.Id), new BulletName(x.Name), new Damage(x.Damage))),
                                            src.GunImageData == null
                                                ? null
                                                : new GunImage(src.GunImageData.Id, src.GunImageData.Extension)))
            .ReverseMap()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dst => dst.Capacity, opt => opt.MapFrom(src => src.Capacity.Value))
            .ForMember(dst => dst.BulletDataList, opt => opt.Ignore()) //マッピングしてしまうとインスタンスが新規作成されるためEFCoreで新規データ登録とみなされる
            .ForMember(dst => dst.GunCategoryDataId, opt => opt.MapFrom(src => src.Category.Id.Value))
            .ForMember(dst => dst.GunCategoryData, opt => opt.Ignore()) //マッピングしてしまうとインスタンスが新規作成されるためEFCoreで新規データ登録とみなされる
            .ForMember(dst => dst.GunImageDataId, opt => opt.MapFrom(src => src.Image == null
                                                                                ? (int?)null
                                                                                : src.Image.Id))
            .ForMember(dst => dst.GunImageData, opt => opt.Ignore()); //マッピングしてしまうとインスタンスが新規作成されるためEFCoreで新規データ登録とみなされる
    }
}
