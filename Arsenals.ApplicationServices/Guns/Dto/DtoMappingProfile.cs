using Arsenals.Domains.Bullets;
using Arsenals.Domains.Guns;
using Arsenals.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace Arsenals.ApplicationServices.Guns.Dto;

public class DtoMappingProfile : Profile
{

    public DtoMappingProfile() { }

    public DtoMappingProfile(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        CreateMap<Bullet, BulletDto>()
        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id.Value))
        .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name.Value))
        .ForMember(dst => dst.Damage, opt => opt.MapFrom(src => src.Damage.Value));

        CreateMap<GunCategory, GunCategoryDto>()
        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id.Value))
        .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name.Value));

        CreateMap<Gun, GunDto>()
        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id.Value))
        .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name.Value))
        .ForMember(dst => dst.Capacity, opt => opt.MapFrom(src => src.Capacity.Value))
        .ForMember(dst => dst.ImageUrl, opt => opt.Ignore())
        .ForMember(dst => dst.Bullets, opt => opt.MapFrom(src => src.UseableBullets));



        CreateMap<GunCategory, GunCategoryModel>()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ReverseMap();

        CreateMap<Bullet, BulletModel>()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dst => dst.Damage, opt => opt.MapFrom(src => src.Damage.Value))
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ReverseMap();

        string? downloadRoot = configuration.GetGunImageDownloadUrl();

        CreateMap<Gun, GunModel>()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dst => dst.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dst => dst.Capacity, opt => opt.MapFrom(src => src.Capacity.Value))
            .ForMember(dst => dst.ImageUrl, opt => opt.MapFrom(src => src.Image == null
                                                                        ? null
                                                                        : src.Image.DownloadUrl(src.Id, downloadRoot!)))
            .ForMember(dst => dst.Bullets, opt => opt.MapFrom(src => src.UseableBullets))
            .ReverseMap();
    }
}
