using System.Net;
using System.Net.Http.Json;
using Arsenals.Models;

namespace Arsenals.WebApi.Tests.Guns;

public class FetchGunsControllerTest : BaseControllerTest
{
    public FetchGunsControllerTest(PostgreSqlTest fixture) : base(fixture) { }

    [Fact(DisplayName = "0件")]
    public async Task no_content()
    {
        using HttpResponseMessage response = await _client.GetAsync("/v1/guns");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact(DisplayName = "1件以上取得")]
    public async Task fetch_any()
    {
        string categoryId = await RegistryCategoryAsync();
        await RegistryGunAsync(gunName: "M1911A1", categoryId);
        await RegistryGunAsync(gunName: "G3A1", categoryId);

        using HttpResponseMessage response = await _client.GetAsync("/v1/guns");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        FetchGunsResponseModel? responseModel = await response.Content.ReadFromJsonAsync<FetchGunsResponseModel>();
        Assert.NotNull(responseModel?.Data);
        Assert.Null(responseModel.Error);
        Assert.Equal(2, responseModel.Data.Count);
    }

    [Theory(DisplayName = "カテゴリー検索")]
    [InlineData("C-0001", 1)]
    [InlineData("C-0002", 2)]
    public async Task fetch_by_category_id(string categoryIdVal, int expectedCount)
    {
        string firstCategoryId = await RegistryCategoryAsync(name: "ライフル");
        string secondCategoryId = await RegistryCategoryAsync(name: "ハンドガン");

        await RegistryGunAsync(gunName: "A", firstCategoryId);
        await RegistryGunAsync(gunName: "B", secondCategoryId);
        await RegistryGunAsync(gunName: "C", secondCategoryId);

        using HttpResponseMessage response = await _client.GetAsync($"/v1/guns?category={categoryIdVal}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        FetchGunsResponseModel? responseModel = await response.Content.ReadFromJsonAsync<FetchGunsResponseModel>();
        Assert.NotNull(responseModel?.Data);
        Assert.Null(responseModel.Error);
        Assert.Equal(expectedCount, responseModel.Data.Count);
    }
}
