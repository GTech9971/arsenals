using System;
using Arsenals.Domains.Bullets;
using Arsenals.Domains.Guns;
using AutoMapper;

namespace Arsenals.ApplicationServices.Guns.Dto;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
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
        .ForMember(dst => dst.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));
    }
}
