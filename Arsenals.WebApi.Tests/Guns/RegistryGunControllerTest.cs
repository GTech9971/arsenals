using System.Net;
using System.Net.Http.Json;
using Arsenals.Models;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.WebApi.Tests.Guns;

public class RegistryGunControllerTest : BaseControllerTest
{
    public RegistryGunControllerTest(PostgreSqlTest fixture) : base(fixture) { }

    [Fact(DisplayName = "登録成功")]
    public async void success()
    {
        string categoryId = await RegistryCategoryAsync();
        string bulletId = await RegistryBulletAsync();

        RegistryGunRequestModel request = new RegistryGunRequestModel()
        {
            Name = "berettaM9",
            Capacity = 9,
            CategoryId = categoryId,
            UseBullets = [bulletId]
        };

        using HttpResponseMessage response = await _client.PostAsJsonAsync("/v1/guns", request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        RegistryGunResponseModel? responseModel = await response.Content.ReadFromJsonAsync<RegistryGunResponseModel>();
        Assert.NotNull(responseModel?.Data);
        Assert.Equal($"guns/{responseModel.Data.Id}", response.Headers.Location?.ToString());
        Assert.Equal("G-0001", responseModel.Data.Id);

        var gun = await _context.Guns
                                    .AsNoTracking()
                                    .Include(x => x.BulletDataList)
                                    .SingleAsync(x => x.Id == responseModel.Data.Id);

        Assert.Equal(request.Capacity, gun.Capacity);
        Assert.Equal(request.CategoryId, gun.GunCategoryDataId);
        Assert.Equal(request.UseBullets, gun.BulletDataList.Select(x => x.Id));
    }

    [Fact(DisplayName = "銃の名前が被っている")]
    public async void duplicate_name()
    {
        string categoryId = await RegistryCategoryAsync();
        string bulletId = await RegistryBulletAsync();
        await RegistryGunAsync(gunName: "berettaM9", categoryId);

        RegistryGunRequestModel request = new RegistryGunRequestModel()
        {
            Name = "berettaM9",
            Capacity = 9,
            CategoryId = categoryId,
            UseBullets = [bulletId]
        };

        using HttpResponseMessage response = await _client.PostAsJsonAsync("/v1/guns", request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        RegistryGunResponseModel? responseModel = await response.Content.ReadFromJsonAsync<RegistryGunResponseModel>();
        Assert.NotNull(responseModel?.Error);
    }

    [Fact(DisplayName = "カテゴリーが存在しない")]
    public async void not_found_category()
    {
        RegistryGunRequestModel request = new RegistryGunRequestModel()
        {
            Name = "berettaM9",
            Capacity = 9,
            CategoryId = "C-1000",
            UseBullets = []
        };

        using HttpResponseMessage response = await _client.PostAsJsonAsync("/v1/guns", request);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        RegistryGunResponseModel? responseModel = await response.Content.ReadFromJsonAsync<RegistryGunResponseModel>();
        Assert.NotNull(responseModel?.Error);
    }


    [Fact(DisplayName = "弾丸が存在しない")]
    public async void not_found_bullet()
    {
        string categoryId = await RegistryCategoryAsync();

        RegistryGunRequestModel request = new RegistryGunRequestModel()
        {
            Name = "berettaM9",
            Capacity = 9,
            CategoryId = categoryId,
            UseBullets = ["B-1000"]
        };

        using HttpResponseMessage response = await _client.PostAsJsonAsync("/v1/guns", request);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        RegistryGunResponseModel? responseModel = await response.Content.ReadFromJsonAsync<RegistryGunResponseModel>();
        Assert.NotNull(responseModel?.Error);
    }

}
