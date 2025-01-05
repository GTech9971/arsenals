using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Arsenals.ApplicationServices.Users;
using Arsenals.Domains.Guns;
using Arsenals.Infrastructure.Ef;
using Arsenals.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arsenals.WebApi.Tests;

public class BaseControllerTest : IClassFixture<PostgreSqlTest>, IDisposable
{
    protected readonly WebApplicationFactory<Program> _factory;
    protected readonly HttpClient _client;

    protected readonly ArsenalDbContext _context;

    public BaseControllerTest(PostgreSqlTest fixture)
    {
        var options = new WebApplicationFactoryClientOptions()
        {
            AllowAutoRedirect = true
        };
        _factory = new CustomWebApplicationFactory(fixture);
        _client = _factory.CreateClient(options);
        _context = fixture.ArsenalDbContext;

        using var scope = _factory.Services.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        string? root = configuration[GunImage.ROOT_KEY]!;
        if (Directory.Exists(root))
        {
            Directory.Delete(root, recursive: true);
        }
    }

    public void Dispose()
    {
        _factory.Dispose();
    }

    protected async Task LoginAsync()
    {
        LoginRequestDto requestDto = new LoginRequestDto()
        {
            UserId = "test",
            Password = "pass"
        };

        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/users", requestDto);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        BaseResponse<LoginResponseDto>? baseResponse = await response.Content.ReadFromJsonAsync<BaseResponse<LoginResponseDto>>();

        Assert.NotNull(baseResponse?.Data?.Token);

        // Jwt設定
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", baseResponse.Data.Token);
    }

    #region カテゴリー

    /// <summary>
    /// カテゴリー登録
    /// </summary>
    /// <returns></returns>
    protected async Task<string> RegistryCategoryAsync(string name = "ライフル")
    {
        RegistryGunCategoryRequestModel request = new RegistryGunCategoryRequestModel() { Name = name };

        using HttpResponseMessage response = await _client.PostAsJsonAsync("/api/v1/arsenals/categories", request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        RegistryGunCategoryResponseModel? baseResponse = await response.Content.ReadFromJsonAsync<RegistryGunCategoryResponseModel>();
        Assert.NotNull(baseResponse?.Data);

        return baseResponse.Data.Id;
    }

    #endregion

    #region 弾丸

    /// <summary>
    /// 弾丸登録
    /// </summary>
    /// <param name="bulletName"></param>
    /// <returns></returns>
    protected async Task<string> RegistryBulletAsync(string bulletName = "9mm")
    {
        RegistryBulletRequestModel request = new RegistryBulletRequestModel()
        {
            Name = bulletName,
            Damage = 3
        };

        using HttpResponseMessage response = await _client.PostAsJsonAsync("/api/v1/arsenals/bullets", request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        RegistryBulletResponseModel? responseModel = await response.Content.ReadFromJsonAsync<RegistryBulletResponseModel>();
        Assert.NotNull(responseModel?.Data);
        Assert.Equal($"bullets/{responseModel.Data.Id}", response.Headers.Location?.ToString());

        return responseModel.Data.Id;
    }

    #endregion

    #region 銃


    /// <summary>
    /// 銃登録
    /// </summary>
    /// <param name="gunName"></param>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    protected async Task<string> RegistryGunAsync(string gunName, string categoryId)
    {
        RegistryGunRequestModel request = new RegistryGunRequestModel()
        {
            Name = gunName,
            Capacity = 9,
            CategoryId = categoryId,
            UseBullets = []
        };

        using HttpResponseMessage response = await _client.PostAsJsonAsync("/api/v1/arsenals/guns", request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        RegistryGunResponseModel? responseModel = await response.Content.ReadFromJsonAsync<RegistryGunResponseModel>();
        Assert.NotNull(responseModel?.Data);

        return responseModel.Data.Id;
    }

    #endregion
}
