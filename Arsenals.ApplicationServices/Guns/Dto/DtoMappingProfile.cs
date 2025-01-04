using System;
using Arsenals.Domains.Bullets;
using Arsenals.Domains.Guns;
using Arsenals.Models;
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
        .ForMember(dst => dst.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
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

        CreateMap<Gun, GunModel>()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dst => dst.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dst => dst.Capacity, opt => opt.MapFrom(src => src.Capacity.Value))
            .ForMember(dst => dst.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl == null ? null : src.ImageUrl.ToString()))
            .ForMember(dst => dst.Bullets, opt => opt.MapFrom(src => src.UseableBullets))
            .ReverseMap();


    }
}
