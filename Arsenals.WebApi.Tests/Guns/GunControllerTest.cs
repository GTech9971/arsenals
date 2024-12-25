using System;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Infrastructure.Ef.Bullets;
using Arsenals.Infrastructure.Ef.Guns;
using Arsenals.Models;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.WebApi.Tests;

public class GunControllerTest : BaseControllerTest
{
    public GunControllerTest(PostgreSqlTest fixture) : base(fixture) { }


    private async Task CreateInitDataAsync()
    {
        //テストデータ作成
        await _context.GunCategories.AddAsync(new GunCategoryData() { Id = "C-0001", Name = "ハンドガン" });
        await _context.Guns.AddAsync(new GunData() { Id = "G-0001", Name = "M1911A1", Capacity = 6, GunCategoryDataId = "C-0001" });
        await _context.Bullets.AddAsync(new BulletData() { Id = "B-0001", Name = "45ACP", Damage = 12 });
        _context.SaveChanges();
    }

    [Fact]
    public async void fetch_guns_empty()
    {
        using HttpResponseMessage response = await _client.GetAsync("/api/v1/arsenals/guns");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async void registry_gun()
    {
        await CreateInitDataAsync();

        RegistryGunRequestModel requestDto = new RegistryGunRequestModel()
        {
            Name = "Glock22",
            CategoryId = "C-0001",
            Capacity = 17,
            UseBullets = ["B-0001"]
        };

        using HttpResponseMessage response = await _client.PostAsJsonAsync<RegistryGunRequestModel>("/api/v1/arsenals/guns", requestDto);

        RegistryGunResponseModel? baseResponse = await response.Content.ReadFromJsonAsync<RegistryGunResponseModel>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(baseResponse);
        Assert.NotNull(baseResponse.Data);
        Assert.Equal("G-0002", baseResponse.Data.Id);
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
        await _context.GunCategories.AddAsync(new GunCategoryData() { Id = "C-2000", Name = "ライフル" });
        await _context.Bullets.AddAsync(new BulletData() { Id = "B-2000", Name = "9mm", Damage = 9 });
        await _context.SaveChangesAsync();

        UpdateGunRequestDto request = new UpdateGunRequestDto()
        {
            Name = "Glock19",
            Category = "C-2000",
            Capacity = 9,
            Bullets = ["B-2000"]
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
        // Assert.Equal(request.Category, data.Category.Id);
        Assert.Equal(request.Capacity, data.Capacity);
        // Assert.Equal(request.Bullets.First(), data.Bullets.First().Id);
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
