using System.Net;

namespace Arsenals.WebApi.Tests.Categories;

/// <summary>
/// 銃カテゴリー削除APIのテスト
/// </summary>
public class DeleteGunCategoryControllerTest : BaseControllerTest
{
    private readonly PostgreSqlTest _fixture;

    public DeleteGunCategoryControllerTest(PostgreSqlTest fixture) : base(fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "存在しない")]
    public async void not_found()
    {
        using HttpResponseMessage response = await _client.DeleteAsync("/api/v1/arsenals/categories/C-9999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(DisplayName = "カテゴリー削除")]
    public async void delete()
    {
        string categoryId = await RegistryCategoryAsync();
        using HttpResponseMessage response = await _client.DeleteAsync($"/api/v1/arsenals/categories/{categoryId}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Empty(_fixture.ArsenalDbContext.GunCategories);
    }
}
