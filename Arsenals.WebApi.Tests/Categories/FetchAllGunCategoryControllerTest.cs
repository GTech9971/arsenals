using System.Net;
using System.Net.Http.Json;
using Arsenals.Models;

namespace Arsenals.WebApi.Tests.Categories;

/// <summary>
/// 全カテゴリー取得APIのテスト
/// </summary>
public class FetchAllGunCategoryControllerTest : BaseControllerTest
{
    public FetchAllGunCategoryControllerTest(PostgreSqlTest fixture) : base(fixture) { }


    [Fact(DisplayName = "取得結果なし")]
    public async Task empty()
    {
        using var response = await _client.GetAsync("/api/v1/arsenals/categories");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact(DisplayName = "カテゴリー取得")]
    public async Task fetch()
    {
        await RegistryCategoryAsync();

        using var response = await _client.GetAsync("/api/v1/arsenals/categories");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        FetchGunCategoryResponseModel? responseModel = await response.Content.ReadFromJsonAsync<FetchGunCategoryResponseModel>();
        Assert.NotNull(responseModel?.Data);
        Assert.Null(responseModel.Error);

        Assert.Single(responseModel.Data);
    }
}
