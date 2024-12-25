using System.Net;
using System.Net.Http.Json;
using Arsenals.Models;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.WebApi.Tests.Categories;

public class RegistryGunCategoryControllerTest : BaseControllerTest
{
    public RegistryGunCategoryControllerTest(PostgreSqlTest fixture) : base(fixture) { }

    [Fact(DisplayName = "データなし")]
    public async void empty()
    {
        using HttpResponseMessage response = await _client.GetAsync("/api/v1/arsenals/categories");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact(DisplayName = "銃カテゴリー登録")]
    public async void registry_gun_category()
    {
        RegistryGunCategoryRequestModel request = new RegistryGunCategoryRequestModel()
        {
            Name = "ライフル"
        };

        using HttpResponseMessage response = await _client.PostAsJsonAsync("/api/v1/arsenals/categories", request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        RegistryGunCategoryResponseModel? baseResponse = await response.Content.ReadFromJsonAsync<RegistryGunCategoryResponseModel>();
        Assert.NotNull(baseResponse?.Data);
        Assert.Null(baseResponse.Error);
        Assert.Equal("C-0001", baseResponse.Data.Id);
        Assert.Equal("/categories/C-0001", response.Headers.Location?.ToString());

        var category = await _context.GunCategories
                                        .AsNoTracking()
                                        .SingleAsync();

        Assert.Equal("C-0001", category.Id);
        Assert.Equal(request.Name, category.Name);
    }
}
