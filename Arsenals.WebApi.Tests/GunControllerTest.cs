using System;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Infrastructure.Ef.Bullets;
using Arsenals.Infrastructure.Ef.Guns;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.WebApi.Tests;

public class GunControllerTest : BaseControllerTest
{
    public GunControllerTest(PostgreSqlTest fixture) : base(fixture) { }


    private async Task CreateInitDataAsync()
    {
        //テストデータ作成
        await _context.GunCategories.AddAsync(new GunCategoryData() { Id = 100, Name = "ハンドガン" });
        await _context.Guns.AddAsync(new GunData() { Id = 100, Name = "M1911A1", Capacity = 6, GunCategoryDataId = 100 });
        await _context.Bullets.AddAsync(new BulletData() { Id = 100, Name = "45ACP", Damage = 12 });
        _context.SaveChanges();
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
        await CreateInitDataAsync();

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
        await CreateInitDataAsync();

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
        await CreateInitDataAsync();

        using HttpResponseMessage response = await _client.DeleteAsync("/api/guns/100");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async void update_gun()
    {
        await CreateInitDataAsync();
        await _context.GunCategories.AddAsync(new GunCategoryData() { Id = 200, Name = "ライフル" });
        await _context.Bullets.AddAsync(new BulletData() { Id = 200, Name = "9mm", Damage = 9 });
        await _context.SaveChangesAsync();

        UpdateGunRequestDto request = new UpdateGunRequestDto()
        {
            Name = "Glock19",
            Category = 200,
            Capacity = 9,
            Bullets = [200]
        };
        IEnumerable<string> filedList = ["name", "category", "capacity", "bullets"];
        string fieldMask = string.Join("&", filedList.Select(x => $"fieldMask={x}"));
        using HttpResponseMessage response = await _client.PatchAsJsonAsync($"/api/guns/100?{fieldMask}", request);

        BaseResponse<GunDto?>? baseResponse = await response.Content.ReadFromJsonAsync<BaseResponse<GunDto?>>();

        Assert.NotNull(baseResponse);
        Assert.NotNull(baseResponse.Data);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var data = baseResponse.Data;

        Assert.Equal(request.Name, data.Name);
        Assert.Equal(request.Category, data.Category.Id);
        Assert.Equal(request.Capacity, data.Capacity);
        Assert.Equal(request.Bullets.First(), data.Bullets.First().Id);
    }

    [Fact]
    public async void upload_gun_image()
    {
        await CreateInitDataAsync();

        string path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Icon-assult.png");
        using MultipartFormDataContent content = new MultipartFormDataContent();
        using ByteArrayContent fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(path));
        fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
        {
            Name = "data",
            FileName = "icon.png"
        };
        content.Add(fileContent);

        using HttpResponseMessage response = await _client.PostAsync("/api/guns/100/images", content);
        BaseResponse<string>? baseResponse = await response.Content.ReadFromJsonAsync<BaseResponse<string>>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(baseResponse);
        Assert.NotNull(baseResponse.Data);
    }

}
