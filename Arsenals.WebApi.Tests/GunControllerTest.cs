using System;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Guns.Dto;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Arsenals.WebApi.Tests;

[Collection("TestContainer Collection")]
public class GunControllerTest : IClassFixture<PostgreSqlTest>, IDisposable
{

    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public GunControllerTest(PostgreSqlTest fixture)
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
    public async void fetch_guns_empty()
    {
        using HttpResponseMessage response = await _client.GetAsync("/api/guns");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async void registry_gun()
    {
        RegistryGunRequestDto requestDto = new RegistryGunRequestDto()
        {
            Name = "Glock22",
            CategoryId = 100,
            Capacity = 17,
            UseBullets = [100]
        };

        using HttpResponseMessage response = await _client.PostAsJsonAsync<RegistryGunRequestDto>("/api/guns", requestDto);

        BaseResponse<RegistryGunResponseDto>? baseResponse = await response.Content.ReadFromJsonAsync<BaseResponse<RegistryGunResponseDto>>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(baseResponse);
        Assert.NotNull(baseResponse.Data);
        Assert.Equal(200, baseResponse.Data.Id);
    }

    [Theory]
    [InlineData(100, HttpStatusCode.OK)]
    [InlineData(999, HttpStatusCode.NotFound)]
    public async void fetch_gun(int gunId, HttpStatusCode expectedStatusCode)
    {
        using HttpResponseMessage response = await _client.GetAsync($"/api/guns/{gunId}");
        BaseResponse<GunDto>? baseResponse = await response.Content.ReadFromJsonAsync<BaseResponse<GunDto>>();

        Assert.Equal(expectedStatusCode, response.StatusCode);
        Assert.NotNull(baseResponse);

        if (expectedStatusCode == HttpStatusCode.OK)
        {
            Assert.NotNull(baseResponse.Data);
            Assert.True(baseResponse.Success);
        }
        else
        {
            Assert.False(baseResponse.Success);
            Assert.NotNull(baseResponse.Message);
        }
    }

    [Fact]
    public async void delete_gun()
    {
        using HttpResponseMessage response = await _client.DeleteAsync("/api/guns/100");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

}
