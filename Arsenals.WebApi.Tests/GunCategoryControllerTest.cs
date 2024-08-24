using System.Net;
using System.Net.Http.Json;
using Arsenals.ApplicationServices.Guns;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Arsenals.WebApi.Tests;

[Collection("TestContainer Gun Category Controller")]
public class GunCategoryControllerTest : IClassFixture<PostgreSqlTest>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public GunCategoryControllerTest(PostgreSqlTest fixture)
    {
        var options = new WebApplicationFactoryClientOptions()
        {
            AllowAutoRedirect = true
        };
        _factory = new CustomWebApplicationFactory(fixture);
        _client = _factory.CreateClient(options);
    }

    public void Dispose()
    {
        _factory.Dispose();
    }

    [Fact]
    public async void registry_gun_category()
    {
        RegistryGunCategoryRequestDto request = new RegistryGunCategoryRequestDto()
        {
            Name = "ライフル"
        };

        using HttpResponseMessage response = await _client.PostAsJsonAsync("/api/categories", request);


        BaseResponse<RegistryGunCategoryResponseDto>? baseResponse = await response.Content.ReadFromJsonAsync<BaseResponse<RegistryGunCategoryResponseDto>>();


        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(baseResponse);
        Assert.NotNull(baseResponse.Data);
        Assert.Equal(200, baseResponse.Data.Id);
    }
}
