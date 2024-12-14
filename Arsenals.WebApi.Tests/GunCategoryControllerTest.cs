using System.Net;
using System.Net.Http.Json;
using Arsenals.ApplicationServices.Guns;

namespace Arsenals.WebApi.Tests;

public class GunCategoryControllerTest : BaseControllerTest
{
    public GunCategoryControllerTest(PostgreSqlTest fixture) : base(fixture) { }

    [Fact]
    public async void fetch()
    {
        using HttpResponseMessage response = await _client.GetAsync("/api/categories");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async void registry_gun_category()
    {
        RegistryGunCategoryRequestDto request = new RegistryGunCategoryRequestDto()
        {
            Name = "ライフル"
        };

        await LoginAsync();

        using HttpResponseMessage response = await _client.PostAsJsonAsync("/api/categories", request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        BaseResponse<RegistryGunCategoryResponseDto>? baseResponse = await response.Content.ReadFromJsonAsync<BaseResponse<RegistryGunCategoryResponseDto>>();

        Assert.NotNull(baseResponse);
        Assert.NotNull(baseResponse.Data);
        Assert.Equal(100, baseResponse.Data.Id);
    }
}
