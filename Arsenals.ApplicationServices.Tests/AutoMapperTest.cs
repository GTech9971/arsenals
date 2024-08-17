using Arsenals.ApplicationServices.Guns;
using Arsenals.Domains.Guns;
using AutoMapper;

namespace Arsenals.ApplicationServices.Tests;

public class AutoMapperTest
{
    [Fact]
    public void fetch_category_dto()
    {
        var config = new MapperConfiguration(c =>
        {
            c.AddProfile<FetchGunCategoryResponseDtoMappingProfile>();
        });
        IMapper mapper = config.CreateMapper();

        GunCategory category = new GunCategory(new GunCategoryId(100), new GunCategoryName("ハンドガン"));

        FetchGunCategoryResponseDto dto = mapper.Map<FetchGunCategoryResponseDto>(category);

        Assert.Equal(100, dto.Id);
        Assert.Equal("ハンドガン", dto.Name);
    }
}
